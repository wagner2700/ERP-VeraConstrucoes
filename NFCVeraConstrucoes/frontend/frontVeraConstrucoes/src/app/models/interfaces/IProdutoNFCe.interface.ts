export interface ProdutoNFCe {
  id: number;
  nfceId: number;
  codigo: string;
  descricao: string;
  quantidade: number;
  valorUnitario: number;
  ncm: string;
  cfop: string;
  unidade: string;
  cest?: string;
}