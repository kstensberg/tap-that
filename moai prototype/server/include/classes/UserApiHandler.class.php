<?php

require_once ( __DIR__ . "/../lib/passwordhash.class.php");
require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/UserAuthJson.class.php");
require_once ( __DIR__ . "/JsonResponse/ErrorJson.class.php");

class UserApiHandler extends ApiHander
{
	public function GetResponse($route)
	{
		$username = $_POST["username"];
		$password = $_POST["password"];
		
		if (strlen($password) > 72 && strlen($username) > 72) { 
			return new ErrorJson('Username or password is incorrect', 'username or password is over the max length');
		}

		$stored_hash = "*";

		$sql = "SELECT password, id FROM users WHERE username=? LIMIT 1;";
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('Username or password is incorrect', 'error preparing sql');
		}
		
		$stmt->bind_param('s', $username);

		$stmt->execute();
		$stmt->bind_result($stored_hash, $id);
		$stmt->fetch();
		$stmt->reset();

		if ($this->GetHasher()->CheckPassword($password, $stored_hash)) {
			
			$totalTaps = 0;
			
			$sql = "SELECT SUM(delta) FROM `tapthat-deltas` WHERE userId=?";
			$stmt = $this->mysql->prepare($sql);
			
			if ($stmt === false) {
				return new ErrorJson('Username or password is incorrect', 'error preparing sql');
			}
			
			$stmt->bind_param('i', $id);

			$stmt->execute();
			$stmt->bind_result($taps);
			$stmt->fetch();
			$stmt->reset();
			
			if ($taps != null) {
				$totalTaps = $taps;
			}
			
			session_start();
			$_SESSION['userId'] = $id;
			
			$response = new UserAuthJson();
			$response->authToken = session_id();
			$response->userId = $id;
			$response->totalTaps = $totalTaps;
			
		} else {
			$response = new ErrorJson('Username or password is incorrect');
		}
		
		
		return $response;
	}
	
	public static function GetHasher()
	{
		return new PasswordHash(8, false);
	}
}

?>