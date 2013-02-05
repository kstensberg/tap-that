<?php

class ApiMediator
{
    protected $routes = array();
	
    public function AttachRoute($route, $obj, $methodName) 
	{
		$this->routes[] = array(
			'route' => $route,
			'obj' => $obj,
			'methodName' => $methodName
		);
    }
    public function GetResponse($route) 
	{
		foreach ($this->routes as $value){
			if (strpos(strtoupper($route), strtoupper($value['route'])) === 0) {
				return call_user_func(array($value['obj'], $value['methodName']));
			}
		}
		
		return new ErrorJson('api route not found');
    }
}

?>