<?php

class ErrorJson
{
	public $msg;
	public $displayError;
	
	function ErrorJson($displayError, $msg = null)
	{
		$this->msg = $msg;
		$this->displayError = $displayError;
	}
}

?>