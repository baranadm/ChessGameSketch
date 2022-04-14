import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddFigureRequest, Figure, Position } from '../app/app.component';

@Injectable({
  providedIn: 'root'
})
export class ChessApiClientService {
  private readonly API_URL = 'https://localhost:7024/Chess';
  private readonly httpClient;

  constructor(http: HttpClient) {
    this.httpClient = http;
  }

  public getPlayingFigures(): Observable<Figure[]> {
    return this.httpClient.get<Figure[]>(this.API_URL);
  }

  public getNewFigures(): Observable<Figure[]> {
    return this.httpClient.get<Figure[]>(this.API_URL + '/available');
  }

  public putNewFigure(newFigure: AddFigureRequest): Observable<Figure[]> {
    let requestUrl = this.API_URL;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    let request: Observable<Figure[]> = this.httpClient.post<Figure[]>(requestUrl, newFigure, httpOptions);
    return request;
  }

  public getMovesForFigure(figure: Figure): Observable<Position[]>  {
    return this.httpClient.get<Position[]>(this.API_URL + '/getAllowedMoves/' + figure.id);
  }

  public moveFigure(figure: Figure, desiredPosition: Position): Observable<Figure[]> {
    let requestUrl = this.API_URL + '/' + figure.id;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    let request: Observable<Figure[]> = this.httpClient.put<Figure[]>(requestUrl, desiredPosition, httpOptions);
    return request;
  }
}
