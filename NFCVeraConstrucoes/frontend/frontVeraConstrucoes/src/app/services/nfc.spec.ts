import { TestBed } from '@angular/core/testing';

import { NFC } from './nfc';

describe('NFC', () => {
  let service: NFC;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NFC);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
