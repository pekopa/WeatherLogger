# august 2016 Ebbe Vang
# for python3. smbus module required
 
import smbus
bus = smbus.SMBus(1)
import time
from socket import *
from datetime import datetime
import random
import struct
 
#choose to read from onboard light sensor / 0
bus.write_byte(0x48, 0x00)
 
# set last read data....
last_temperature = -1
last_pressure = -1
last_humidity = -1
last_windspeed = -1

# do forever...
while (True):
#choose to read from temperature sensor / 0
	bus.write_byte(0x48, 0x00)
	#read analog
	reading = bus.read_byte(0x48)
	#if current reading is different from last reading
	if (abs(last_temperature - reading) > 2):
		# print data
		print('Temperature: {}'.format(reading))
		# store data as last data
		last_temperature = reading
		
	reading = random.uniform(900, 1100)
	if (abs(last_pressure - reading) > 2):
		print('Pressure: {}' .format(reading))
		last_pressure = reading
		
	reading = random.uniform(4, 10)
	if (abs(last_windspeed - reading) > 2):
		print('WindSpeed: {}' .format(reading))
		last_windspeed = reading

#choose to read from humidity sensor / 2
	bus.write_byte(0x48, 0x02)
	#read analog
	reading = bus.read_byte(0x48)
	#if current reading is different from last reading
	if (abs(last_humidity - reading) > 2):
		#print data
		print('Humidity: {}'.format(reading))
		#store data as last reading
		last_humidity = reading

	time.sleep(1)

data = reading
ba = bytearray(struct.pack("f", data))
	
	BROADCAST_TO_PORT = 7000 
s = socket(AF_INET, SOCK_DGRAM)
#s.bind(('', 14593))     # (ip, port)
# no explicit bind: will bind to default IP + random port
s.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)
while True:
	#data = "Current time " + str(datetime.now())
	s.sendto(bytes(ba), ('<broadcast>', BROADCAST_TO_PORT))
	print(data)
	#Read the data every 10 minutes and send it right away
	time.sleep(10000)
