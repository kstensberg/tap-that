<?php
//using mod_rewrite, this is the entry point of all api requests
//RewriteRule ^api/(.*)$ api-entry.php?route=$1 [L,QSA]

require_once ( __DIR__ . "/include/classes/ApiMediator.class.php");
require_once ( __DIR__ . "/include/classes/UserApiHandler.class.php");
require_once ( __DIR__ . "/include/classes/TapThatApiHandler.class.php");
require_once ( __DIR__ . "/include/classes/JsonResponse/ErrorJson.class.php");

require_once ( __DIR__ . "/include/mysql.inc.php");

header('Content-Type: application/json');

$mediator = new ApiMediator();
$mysql = new EightBitMysql();

$userApi = new UserApiHandler($mysql);
$userApi->Init($mediator);

$tapThatApi = new TapThatApiHandler($mysql);
$tapThatApi->Init($mediator);

$response = $mediator->GetResponse($_GET['route']);

if ($response instanceof ErrorJson) {
	http_response_code(500);
}

print json_encode($response);

?>
