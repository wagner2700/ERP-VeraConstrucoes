export interface IDashboardSummary {
  totalNotasFiscais: number;
  notasEmitidasHoje: number;
  notasCanceladasHoje: number;
  valorTotalNotasHoje: number;
  valorMedioNotas: number;
  ultimasNotas: INotaFiscalResumo[];
}

export interface INotaFiscalResumo {
  id: string;
  numero: string;
  serie: string;
  dataEmissao: Date;
  valorTotal: number;
  status: 'emitida' | 'cancelada' | 'denegada' | 'inutilizada';
  clienteNome?: string;
}