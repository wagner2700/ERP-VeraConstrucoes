import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Emissao } from './emissao';

describe('Emissao', () => {
  let component: Emissao;
  let fixture: ComponentFixture<Emissao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Emissao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Emissao);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
