# FreshRoomsExamProject

A new C# & Angular project made for a school exam project.
The purpose of this project is to make a app that can work as Web controller for IOT devices.

This project was made by Jens Chr P., Andreas A. H., Andreas S. H. and Emil W. A.

## Getting Started
Get dependencies by running the (*npm install*) command in a bash console within the frontend, or follow Webstorm recommendation.
Run the Api connection on localhost, or if different port change address in Envirement file in the frontend folder

To get started do the following
* Run the SQL script in a avalible DB
* Envirement variables:
  * crt | add a certificate authority file path. for the MQTT broker
  * JWT_KEY | Base64Encode 64 charector string
  * mqtt_host | host address for the mqtt
  * pgconn | database conection Strig
* For security purpose´s the envirement vaiables will not be published.
* To start the api enter the api folder and run the (*dotnet run*) command, in a fresh terminal.
* to start the angular frontend enter the frontend folder and run the (*ng serve*) command in a seperate terminal from the api.

Video Presentaion´s - https://drive.google.com/drive/folders/1ZoXUsgAByRx0BKiN57BQ4zlUzZhw6ahO?usp=sharing
