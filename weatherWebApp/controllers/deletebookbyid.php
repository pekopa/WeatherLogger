<?php
$uri = "http://anbo-restserviceproviderbooks.azurewebsites.net/Service1.svc/books/";
$id = $_POST['id'];
$full_uri = $uri . $id;

$ch = curl_init($full_uri);
// curl is good for more complex operations than just plain GET
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "DELETE");
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
// TRUE to return the transfer as a string of the return value of curl_exec() instead of outputting it directly.

$jsondata = curl_exec($ch);
$theDeletedBook = json_decode($jsondata, true);

if ($theDeletedBook == null) {
    $bookArray = false;
} else {
    $bookArray = array($theDeletedBook);
}
//print_r($bookArray);

require_once '../vendor/autoload.php';
Twig_Autoloader::register();

$loader = new Twig_Loader_Filesystem('../views');
$twig = new Twig_Environment($loader, array(
    // 'cache' => '/path/to/compilation_cache',
    'auto_reload' => true
));
$template = $twig->loadTemplate('books.html.twig');

$parametersToTwig = array("books" => $bookArray);
echo $template->render($parametersToTwig);