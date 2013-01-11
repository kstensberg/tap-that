<?php
require_once ( __DIR__ . "/base/ApiHandler.class.php");
require_once ( __DIR__ . "/JsonResponse/TapThatLeaderboardJson.class.php");
require_once ( __DIR__ . "/JsonResponse/NearRankEntry.class.php");

class TapThatLeaderboardApiHandler extends ApiHander
{
	public function GetResponse($route)
	{
		$response = new TapThatLeaderboardJson();
		$response->rank = 5;
		$response->totalTaps = 47238474043;
		$response->nearRank = array();
		
		$response->nearRank[0] = new NearRankEntry();
		$response->nearRank[0]->rank = 4;
		$response->nearRank[0]->name = 'Kevin S.';
		$response->nearRank[0]->totalTaps = 50000000000;
		$response->nearRank[0]->delta = 200;
		
		$response->nearRank[1] = new NearRankEntry();
		$response->nearRank[1]->rank = 6;
		$response->nearRank[1]->name = 'Betty B.';
		$response->nearRank[1]->totalTaps = 40000000000;
		$response->nearRank[1]->delta = 500;
		
		return $response;
	}
}

?>