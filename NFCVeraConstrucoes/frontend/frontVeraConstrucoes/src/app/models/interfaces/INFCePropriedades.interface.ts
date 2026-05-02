export interface NFCePropriedades {
  id: number;
  nfceId: number;
  observacoes?: string;
  desconto: number;
  acrescimo: number;
  xmlPath?: string;
  pdfPath?: string;
  qrCodeUrl?: string;
  mensagemErro?: string;
  chaveAcesso?: string;
  protocolo?: string;
}