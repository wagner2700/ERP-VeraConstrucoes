export interface ResultadoEmissao {
  id: number;
  sucesso: boolean;
  mensagem: string;
  mensagemErro: string;
  chaveAcesso: string;
  protocolo: string;
  numero: number;
  serie: number;
  xmlPath: string;
  pdfPath: string;
  qrCodeUrl: string;
}
