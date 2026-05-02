export interface PagamentoNFCe {
  id: number;
  nfceId: number;
  metodo: string; // código da SEFAZ (01, 03, etc.)
  valor: number;
  parcelas?: number;
  troco?: number;
  bandeira?: string;
}