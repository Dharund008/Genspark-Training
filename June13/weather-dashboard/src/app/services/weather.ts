import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root' // Makes it available app-wide automatically
})
export class WeatherService {
  private apikey = 'dcd3b9e633d753b4407df80e818b26e0';
  private apiurl = 'https://api.openweathermap.org/data/2.5/weather';
  private http = inject(HttpClient);

  private selectedCitySubject = new BehaviorSubject<string>('London');
  selectedCity$ = this.selectedCitySubject.asObservable();

  updateCity(city: string) {
    this.selectedCitySubject.next(city);
  }

  getWeatherByCity(city: string) {
    const url = `${this.apiurl}?q=${city}&appid=${this.apikey}&units=metric`;
    return this.http.get(url).pipe(catchError(this.handleerror));
  }

  handleerror(error: HttpErrorResponse) {
    let errorMessage = 'Something went wrong!';
    if (error.status === 404) {
      errorMessage = 'City not found.';
    } else if (error.error instanceof ErrorEvent) {
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      errorMessage = `Server error: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
