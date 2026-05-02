import { Cliente } from "./ICliente.interface";
import { Produto } from "./IProduto.interface";
import { Pagamento } from "./Pagamento.interface";

export interface NFCeRequest {
  cliente: Cliente | null;
  produtos: Produto[];
  pagamentos: Pagamento[];
  observacoes?: string;
  desconto: number;
  acrescimo: number;
}