import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DashboardComponent } from './dashboard';
import { DashboardService } from '../../services/dashboard';
import { of, throwError } from 'rxjs';
import { IDashboardSummary } from '../../models/interfaces/dashboard/IDashboardSummary.interface';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let dashboardService: jasmine.SpyObj<DashboardService>;

  const mockSummary: IDashboardSummary = {
    totalNotasFiscais: 100,
    notasEmitidasHoje: 10,
    notasCanceladasHoje: 2,
    valorTotalNotasHoje: 5000,
    valorMedioNotas: 500,
    ultimasNotas: [
      {
        id: '1',
        numero: '001',
        serie: '1',
        dataEmissao: new Date(),
        valorTotal: 1000,
        status: 'emitida',
        clienteNome: 'Cliente Teste'
      }
    ]
  };

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('DashboardService', ['getDashboardSummary']);

    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        { provide: DashboardService, useValue: spy }
      ]
    }).compileComponents();

    dashboardService = TestBed.inject(DashboardService) as jasmine.SpyObj<DashboardService>;
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load dashboard data on init', () => {
    dashboardService.getDashboardSummary.and.returnValue(of(mockSummary));

    component.ngOnInit();

    expect(dashboardService.getDashboardSummary).toHaveBeenCalled();
    expect(component.summary).toEqual(mockSummary);
    expect(component.loading).toBeFalse();
  });

  it('should handle error on load failure', () => {
    dashboardService.getDashboardSummary.and.returnValue(throwError(() => new Error('Erro')));

    component.ngOnInit();

    expect(component.error).toBe('Erro ao carregar dados do dashboard');
    expect(component.loading).toBeFalse();
  });

  it('should format currency correctly', () => {
    const formatted = component.formatCurrency(1000);
    expect(formatted).toContain('1.000');
  });

  it('should return correct status class', () => {
    expect(component.getStatusClass('emitida')).toBe('status-emitida');
    expect(component.getStatusClass('cancelada')).toBe('status-cancelada');
    expect(component.getStatusClass('denegada')).toBe('status-denegada');
    expect(component.getStatusClass('inutilizada')).toBe('status-inutilizada');
  });
});