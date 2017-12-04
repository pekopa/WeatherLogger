<?php
require('../config.php');

$apiUrl = "http://api.openweathermap.org/data/2.5/forecast?id=2614478&mode=json&units=metric&APPID=" . $APPID;

$dateStart = $_GET['date-start'];
$dateEnd = $_GET['date-end'];

$timestampStart = strtotime($dateStart) - (43200*2);
$timestampEnd = strtotime($dateEnd) + (43200*2);

// $uri = "http://restfullservicefordatalogger.azurewebsites.net/Weather.svc/WeatherMeasurements/";
// $uriParams = (string)$timestampStart . '/' . (string)$timestampEnd;

$apiUrl .= "&date-start=" . $timestampStart . "&date-end=" . $timestampEnd;

$apiWeatherJson = file_get_contents($apiUrl);
$convertToAssociativeArray = true;
$apiWeather = json_decode($apiWeatherJson, $convertToAssociativeArray);

// print_r($apiWeather);

echo $apiWeatherJson;