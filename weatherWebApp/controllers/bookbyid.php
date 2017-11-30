<?php
$uri = "http://anbo-restserviceproviderbooks.azurewebsites.net/Service1.svc/books/";
$id = $_POST['id'];
$jsondata = file_get_contents($uri  . $id);
$book = json_decode($jsondata, true);
if (empty($book)) {
    $bookArray = null;
}
else {
    $bookArray = array($book);
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