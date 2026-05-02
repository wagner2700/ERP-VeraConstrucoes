import { PagamentoNFCe } from './IPagamentoNFCe.interface';
import { ProdutoNFCe } from './IProdutoNFCe.interface';

export interface NFCeDetalhesResponse {
  id: number;
  numero: number;
  serie: number;
  situacaoNota: string;
  dataEmissao: Date;
  valorTotal: number;
  statusProcessamento: boolean;
  clienteDocumento: string;
  clienteNome: string;
  clienteEmail: string;
  clienteTelefone: string;
  observacoes: string;
  desconto: number;
  acrescimo: number;
  xmlPath: string;
  pdfPath: string;
  qrCodeUrl: string;
  mensagemErro: string;
  chaveAcesso: string;
  protocolo: string;
  produtos: ProdutoNFCe[];
  pagamentos: PagamentoNFCe[];
}
