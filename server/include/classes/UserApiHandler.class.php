<?php

require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/UserAuthJson.class.php");

class UserApiHandler extends ApiHander
{
	public function GetResponse($route)
	{
		$response = new UserAuthJson();
		$response->uid = 1234;
		$response->totalTaps = 47238473843;
		
		return $response;
	}
}

?>