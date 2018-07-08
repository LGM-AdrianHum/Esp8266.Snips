#include <SimpleDHT.h>
#include <ESP8266WiFi.h>
#include "Adafruit_MQTT.h"
#include "Adafruit_MQTT_Client.h"

#define WLAN_SSID       "TelstraAF73E6"  // can't be longer than 32 characters!
#define WLAN_PASS       "mxcv6kfa9dbc"
#define AIO_SERVER      "10.0.0.1"
#define AIO_SERVERPORT  1883
#define AIO_USERNAME    "Adrian"
#define AIO_KEY         "Key"

// for DHT11,
//      VCC: 5V or 3V
//      GND: GND
//      DATA: 2
int pinDHT = 14;
SimpleDHT22 dht;
WiFiClient client;
Adafruit_MQTT_Client mqtt(&client, AIO_SERVER, AIO_SERVERPORT, AIO_USERNAME, AIO_KEY);
Adafruit_MQTT_Publish temphumid = Adafruit_MQTT_Publish(&mqtt, AIO_USERNAME "/feeds/temphumid");

void setup() {
Serial.begin(115200);
  delay(10);
 
  Serial.println(F("MQTT demo"));
 
  // Connect to WiFi access point.
  Serial.println(); Serial.println();
  Serial.print("Connecting to ");
  Serial.println(WLAN_SSID);
 
  WiFi.begin(WLAN_SSID, WLAN_PASS);
  while (WiFi.status() != WL_CONNECTED) { 
    delay(500);
    Serial.print(".");
  }
  Serial.println();
 
  Serial.println("WiFi connected");
  Serial.println("IP address: "); Serial.println(WiFi.localIP());
}

void loop() {

 MQTT_connect();


  
  byte temperature = 0;
  byte humidity = 0;
  int err = SimpleDHTErrSuccess;
  if ((err = dht.read(pinDHT, &temperature, &humidity, NULL)) != SimpleDHTErrSuccess) {
    Serial.print("Read DHT failed, err="); Serial.println(err); delay(1000);
    return;
  }

  Serial.print((int)temperature); Serial.print(" *C, ");
  Serial.print((int)humidity); Serial.println(" H");
  char data[100];
  snprintf(data,sizeof(data), "{\"temp\":%04d,\"humid\":%04d}", (int)temperature, (int)humidity);
  Serial.print(data);
 temphumid.publish(data);
  
  // DHT11 sampling rate is 1HZ.
  delay(1500);
}
void MQTT_connect() {
  int8_t ret;

  // Stop if already connected.
  if (mqtt.connected()) {
    return;
  }

  Serial.print("Connecting to MQTT... ");

  uint8_t retries = 3;
  while ((ret = mqtt.connect()) != 0) { // connect will return 0 for connected
       Serial.println(mqtt.connectErrorString(ret));
       Serial.println("Retrying MQTT connection in 5 seconds...");
       mqtt.disconnect();
       delay(5000);  // wait 5 seconds
       retries--;
       if (retries == 0) {
         // basically die and wait for WDT to reset me
         while (1);
       }
  }
  Serial.println("MQTT Connected!");
}

