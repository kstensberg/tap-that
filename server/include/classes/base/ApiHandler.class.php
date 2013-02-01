<?php

require_once ( __DIR__ . "/../ApiMediator.class.php");

abstract class ApiHander
{
	protected $mysql;
	
	function ApiHander(mysqli $mysql)
	{
		$this->mysql = $mysql;
	}
	
	public abstract function Init(ApiMediator $apiMediator);
}

?>