export interface TodayWeatherModel {
    latitude: number;
    longitude: number;
    generationtime_ms: number;
    utc_offset_seconds: number;
    timezone: string;
    timezone_abbreviation: string;
    elevation: number;
    hourly_units: HourlyUnits;
    hourly: Hourly;
}

export interface Hourly {
    time?: string[];
    temperature_2m?: number[];
    apparent_temperature?: number[];
    precipitation?: number[];
    weather_code?: number[];
}

export interface HourlyUnits {
    time: string;
    temperature_2m: string;
    apparent_temperature: string;
    precipitation: string;
    weather_code: string;
}
