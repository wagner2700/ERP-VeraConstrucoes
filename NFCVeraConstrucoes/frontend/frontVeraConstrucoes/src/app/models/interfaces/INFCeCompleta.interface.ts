import { NFCe } from "./INFCe.interface";
import { NFCePropriedades } from "./INFCePropriedades.interface";
import { PagamentoNFCe } from "./IPagamentoNFCe.interface";
import { ProdutoNFCe } from "./IProdutoNFCe.interface";

// View completa
export interface NFCeCompleta {
  nfce: NFCe;
  propriedades: NFCePropriedades;
  produtos: ProdutoNFCe[];
  pagamentos: PagamentoNFCe[];
}