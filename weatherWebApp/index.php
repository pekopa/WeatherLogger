<?php
require_once './vendor/autoload.php';
Twig_Autoloader::register();

$loader = new Twig_Loader_Filesystem('./views');
$twig = new Twig_Environment($loader, array(
    'auto_reload' => true,
    'optimizations' => 0
));
$template = $twig->loadTemplate('dashboard.html.twig');

$prevDayTimestamp = strtotime('-1 day');

$dateStart = ($_GET && $_GET['date-start']) ?: date('Y/m/d', $prevDayTimestamp);
$dateEnd = ($_GET && $_GET['date-end']) ?: date('Y/m/d');

$timestampStart = strtotime($dateStart) - (43200*5);
$timestampEnd = strtotime($dateEnd) + (43200*2);

print_r($timestampStart);

$uri = "http://restfullservicefordatalogger.azurewebsites.net/Weather.svc/WeatherMeasurements/";
$uriParams = (string)$timestampStart . '/' . (string)$timestampEnd;

$sensorWeatherJson = file_get_contents($uri . $uriParams);
$convertToAssociativeArray = true;
$sensorWeather = json_decode($sensorWeatherJson, $convertToAssociativeArray);

function getMinValueByKey($weatherArray, $key) {
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
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
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
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
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
	$data = array();

	foreach ($weatherArray as $i => $value) {
		array_push($data, $weatherArray[$i][$key]);
	}
	
	$average = array_sum($data)/count($data);
	$average = round($average, 2);
	return $average;
}

function getMinValues($weatherArray) {
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
	$minValues = array();
	foreach ($weatherArray[0] as $key => $value) {
		$minValue = getMinValueByKey($weatherArray, $key);
		$minValues[$key] = $minValue;
	}

	return $minValues;
}

function getMaxValues($weatherArray) {
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
	$maxValues = array();
	foreach ($weatherArray[0] as $key => $value) {
		$maxValue = getMaxValueByKey($weatherArray, $key);
		$maxValues[$key] = $maxValue;
	}

	return $maxValues;
}

function getAvgValues($weatherArray) {
	if ($weatherArray == null || count($weatherArray) == 0) {
		return [];
	}
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

if (count($sensorWeather) === 0) {
	$sensorWeather = array(1);
}

$parametersToTwig = array(
	"sensorWeather" => $sensorWeather,
	"latestWeater" => $sensorWeather[count($sensorWeather)-1],
	"minSensorValues" => $minSensorValues,
	"maxSensorValues" => $maxSensorValues,
	"avgSensorValues" => $avgSensorValues,
	"dateStart" => $dateStart,
	"dateEnd" => $dateEnd,
	"timestampStart" => $timestampStart,
	"timestampEnd" => $timestampEnd,
);

echo $template->render($parametersToTwig);