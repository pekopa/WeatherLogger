var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June',
	'July', 'August', 'September', 'October', 'November', 'December']


function drawGraph(graphId, graphData) {

	var chartOptions = {
    responsive: true,
    scaleLineColor: "rgba(0,0,0,.2)",
    scaleGridLineColor: "rgba(0,0,0,.05)",
    scaleFontColor: "#c5c7cc"
  }

  resetCanvas(graphId);
	var canvasCtx = document.getElementById(graphId).getContext("2d");
	canvasCtx.clearRect(0, 0, 10000, 100000);
  window.myLine = new Chart(canvasCtx).Line(graphData, chartOptions);
}

var resetCanvas = function(canvasId){
  $('#'+canvasId).remove(); // this is my <canvas> element
  $('.canvas-wrapper.'+canvasId).append('<canvas class="main-chart" id="'+canvasId+'"><canvas>');
  canvas = document.querySelector('#'+canvasId);
  ctx = canvas.getContext('2d');
  ctx.canvas.width = $('.canvas-wrapper.'+canvasId).width(); // resize to parent width
  ctx.canvas.height = $('.canvas-wrapper.'+canvasId).height(); // resize to parent height
  var x = canvas.width/2;
  var y = canvas.height/2;
};

function drawPieChart(id, minValue, maxValue, averageValue) {
	var percent = (100/maxValue)*averageValue;
	var element = '<div class="easypiechart" id="'+id+'" data-percent="'+ 100 +'"></div>';

	$('#'+id).easyPieChart({
      barColor: 'red'
  });
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

function getOpenWeatherProps(data) {
	var temperature = [],
			humidity = [],
			pressure = [],
			windSpeed = [];

	data.forEach(function(measurement) {
      temperature.push(measurement.main.temp);
      humidity.push(measurement.main.humidity);
      pressure.push(measurement.main.pressure);
      windSpeed.push(measurement.wind.speed);
  });

  return {
  	temperature: temperature,
  	humidity: humidity,
  	pressure: pressure,
  	windSpeed: windSpeed
  }
}


var sensorGraphConfig = {
  fillColor: "rgba(220,220,220,0)",
  pointColor:"rgba(0,0,255,1)",
  pointHighlightFill:"#fff",
  pointHighlightStroke:"rgba(0,0,255,1)",
  pointStrokeColor:"#fff",
  strokeColor:"rgba(0,0,255,1)"
}

var openWeatherGraphConfig = {
  fillColor: "rgba(255,0,0,0)",
  pointColor:"rgba(255,0,0,1)",
  pointHighlightFill:"#fff",
  pointHighlightStroke:"rgba(220,220,220,1)",
  pointStrokeColor:"#fff",
  strokeColor:"rgba(255,0,0,1)"
}

function fetchWeatherData(timeStampStart, timeStampEnd) {

	if (isNaN(timeStampStart) || isNaN(timeStampEnd)) {
		return null;
	}

	var apiUrl = 'http://restfullservicefordatalogger.azurewebsites.net/Weather.svc/WeatherMeasurements/';
	apiUrl += timeStampStart + "/" + timeStampEnd; 

	var proxy = 'https://cors-anywhere.herokuapp.com/';

	$.get(proxy+apiUrl, function(data) {

		var labels = getDateLabels(data, 'HH:MM');

		var latestWeater = data[data.length-1];
		var minSensorValues = getMinValues(data);
		var maxSensorValues = getMaxValues(data);
		var avgSensorValues = getAvgValues(data);
		var dataByProps = getWeatherPropsData(data);

		console.log('MIN')
		console.log(minSensorValues)

		// 	TEMPERATURE
		var datasetSensorTemperature = sensorGraphConfig;
		datasetSensorTemperature.data = dataByProps.temperature;

		var SensorTemperatureGraphData = {
       datasets: [datasetSensorTemperature],
       labels: labels
    }

    // HUMIDITY
    var datasetSensorHumidity = sensorGraphConfig;
    datasetSensorHumidity.data = dataByProps.humidity;    
        
		var SensorHumidityGraphData = {
       datasets: [datasetSensorHumidity],
       labels: labels
    }

    // PRESSURE
    var datasetSensorPressure = sensorGraphConfig;
    datasetSensorPressure.data = dataByProps.pressure;

		var SensorPressureGraphData = {
       datasets: [datasetSensorPressure],
       labels: labels
    }

    // WIND SPEED
    var datasetSensorWindSpeed = sensorGraphConfig;
    datasetSensorWindSpeed.data = dataByProps.windSpeed;

		var SensorWindSpeedGraphData = {
       datasets: [datasetSensorWindSpeed],
       labels: labels
    }

    var url = "../../controllers/_3rdPartyWeather.php?date-start="+timeStampStart+"&date-end="+timeStampEnd;

    $.get(url, function(data) {
    	
    	var apiWeather = JSON.parse(data);
    	var openWeather = getOpenWeatherProps(apiWeather.list);

    	var openWeatherDatasets = {};
    	Object.keys(openWeather).forEach(function(key) {
    		openWeatherDatasets[key] = Object.assign({}, openWeatherGraphConfig);
    		openWeatherDatasets[key].data = openWeather[key];
    	});

    	SensorTemperatureGraphData.datasets.push(openWeatherDatasets.temperature);
    	SensorHumidityGraphData.datasets.push(openWeatherDatasets.humidity);
    	SensorPressureGraphData.datasets.push(openWeatherDatasets.pressure);
    	SensorWindSpeedGraphData.datasets.push(openWeatherDatasets.windSpeed);

    	drawGraph('temperature-chart', SensorTemperatureGraphData);
	    drawGraph('humidity-chart', SensorHumidityGraphData);
	    drawGraph('pressure-chart', SensorPressureGraphData);
			drawGraph('windSpeed-chart', SensorWindSpeedGraphData);

			drawPieChart('easypiechart-blue');
    });
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