<?php

require_once ( __DIR__ . "/../lib/passwordhash.class.php");
require_once ( __DIR__ . "/ApiMediator.class.php");
require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/UserAuthJson.class.php");
require_once ( __DIR__ . "/JsonResponse/ErrorJson.class.php");

class UserApiHandler extends ApiHander
{
	public function Init(ApiMediator $mediator)
	{
		$mediator->AttachRoute('user/auth', $this, 'GetAuthResponse');
		$mediator->AttachRoute('user/create', $this, 'GetCreateResponse');
	}
	
	public function GetAuthResponse()
	{
		if (!array_key_exists('email', $_POST) || !array_key_exists('password', $_POST)) {
			return new ErrorJson('email or password is incorrect', 'email or password not given in the post');
		}
		
		$email = $_POST["email"];
		$password = $_POST["password"];
		
		if (strlen($password) > 72 && strlen($email) > 72) { 
			return new ErrorJson('email or password is incorrect', 'email or password is over the max length');
		}

		$stored_hash = "*";

		$sql = "SELECT password, id, name FROM users WHERE email=? LIMIT 1;";
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('email or password is incorrect', 'error preparing sql');
		}
		
		$stmt->bind_param('s', $email);

		$stmt->execute();
		$stmt->bind_result($stored_hash, $id, $name);
		$stmt->fetch();
		$stmt->reset();

		if ($this->GetHasher()->CheckPassword($password, $stored_hash)) {
			session_start();
			$_SESSION['userId'] = $id;
			
			$response = new UserAuthJson();
			$response->authToken = session_id();
			$response->userId = intval($id);
			$response->name = $name;
		} else {
			$response = new ErrorJson('Username or password is incorrect');
		}
		
		return $response;
	}
	
	public function GetCreateResponse()
	{
		if (!array_key_exists('email', $_POST)) {
			return new ErrorJson("post variable 'email' was not provided");
		}
		
		if (!array_key_exists('password', $_POST)) {
			return new ErrorJson("post variable 'password' was not provided");
		}
		
		if (!array_key_exists('name', $_POST)) {
			return new ErrorJson("post variable 'name' was not provided");
		}
		
		$email = $_POST['email'];
		$password = $_POST['password'];
		$name = $_POST['name'];
		
		$sql = "INSERT INTO `users` (email, password, name) VALUES (?, ?, ?)";
			
		$stmt = $this->mysql->prepare($sql);
		
		if ($stmt === false) {
			return new ErrorJson('internal error, please try again later', 'error preparing sql');
		}
		
		$passwordHash = $this->GetHasher()->HashPassword($password);
		
		$stmt->bind_param('sss', $email, $passwordHash, $name);

		if ($stmt->execute() == false) {
			return new ErrorJson('internal error, please try again later', 'error executing sql');
		}
		
		$stmt->reset();
		
		return $this->GetAuthResponse();
		
	}
	
	private static function GetHasher()
	{
		return new PasswordHash(8, false);
	}
}

?>