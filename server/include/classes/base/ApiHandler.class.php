<?php

abstract class ApiHander
{
	protected $mysql;
	
	function ApiHander(mysqli $mysql)
	{
		$this->mysql = $mysql;
	}
	
	public abstract function GetResponse($route);
}

?>