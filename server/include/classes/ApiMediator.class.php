<?php

class ApiMediator
{
    protected $routes = array();
	
    public function AttachRoute($route, $obj, $methodName, $requiresAuth) 
	{
		$this->routes[] = array(
			'route' => $route,
			'obj' => $obj,
			'methodName' => $methodName,
			'requiresAuth' => $requiresAuth
		);
    }
    public function GetResponse($route) 
	{
		foreach ($this->routes as $value){
			if (strpos(strtoupper($route), strtoupper($value['route'])) === 0) {
				
				if ($value['requiresAuth'] !== false) {
					if (array_key_exists('authToken', $_POST)) {
						session_id($_POST['authToken']);
					}
					
					session_start();
					
					if (count($_SESSION) <= 0) {
						return new ErrorJson('access denied', 'no session exists');
					} else if (!array_key_exists('userId', $_SESSION)) {
						return new ErrorJson('access denied', 'user id not found in session');
					}
				}
				
				return call_user_func(array($value['obj'], $value['methodName']));
			}
		}
		
		return new ErrorJson('api route not found');
    }
}

?>