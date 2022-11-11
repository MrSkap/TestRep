import { TestBed } from '@angular/core/testing';

import { HealthServiceService } from './health-service.service';

describe('HealthServiceService', () => {
  let service: HealthServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HealthServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
