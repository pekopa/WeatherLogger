var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June',
	'July', 'August', 'September', 'October', 'November', 'December']

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