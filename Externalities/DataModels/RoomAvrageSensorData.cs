﻿namespace Infastructure.DataModels;

public class RoomAvrageSensorData
{
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double CO2 { get; set; }
    public int roomId { get; set; }
}