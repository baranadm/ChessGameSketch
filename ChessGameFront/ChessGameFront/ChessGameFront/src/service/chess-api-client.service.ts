import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NewFigureDto } from '../dto/new-figure-dto';
import { PutFigureDto } from '../dto/put-figure-dto';
import { PlayingFigure } from '../model/playing-figure';
import { Position } from '../model/position';

@Injectable({
  providedIn: 'root'
})
export class ChessApiClientService {
  private readonly API_URL = 'https://localhost:7024/Chess';
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  private readonly httpClient;


  constructor(http: HttpClient) {
    this.httpClient = http;
  }

  public getPlayingFigures(): Observable<PlayingFigure[]> {
    return this.httpClient.get<PlayingFigure[]>(this.API_URL +'/figures');
  }

  public getNewFigures(): Observable<NewFigureDto[]> {
    return this.httpClient.get<NewFigureDto[]>(this.API_URL + '/figures/available');
  }

  public putNewFigure(newFigure: PutFigureDto): Observable<PlayingFigure[]> {
    let requestUrl = this.API_URL + '/figures';


    let request: Observable<PlayingFigure[]> = this.httpClient.post<PlayingFigure[]>(requestUrl, newFigure, this.httpOptions);
    return request;
  }

  public getMovesForFigure(figure: PlayingFigure): Observable<Position[]>  {
    return this.httpClient.get<Position[]>(this.API_URL + '/getAllowedMoves/' + figure.id);
  }

  public moveFigure(figure: PlayingFigure, desiredPosition: Position): Observable<PlayingFigure[]> {
    let requestUrl = this.API_URL + '/figures/' + figure.id;

    let request: Observable<PlayingFigure[]> = this.httpClient.put<PlayingFigure[]>(requestUrl, desiredPosition, this.httpOptions);
    return request;
  }
}
