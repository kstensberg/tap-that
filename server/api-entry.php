<?php
//using mod_rewrite, this is the entry point of all api requests
//RewriteRule ^api/(.*)$ api-entry.php?route=$1 [L,QSA]

require_once ( __DIR__ . "/include/classes/ApiMediator.class.php");
require_once ( __DIR__ . "/include/classes/UserApiHandler.class.php");
require_once ( __DIR__ . "/include/classes/TapThatApiHandler.class.php");
require_once ( __DIR__ . "/include/classes/JsonResponse/ErrorJson.class.php");

require_once ( __DIR__ . "/include/mysql.inc.php");

header('Content-Type: application/json');

/*
$handler = null;

switch (strtoupper($_GET['route']))
{
	case 'USER/AUTH':
		$handler = new UserApiHandler(new EightBitMysql());
		break;
	case 'TAPTHAT/LEADERBOARD':
		$handler = new TapThatLeaderboardApiHandler(new EightBitMysql());
		break;
	default:
		throw new Exception('unknown route');
}

if ($handler != null) {
	$response = $handler->GetResponse($_GET['route']);
} else {
	$response = null;
}

if ($response instanceof ErrorJson) {
	header('Error-State: true');
}

print json_encode($response);
*/

$mediator = new ApiMediator();
$mysql = new EightBitMysql();

$userApi = new UserApiHandler($mysql);
$userApi->Init($mediator);

$tapThatApi = new TapThatApiHandler($mysql);
$tapThatApi->Init($mediator);

$response = $mediator->GetResponse($_GET['route']);

if ($response instanceof ErrorJson) {
	header('Error-State: true');
}

print json_encode($response);

/*
$mediator->AttachRoute('load', function() { echo "Loading"; });
$mediator->AttachRoute('stop', function() { echo "Stopping"; });
$mediator->GetResponse('load'); // prints "Loading"
$mediator->GetResponse('stop'); // prints "StoppingStopped"
*/

?>
