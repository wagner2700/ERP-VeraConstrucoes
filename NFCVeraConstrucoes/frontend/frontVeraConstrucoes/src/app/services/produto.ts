import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { produtoModel, PagedResponseProducts } from '../models/interfaces/IProdutoModel.interface';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ProdutoNFCe } from '../models/interfaces/IProdutoNFCe.interface';

@Injectable({
  providedIn: 'root',
})
export class Produto {
  // O HttpClient agora é injetado
  private http = inject(HttpClient);
  // O proxy.conf.json vai redirecionar '/api'
  private apiUrl = '/api/Produto';

  // 3. Controle de loading
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();

  // 1. Criamos o BehaviorSubject (nosso "rádio transmissor")
  private produtosSubject = new BehaviorSubject<produtoModel[]>([]);

  // 2. Criamos um Observable (o "rádio" que outros podem sintonizar)
  produtos$ = this.produtosSubject.asObservable();

  constructor() {
    this.carregarProdutosIniciais();
  }

  BuscarProdutoPorDescritivoOuCodigo(termo: string): Observable<ProdutoNFCe[]> {
    return this.http.post<ProdutoNFCe[]>(`${this.apiUrl}/buscar`, termo);
  }

  private carregarProdutosIniciais(): void {
    this.loadingSubject.next(true);
    this.http.get<produtoModel>(this.apiUrl).subscribe({
      next: (resposta) => {
        // Extrai array de produtos da resposta
        var produtos = this.extrairProdutosDaResposta(resposta);

        produtos = produtos.map((p) => ({
          ...p, // mantém todas as propriedades originais
          quantidade: 1, // valor inicial
          noCarrinho: false,
        }));

        console.log(`📊 ${produtos.length} produtos encontrados`);
        this.produtosSubject.next(produtos);
        this.loadingSubject.next(false);
      },
      error: (erro) => {
        console.error('❌ Erro ao carregar produtos:', erro);
        this.produtosSubject.next([]);
        this.loadingSubject.next(false);
      },
    });
  }

  /**
   * Atualiza a lista do servidor (chamado após CRUD)
   */
  private atualizarLista(): void {
    this.loadingSubject.next(true);
    this.http.get<any>(this.apiUrl).subscribe({
      next: (resposta) => {
        const produtos = this.extrairProdutosDaResposta(resposta);
        this.produtosSubject.next(produtos);
        this.loadingSubject.next(false);
      },
      error: (erro) => {
        console.error('❌ Erro ao atualizar lista:', erro);
        this.loadingSubject.next(false);
      },
    });
  }

  /**
   * Extrai array de produtos da resposta da API
   */
  private extrairProdutosDaResposta(resposta: any): produtoModel[] {
    // Tenta encontrar produtos em diferentes formatos
    return resposta?.products || resposta?.Products || resposta || [];
  }

  // ========== MÉTODOS PÚBLICOS ==========

  /**
   * SALVAR PRODUTO (com atualização automática)
   */
  salvar(produto: produtoModel): Observable<any> {
    this.loadingSubject.next(true);

    return this.http.post(this.apiUrl, produto).pipe(
      tap({
        next: () => {
          this.atualizarLista(); // ← ATUALIZAÇÃO AUTOMÁTICA
        },
        error: (erro) => {
          console.error('❌ Erro ao salvar:', erro);
          this.loadingSubject.next(false);
        },
      }),
    );
  }

  /**
   * LISTAR TODOS PRODUTOS (retorna Observable do BehaviorSubject)
   */
  listarTodosProdutos(): Observable<produtoModel[]> {
    return this.produtos$;
  }

  /**
   * BUSCAR PRODUTOS DIRETO DA API (força refresh)
   */
  buscarProdutos(): Observable<produtoModel[]> {
    console.log('🔍 Buscando produtos da API...');
    return this.http.get<produtoModel[]>(this.apiUrl).pipe(
      tap({
        next: (produtos) => {
          console.log(`📦 ${produtos.length} produtos encontrados`);
          this.produtosSubject.next(produtos);
        },
      }),
    );
  }

  /**
   * EXCLUIR PRODUTO (com atualização automática)
   */
  excluirProduto(id: number): Observable<any> {
    console.log(`🗑️ Excluindo produto ID: ${id}`);
    this.loadingSubject.next(true);

    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      tap({
        next: () => {
          console.log('✅ Produto excluído');
          this.atualizarLista(); // ← ATUALIZAÇÃO AUTOMÁTICA
        },
        error: (erro) => {
          console.error('❌ Erro ao excluir:', erro);
          this.loadingSubject.next(false);
        },
      }),
    );
  }

  /**
   * ATUALIZAR PRODUTO (com atualização automática)
   */
  atualizarProduto(produto: produtoModel): Observable<any> {
    this.loadingSubject.next(true);

    return this.http.put(`${this.apiUrl}`, produto).pipe(
      tap({
        next: () => {
          console.log('✅ Produto atualizado');
          this.atualizarLista();
        },
        error: (erro) => {
          console.error('❌ Erro ao atualizar:', erro);
          this.loadingSubject.next(false);
        },
      }),
    );
  }

  /**
   * FORÇAR ATUALIZAÇÃO MANUAL (para botão de refresh)
   */
  refresh(): void {
    console.log('🔃 Refresh manual');
    this.atualizarLista();
  }

  /**
   * CARREGAR PRODUTOS PAGINADOS
   */
  carregarProdutosPaginado(
    page: number = 1,
    pageSize: number = 10,
  ): Observable<PagedResponseProducts> {
    console.log(`📄 Carregando produtos página ${page} (tamanho: ${pageSize})`);

    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResponseProducts>(`${this.apiUrl}/paginado`, { params });
  }
}
