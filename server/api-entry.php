<?php
//using mod_rewrite, this is the entry point of all api class
//RewriteRule ^api/(.*)$ api-entry.php?route=$1 [L,QSA]

require_once ( __DIR__ . "/include/classes/UserApiHandler.class.php");
require_once ( __DIR__ . "/include/classes/TapThatLeaderboardApiHandler.class.php");

header('Content-Type: text/javascript; charset=utf8');

$handler = null;

switch (strtoupper($_GET['route']))
{
	case 'USER/AUTH':
		$handler = new UserApiHandler();
		break;
	case 'TAPTHAT/LEADERBOARD':
		$handler = new TapThatLeaderboardApiHandler();
		break;
	default:
		throw new Exception('unknown route');
}

if ($handler != null) {
	$response = $handler->GetResponse($_GET['route']);
} else {
	$response = null;
}

print json_encode($response);
?>
