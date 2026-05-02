export interface Pagamento {
 metodo: string;     // dinheiro, credito, debito, pix
  valor: number;
  parcelas?: number;
  troco?: number;
  //bandeira?: string;
}
