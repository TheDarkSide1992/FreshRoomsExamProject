export interface DailyWeatherModel {
  latitude: number;
  longitude: number;
  generationtime_ms: number;
  utc_offset_seconds: number;
  timezone: string;
  timezone_abbreviation: string;
  elevation: number;
  daily_units: DailyUnits;
  daily: Daily;
}

export interface Daily {
  time?: string[];
  weather_code?: number[];
  temperature_2m_max?: number[];
  apparent_temperature_max?: number[];
  precipitation_probability_max?: any[];
}

export interface DailyUnits {
  time: string;
  weather_code: string;
  temperature_2m_max: string;
  apparent_temperature_max: string;
  precipitation_probability_max: string;
}
