import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import  { WeatherDashboardComponent } from './components/weather-dashboard/weather-dashboard';
import { CitySearchComponent } from './components/city-search/city-search';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, WeatherDashboardComponent, CitySearchComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weather-dashboard';
}
