import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NfcLista } from './nfc-lista';

describe('NfcLista', () => {
  let component: NfcLista;
  let fixture: ComponentFixture<NfcLista>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NfcLista]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NfcLista);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
