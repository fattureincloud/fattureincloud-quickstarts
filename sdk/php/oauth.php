<?php
require_once(__DIR__ . '/vendor/autoload.php');

use FattureInCloud\OAuth2\OAuth2AuthorizationCodeManager;
use FattureInCloud\OAuth2\Scope;

session_set_cookie_params(86400);
session_start();

$oauth = new OAuth2AuthorizationCodeManager("CLIENT_ID", "CLIENT_SECRET", "http://localhost:8000/oauth.php");

if(isset($_SESSION['token'])) die('You already have an access token');

if(!isset($_GET['code'])) {
  $url = $oauth->getAuthorizationUrl([Scope::ENTITY_SUPPLIERS_READ], "EXAMPLE_STATE");
  header('location: '.$url);
} else {
    $code = $_GET['code'];
    $obj = $oauth->fetchToken($code);
    if(!isset($obj->error)) {
      $_SESSION['token'] = $obj->getAccessToken(); //saving the oAuth access token in a session variable
      $_SESSION['refresh'] = $obj->getRefreshToken(); 
    }

    echo 'Token saved correctly in the session variable';
}

?>