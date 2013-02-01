<?php
require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/TapThatLeaderboardJson.class.php");
require_once ( __DIR__ . "/JsonResponse/NearRankEntry.class.php");
require_once ( __DIR__ . "/JsonResponse/ErrorJson.class.php");

class TapThatApiHandler extends ApiHander
{
	public function Init(ApiMediator $mediator)
	{
		$mediator->AttachRoute('tapthat/leaderboard', $this, 'GetLeaderboardResponse');
	}
	
	public function GetLeaderboardResponse()
	{
		if (array_key_exists('authToken', $_POST)) {
			session_id($_POST['authToken']);
		}
		
		session_start();
		
		if (count($_SESSION) <= 0) {
			return new ErrorJson('access denied', 'no session exists');
		} else if (!array_key_exists('userId', $_SESSION)) {
			return new ErrorJson('access denied', 'user id not found in session');
		}
		
		$userId = $_SESSION['userId'];
		
		if (array_key_exists('delta', $_POST)) {
			$newDeltaResponse = $this->AddNewDelta($userId, intval($_POST['delta']));
			
			if ($newDeltaResponse instanceof JsonError) {
				return $newDeltaResponse;
			}
		}
		
		$response = new TapThatLeaderboardJson();
		
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
		$stmt->bind_result($currentUsuerTotalTaps, $currentUserRank);
		$stmt->fetch();
		$stmt->reset();
		
		$response->rank = intval($currentUserRank);
		$response->totalTaps = intval($currentUsuerTotalTaps);
		$response->nearRank = array();
		
		$sql = "SELECT
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
			JOIN (SELECT @curRow := 0) r";
			
		$result = $this->mysql->query($sql);
		while($row = $result->fetch_assoc()) {
			$nearRank = new NearRankEntry();
			$nearRank->rank = intval($row['rank']);
			$nearRank->totalTaps = intval($row['totalTaps']);
			$nearRank->delta = $this->GetLastDeltaFromUser($row['userId']);
			$nearRank->name = $this->GetNameForUser($row['userId']);
			
			if ($nearRank->delta instanceof JsonError) {
				return $nearRank->delta;
			}
			
			if ($nearRank->name instanceof JsonError) {
				return $nearRank->name;
			}
			
			$nearRank->delta = intval($nearRank->delta);
			
			array_push($response->nearRank, $nearRank);
		}
		
		return $response;
	}
	
	private function GetLastDeltaFromUser($userId)
	{
		$sql = "SELECT delta FROM `tapthat-deltas` where userId = ? ORDER BY timestamp DESC LIMIT 1";
			
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$stmt->bind_param('i', $userId);

		$stmt->execute();
		$stmt->bind_result($delta);
		$stmt->fetch();
		$stmt->reset();
		
		return $delta;
	}
	
	private function GetNameForUser($userId)
	{
		$sql = "SELECT name FROM users where id = ? LIMIT 1";
			
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$stmt->bind_param('i', $userId);

		$stmt->execute();
		$stmt->bind_result($nameForUser);
		$stmt->fetch();
		$stmt->reset();
		
		return $nameForUser;
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