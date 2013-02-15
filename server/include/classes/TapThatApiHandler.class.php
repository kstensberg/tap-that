<?php
require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/TapThatTapsJson.class.php");
require_once ( __DIR__ . "/JsonResponse/TapThatLeaderboardJson.class.php");
require_once ( __DIR__ . "/JsonResponse/ErrorJson.class.php");

class TapThatApiHandler extends ApiHander
{
	public function Init(ApiMediator $mediator)
	{
		$mediator->AttachRoute('tapthat/taps', $this, 'GetTapsResponse', true);
		$mediator->AttachRoute('tapthat/leaderboard', $this, 'GetLeaderboardResponse', false);
	}
	
	public function GetLeaderboardResponse()
	{
		$response = array();
		$top = 10;
		
		if (array_key_exists('top', $_POST)) {
			$top = intval($_POST['top']);
		}
		
		$sql = "SELECT
				totalTaps,
				@curRow := @curRow + 1 AS rank,
				userId,
				users.name AS name
			FROM (
				SELECT  
					userId,
					SUM(delta) AS totalTaps
				FROM `tapthat-deltas` AS deltas
				
				GROUP BY userId
				ORDER BY totalTaps DESC
			) AS innerQuery
			JOIN (SELECT @curRow := 0) r
			JOIN users AS users ON userId = users.id
			ORDER BY rank
			LIMIT ?";
		
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$stmt->bind_param('i', $top);

		$stmt->execute();
		$stmt->bind_result($totalTaps, $rank, $userId, $name);
		$stmt->store_result();
		
		while($stmt->fetch()) {
			$leaderboard = new TapThatLeaderboardJson();
			
			$leaderboard->rank = intval($rank);
			$leaderboard->totalTaps = intval($totalTaps);
			$leaderboard->name = $name;
			$leaderboard->delta = $this->GetLastDeltaFromUser(intval($userId));
			
			if ($leaderboard->delta instanceof ErrorJson) {
				return $leaderboard->delta;
			}
			
			array_push($response, $leaderboard);
		}
		
		$stmt->reset();
		
		return $response;
	}
	
	public function GetTapsResponse()
	{	
		$userId = $_SESSION['userId'];
		
		if (array_key_exists('delta', $_POST)) {
			$newDeltaResponse = $this->AddNewDelta($userId, intval($_POST['delta']));
			
			if ($newDeltaResponse instanceof JsonError) {
				return $newDeltaResponse;
			}
		}
		
		$near = 1;
		
		if (array_key_exists('near', $_POST)) {
			$near = intval($_POST['near']);
		}
		
		$response = new TapThatTapsJson();
		
		$sql = "SELECT
					totalTaps,
					rank
				FROM (
					SELECT
						totalTaps,
						@curRow := @curRow + 1 AS rank,
						userId
					FROM (
						SELECT  
							userId,
							SUM(delta) AS totalTaps
						FROM `tapthat-deltas` AS deltas
						
						GROUP BY userId
						ORDER BY totalTaps DESC
					) AS innerInnerQuery
					JOIN (SELECT @curRow := 0) r
				) AS innerQuery
				WHERE userId = ?";
		
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$stmt->bind_param('i', $userId);

		$stmt->execute();
		$stmt->bind_result($currentUserTotalTaps, $currentUserRank);
		$stmt->fetch();
		$stmt->reset();
		
		$rankRangeLowest = $currentUserRank - $near;
		$rankRangeHighest = $currentUserRank + $near;
		
		$response->rank = intval($currentUserRank);
		$response->totalTaps = intval($currentUserTotalTaps);
		$response->leaderboard = array();
		
		$sql = "SELECT 
					i.totalTaps,
					i.rank,
					i.userId,
					u.name
				FROM (
					SELECT
						totalTaps,
						@curRow := @curRow + 1 AS rank,
						userId
					FROM (
						SELECT  
							userId,
							SUM(delta) AS totalTaps
						FROM `tapthat-deltas` AS deltas
						
						GROUP BY userId
						ORDER BY totalTaps DESC
					) AS innerQuery
					JOIN (SELECT @curRow := 0) r
				) AS i
				JOIN users AS u ON u.id = i.userId
				WHERE rank >= ? AND rank <= ? AND rank != ?";
		
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		var_dump($currentUserRank);
		
		$stmt->bind_param('iii', $rankRangeLowest, $rankRangeHighest, $currentUserRank);

		$stmt->execute();
		$stmt->bind_result($totalTaps, $rank, $userId, $name);
		$stmt->store_result();
		
		while($stmt->fetch()) {
			$leaderboard = new TapThatLeaderboardJson();
			$leaderboard->rank = intval($rank);
			$leaderboard->totalTaps = intval($totalTaps);
			$leaderboard->delta = $this->GetLastDeltaFromUser($userId);
			$leaderboard->name = $name;
			
			if ($leaderboard->delta instanceof JsonError) {
				return $leaderboard->delta;
			}
			
			if ($leaderboard->name instanceof JsonError) {
				return $leaderboard->name;
			}
			
			$leaderboard->delta = intval($leaderboard->delta);
			
			array_push($response->leaderboard, $leaderboard);
		}
		
		$stmt->reset();
		
		return $response;
	}
	
	private function GetLastDeltaFromUser($userId)
	{
		//TODO: why is this here?  shouldn't we just join?
		
		$sql = "SELECT delta FROM `tapthat-deltas` where userId = ? ORDER BY timestamp DESC LIMIT 1";
		
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql: ' . $stmt->error);
		}
		
		$stmt->bind_param('i', $userId);

		$stmt->execute();
		$stmt->bind_result($delta);
		$stmt->fetch();
		$stmt->reset();
		
		return $delta;
	}
	
	private function AddNewDelta($userId, $delta)
	{
		if ($delta <= 0)
			return;
		
		$sql = "INSERT INTO `tapthat-deltas` (userId, delta) VALUES (?, ?)";
			
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$stmt->bind_param('ii', $userId, $delta);

		$stmt->execute();
		$stmt->reset();
	}
}

?>