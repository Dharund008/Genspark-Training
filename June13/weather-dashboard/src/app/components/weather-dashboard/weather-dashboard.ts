import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WeatherService } from '../../services/weather';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-weather-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './weather-dashboard.html',
  styleUrls: ['./weather-dashboard.css']
})
export class WeatherDashboardComponent implements OnInit {
  weatherData: any = null;
  error: string | null = null;

  constructor(private weatherService: WeatherService) {}

  ngOnInit() {
    this.weatherService.selectedCity$.subscribe(city => {
      if (city) {
        this.weatherService.getWeatherByCity(city).subscribe({
          next: (data) => {
            this.weatherData = data;
            this.error = null;
          },
          error: (err) => {
            this.error = err.message;
            this.weatherData = null;
          }
        });
      }
    });
  }
}
