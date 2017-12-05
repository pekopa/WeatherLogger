var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June',
	'July', 'August', 'September', 'October', 'November', 'December']


function drawGraph(graphId, graphData) {

	var chartOptions = {
    responsive: true,
    scaleLineColor: "rgba(0,0,0,.2)",
    scaleGridLineColor: "rgba(0,0,0,.05)",
    scaleFontColor: "#c5c7cc"
  }

	var chartElement = document.getElementById(graphId).getContext("2d");
  window.myLine = new Chart(chartElement).Line(graphData, chartOptions);

}

function getDateLabels(data, format) {

	seperators = format.match(/(\W)/g);
	formatStrings = format.match(/(\w)\1+/g);

	var labels = data.map(function(measurement) {
		var dateObj = new Date(measurement.TimeStamp);
		var labelDateData = formatStrings.map(function(str) {
			switch (str) {
				case 'YY':
					return dateObj.getFullYear()
					break;
				case 'mm':
					var month = dateObj.getMonth();
					month = ('0'+(month+1)).slice(-2);
					return month;
					break;
				case 'TT':
					var month = dateObj.getMonth();
					var monthAsWord = MONTHS[month];
					console.log(MONTHS)
					return monthAsWord;
					break;
				case 'DD':
					var day = dateObj.getDate();
					day = ('0'+day).slice(-2);
					return day;
					break;
				case 'HH':
					var hours = dateObj.getHours();
					hours = ('0'+hours).slice(-2);
					return hours;
					break;
				case 'MM':
					var minutes = dateObj.getMinutes();
					minutes = ('0'+minutes).slice(-2);
					return minutes;
					break;
				case 'SS':
					var seconds = dateObj.getSeconds();
					seconds = ('0'+seconds).slice(-2);
					return seconds;
					break;
				default:
					break;
			}
		}); // labelDateData

		var label = '';
		labelDateData.forEach(function(datePart,i) {
			var separator = seperators[i] ? seperators[i] : ' ';
			label = label + datePart + separator;
		});

		return label;
	}); // labels

	return  labels;
}

function getWeatherPropsData(data) {
	var temperature = [],
			humidity = [],
			pressure = [],
			windSpeed = [];

	data.forEach(function(measurement) {
      temperature.push(measurement.Temperature);
      humidity.push(measurement.Humidity);
      pressure.push(measurement.Pressure);
      windSpeed.push(measurement.WindSpeed);
  });

  return {
  	temperature: temperature,
  	humidity: humidity,
  	pressure: pressure,
  	windSpeed: windSpeed
  }
}

function fetchWeatherData(timeStampStart, timeStampEnd) {

	if (isNaN(timeStampStart) || isNaN(timeStampEnd)) {
		return null;
	}

	var apiUrl = 'http://restfullservicefordatalogger.azurewebsites.net/Weather.svc/WeatherMeasurements/';
	apiUrl += timeStampStart + "/" + timeStampEnd; 

	console.log('tiimestamps')
	console.log(apiUrl)

	var proxy = 'https://cors-anywhere.herokuapp.com/';

	var graphConfig = {
    fillColor: "rgba(220,220,220,0.2)",
    pointColor:"rgba(220,220,220,1)",
    pointHighlightFill:"#fff",
    pointHighlightStroke:"rgba(220,220,220,1)",
    pointStrokeColor:"#fff",
    strokeColor:"rgba(220,220,220,1)"
  }

	$.get(proxy+apiUrl, function(data) {

		var labels = getDateLabels(data, 'HH:MM');

		var latestWeater = data[data.length-1];
		var minSensorValues = getMinValues(data);
		var maxSensorValues = getMaxValues(data);
		var avgSensorValues = getAvgValues(data);
		var dataByProps = getWeatherPropsData(data);

		console.log(dataByProps)
		// 	TEMPERATURE
		var datasetSensorTemperature = {
        data: dataByProps.temperature,
        graphConfig
    }
		var SensorTemperatureGraphData = {
       datasets: [datasetSensorTemperature],
       labels: labels
    }

    // HUMIDITY
    var datasetSensorHumidity = {
        data: dataByProps.humidity,
        graphConfig
    }
		var SensorHumidityGraphData = {
       datasets: [datasetSensorHumidity],
       labels: labels
    }

    // PRESSURE
    var datasetSensorPressure = {
        data: dataByProps.pressure,
        graphConfig
    }
		var SensorPressureGraphData = {
       datasets: [datasetSensorPressure],
       labels: labels
    }

    // HUMIDITY
    var datasetSensorHumidity = {
        data: dataByProps.humidity,
        graphConfig
    }
		var SensorHumidityGraphData = {
       datasets: [datasetSensorHumidity],
       labels: labels
    }

    // WIND SPEED
    var datasetSensorWindSpeed = {
        data: dataByProps.windSpeed,
        graphConfig
    }
		var SensorWindSpeedGraphData = {
       datasets: [datasetSensorWindSpeed],
       labels: labels
    }

    var url = "../../controllers/_3rdPartyWeather.php?date-start="+timeStampStart+"&date-end="+timeStampEnd;

    $.get(url, function(data) {
    	console.log('OPEN WEATHER')
    	var apiWeather = JSON.parse(data);
    	console.log(apiWeather)
    });

    drawGraph('temperature-chart', SensorTemperatureGraphData);
    drawGraph('humidity-chart', SensorHumidityGraphData);
    drawGraph('pressure-chart', SensorPressureGraphData);
		drawGraph('windSpeed-chart', SensorWindSpeedGraphData);
	});
}


function getMinValueByKey(weatherArray, key) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	var minValue;

	weatherArray.forEach(function(weather, i) {
		if(i === 0 ) {
			minValue = weather[key];
		} else {
			if(minValue > weather[key]) {
				minValue = weather[key];
			}
		}
	});

	return minValue;
}

function getMaxValueByKey(weatherArray, key) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	var maxValue;

	weatherArray.forEach(function(weather, i) {
		if(i === 0 ) {
			maxValue = weather[key];
		} else {
			if(maxValue < weather[key]) {
				maxValue = weather[key];
			}
		}
	});

	return maxValue;
}

function getAvgValueByKey(weatherArray, key) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	data = [];
	sum = 0;

	weatherArray.forEach( function(value, i) {
		data.push(weatherArray[i][key]);
		sum += weatherArray[i][key];
	});


	average = sum/data.length;
	average = Math.round(average, 2);
	return average;
}

function getMinValues(weatherArray) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	minValues = [];

	Object.keys(weatherArray[0]).forEach(function(key) {
		minValue = getMinValueByKey(weatherArray, key);
		minValues[key] = minValue;
	});

	return minValues;
}

function getMaxValues(weatherArray) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	maxValues = [];

	Object.keys(weatherArray[0]).forEach(function(key) {
		maxValue = getMaxValueByKey(weatherArray, key);
		maxValues[key] = maxValue;
	});

	return maxValues;
}

function getAvgValues(weatherArray) {
	if (weatherArray == null || weatherArray.length === 0) {
		return [];
	}
	avgValues = [];

	Object.keys(weatherArray[0]).forEach(function(key) {
		avgValue = getAvgValueByKey(weatherArray, key);
		avgValues[key] = avgValue;
	});

	return avgValues;
}