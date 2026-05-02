export interface NFCeResponse {
  sucesso: boolean;
  mensagem: string;
  chaveAcesso: string;
  numero: string;
  serie: string;
  dataEmissao: string;
  protocolo: string;
  xml: string;
  danfeUrl: string;
  qrCode: string;
  valorTotal: number;
  erros: string[];
}