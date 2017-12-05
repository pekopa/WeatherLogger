<?php

$id = $_POST["id"];
$author = $_POST["author"];
$title = $_POST["title"];
$publisher = $_POST["publisher"];
$price = $_POST["price"];

$data = array("Author" => $author, "Title" => $title, "Publisher" => $publisher, "Price" => $price);
$json_string = json_encode($data);

$uri = "http://anbo-restserviceproviderbooks.azurewebsites.net/Service1.svc/books/";
$full_uri = $uri . $id;
$ch = curl_init($full_uri);
// curl is good for more complex operations than just plain GET
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "PUT");
curl_setopt($ch, CURLOPT_POSTFIELDS, $json_string);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
// TRUE to return the transfer as a string of the return value of curl_exec() instead of outputting it directly.

curl_setopt($ch, CURLOPT_HTTPHEADER, array(
        'Content-Type: application/json',
        'Content-Length: ' . strlen($json_string))
);

$jsondata = curl_exec($ch);
$theNewBook = json_decode($jsondata, true);

$bookArray = array($theNewBook);
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