// models/interfaces/INFCeResumo.interface.ts
export interface NFCeResumo {
  id: number;
  numero: number;
  serie: number;
  dataEmissao: Date; // string ISO
  valorTotal: number;
  statusProcessamento: boolean;
  clienteNome?: string;
  clienteDocumento?: string;
  chaveAcesso?: string;
  protocolo?: string;
  // opcional: qtd produtos, etc.
}