<?php

$author = $_POST["author"];
$title = $_POST["title"];
$publisher = $_POST["publisher"];
$price = $_POST["price"];

// Adapted from http://www.lornajane.net/posts/2011/posting-json-data-with-php-curl
$data = array("Author" => $author, "Title" => $title, "Publisher" => $publisher, "Price" => $price);
$data_string = json_encode($data);

$uri = "http://anbo-restserviceproviderbooks.azurewebsites.net/Service1.svc/books";
$ch = curl_init($uri);
// curl is good for more complex operations than just plain GET
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");
curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
// TRUE to return the transfer as a string of the return value of curl_exec() instead of outputting it directly.

curl_setopt($ch, CURLOPT_HTTPHEADER, array(
        'Content-Type: application/json',
        'Content-Length: ' . strlen($data_string))
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