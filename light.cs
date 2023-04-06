#include <Wire.h>

#define BH1750_ADDRESS 0x23


bool low_light = false;
bool high_light = false;

void setup() {
  Wire.begin();
}


void loop() {

  uint16_t light_sense;
  String light;
  
  Wire.beginTransmission(BH1750_ADDRESS);
  Wire.write(0x10); // set measurement mode set to the maximum resolution, so it can be more sensitive light
  Wire.endTransmission();
  delay(200); // delay to allow sensor to take a measurement
  Wire.requestFrom(BH1750_ADDRESS, 2);
  if(Wire.available()) { // if the sensor is connected to the board
    light_sense = Wire.read() << 8 | Wire.read(); // read the raw power of the intake light from the sensor
    light_sense /= 1.2; // convert raw sense into the light_sense data
  }
  
  
  // 350 was tested durig a cloudy environment, however in dark night environment, it would dip below that
  
  
  // if the plant does not receieves enough sunlight then, display this message
  if (light_sense <= 350 && low_light == false) {
    low_light = true;
    high_light = false;
    light = "OH NO! Light Sensor hit below 350; there is no sunlight for your plant! Light: " + String(light_sense);
    Particle.publish("light", light, PRIVATE);
  }
  
  // if the plant receieves enough sunlight then, display this message
  else if (light_sense > 350 && high_light == false) {
    high_light = true;
    low_light = false;
    light = "Light Sensor is above 350; Your plant has enough sunlight! Light: " + String(light_sense);
    // light = String(light_sense);
    Particle.publish("light", light, PRIVATE);
  }
  // if the repeat information is coming through, then return back to for loop without outputing information
  // this is used to reduce notification clutter
  else {
      return;
  }

  
  // delay by 30 mins (Thi can change based on whether the)
  delay(10000);
//   delay(1800000);
}
