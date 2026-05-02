export interface NFCe {
  id: number;
  numero: number;
  serie: number;
  dataEmissao: Date;
  valorTotal: number;
  statusProcessamento: boolean;
  situacaoNota: string;
  clienteDocumento?: string;
  clienteNome?: string;
  clienteEmail?: string;
  clienteTelefone?: string;
}
