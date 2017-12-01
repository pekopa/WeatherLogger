<?php
$uri = "http://anbo-restserviceproviderbooks.azurewebsites.net/Service1.svc/books/";
$titlefragment = $_POST['titlefragment'];
$jsondata = file_get_contents($uri . "title/" . $titlefragment);
$books = json_decode($jsondata, true);

require_once '../vendor/autoload.php';
Twig_Autoloader::register();

$loader = new Twig_Loader_Filesystem('../views');
$twig = new Twig_Environment($loader, array(
    // 'cache' => '/path/to/compilation_cache',
    'auto_reload' => true
));
$template = $twig->loadTemplate('books.html.twig');

$parametersToTwig = array("books" => $books);
echo $template->render($parametersToTwig);