export interface produtoModel {
  id: number;
  descricao: string;
  valorUnitario: number;
  estoque: number;
  ncm: string;
}

export interface PagedResponseProducts {
  items: produtoModel[];
  currentPage: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}
