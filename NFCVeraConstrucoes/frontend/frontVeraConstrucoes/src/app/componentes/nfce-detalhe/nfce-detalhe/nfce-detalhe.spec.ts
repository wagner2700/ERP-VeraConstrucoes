import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NfceDetalhe } from './nfce-detalhe';

describe('NfceDetalhe', () => {
  let component: NfceDetalhe;
  let fixture: ComponentFixture<NfceDetalhe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NfceDetalhe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NfceDetalhe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
