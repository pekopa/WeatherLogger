<?php
$uri = "http://restfullservicefordatalogger.azurewebsites.net/Weather.svc/WeatherMeasurements/";
$sensorWeatherJson = file_get_contents($uri);
$convertToAssociativeArray = true;
$sensorWeather = json_decode($sensorWeatherJson, $convertToAssociativeArray);

require_once './vendor/autoload.php';
Twig_Autoloader::register();

$loader = new Twig_Loader_Filesystem('./views');
$twig = new Twig_Environment($loader, array(
    'auto_reload' => true
));
$template = $twig->loadTemplate('dashboard.html.twig');

function getMinValueByKey($weatherArray, $key) {
	$minValue;
	foreach ($weatherArray as $i => $weather) {
		if($i === 0 ) {
			$minValue = $weather[$key];
		} else {
			if($minValue > $weather[$key]) {
				$minValue = $weather[$key];
			}
		}
	}
	return $minValue;
}

function getMaxValueByKey($weatherArray, $key) {
	$minValue;
	foreach ($weatherArray as $i => $weather) {
		if($i === 0 ) {
			$maxValue = $weather[$key];
		} else {
			if($maxValue < $weather[$key]) {
				$maxValue = $weather[$key];
			}
		}
	}
	return $maxValue;
}

function getAvgValueByKey($weatherArray, $key) {
	$data = array();

	foreach ($weatherArray as $i => $value) {
		array_push($data, $weatherArray[$i][$key]);
	}

	$average = array_sum($data)/count($data);
	$average = round($average, 2);
	return $average;
}

function getMinValues($weatherArray) {
	$minValues = array();
	foreach ($weatherArray[0] as $key => $value) {
		$minValue = getMinValueByKey($weatherArray, $key);
		$minValues[$key] = $minValue;
	}

	return $minValues;
}

function getMaxValues($weatherArray) {
	$maxValues = array();
	foreach ($weatherArray[0] as $key => $value) {
		$maxValue = getMaxValueByKey($weatherArray, $key);
		$maxValues[$key] = $maxValue;
	}

	return $maxValues;
}

function getAvgValues($weatherArray) {
	$avgValues = array();
	foreach ($weatherArray[0] as $key => $value) {
		$avgValue = getAvgValueByKey($weatherArray, $key);
		$avgValues[$key] = $avgValue;
	}

	return $avgValues;
}

$minSensorValues = getMinValues($sensorWeather);
$maxSensorValues = getMaxValues($sensorWeather);
$avgSensorValues = getAvgValues($sensorWeather);;

$parametersToTwig = array(
	"sensorWeather" => $sensorWeather,
	"latestWeater" => $sensorWeather[count($sensorWeather)-1],
	"minSensorValues" => $minSensorValues,
	"maxSensorValues" => $maxSensorValues,
	"avgSensorValues" => $avgSensorValues
);
echo $template->render($parametersToTwig);