import { TestBed } from '@angular/core/testing';

import { ChessApiClientService } from './chess-api-client.service';

describe('ChessApiClientService', () => {
  let service: ChessApiClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChessApiClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
