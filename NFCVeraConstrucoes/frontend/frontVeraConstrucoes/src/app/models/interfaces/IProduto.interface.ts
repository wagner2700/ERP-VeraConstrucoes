export interface Produto {
  id: number;
  descricao: string;
  quantidade: number;
  valorUnitario: number;
  ncm: string;
  cfop: string;
  unidade: string;
  cest?: string;
  valorTotal: number; // opcional, pode ser calculado no backend
}
