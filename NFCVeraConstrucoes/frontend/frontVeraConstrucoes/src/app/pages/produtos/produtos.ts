import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Subscription } from 'rxjs';

import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DrawerModule } from 'primeng/drawer';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { PaginatorModule } from 'primeng/paginator';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ToolbarModule } from 'primeng/toolbar';

import { ncmModel } from '../../models/interfaces/INcmModel.interface';
import {
  produtoModel,
  PagedResponseProducts,
} from '../../models/interfaces/IProdutoModel.interface';
import { NcmService } from '../../services/ncm';
import { Produto } from '../../services/produto';

@Component({
  selector: 'app-produtos',
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    DrawerModule,
    DialogModule,
    InputTextModule,
    InputNumberModule,
    ToolbarModule,
    TagModule,
    ProgressSpinnerModule,
    PaginatorModule,
  ],
  templateUrl: './produtos.html',
  styleUrl: './produtos.css',
})
export class Produtos implements OnInit, OnDestroy {
  private readonly produtoService = inject(Produto);
  private readonly ncmService = inject(NcmService);

  carregando = false;
  carregandoNcm = false;
  sidebarVisivel = false;
  modalNcmVisivel = false;
  termoBuscaNcm = '';

  produto: produtoModel = this.criarProdutoVazio();

  // Propriedades de paginação
  produtosPaginados: produtoModel[] = [];
  produtosFiltrados: produtoModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;

  listaNcm: ncmModel[] = [];
  ncmFiltrados: ncmModel[] = [];

  private readonly subscriptions: Subscription[] = [];

  ngOnInit(): void {
    this.carregarPagina(1);

    const ncmSubscription = this.ncmService.ncm$.subscribe((items) => {
      this.listaNcm = items;
      this.atualizarFiltroNcm();
    });

    const ncmLoadingSubscription = this.ncmService.loading$.subscribe((loading) => {
      this.carregandoNcm = loading;
    });

    this.subscriptions.push(ncmSubscription, ncmLoadingSubscription);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  get ncmSelecionado(): ncmModel | undefined {
    return this.listaNcm.find((item) => item.ncm === this.produto.ncm);
  }

  getSeverity(
    estoque: number,
  ): 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined {
    if (estoque > 20) return 'success';
    if (estoque > 10) return 'info';
    if (estoque > 5) return 'warn';
    return 'danger';
  }

  getSeverityValor(
    valor: number,
  ): 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined {
    if (valor > 1000) return 'danger';
    if (valor > 500) return 'warn';
    return 'success';
  }

  getSeverityCsosn(
    csosn: number,
  ): 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast' | undefined {
    if (csosn === 102 || csosn === 400) return 'success';
    if (csosn === 500) return 'warn';
    return 'info';
  }

  filtrarProdutos(termo: string): void {
    if (!termo.trim()) {
      this.produtosFiltrados = [];
      return;
    }

    const termoLower = termo.toLowerCase();

    this.produtosFiltrados = this.produtosPaginados.filter((item) =>
      item.descricao.toLowerCase().includes(termoLower),
    );
  }

  filtrarNcm(termo: string): void {
    this.termoBuscaNcm = termo;
    this.atualizarFiltroNcm();
  }

  novoProduto(): void {
    this.produto = this.criarProdutoVazio();
    this.sidebarVisivel = true;
  }

  editarProduto(prod: produtoModel): void {
    this.produto = { ...prod };
    this.sidebarVisivel = true;
  }

  abrirModalNcm(): void {
    this.modalNcmVisivel = true;
    this.termoBuscaNcm = '';
    this.atualizarFiltroNcm();

    if (!this.listaNcm.length && !this.carregandoNcm) {
      this.ncmService.refresh();
    }
  }

  fecharModalNcm(): void {
    this.modalNcmVisivel = false;
    this.termoBuscaNcm = '';
    this.atualizarFiltroNcm();
  }

  selecionarNcm(item: ncmModel): void {
    this.produto.ncm = item.ncm;
    this.fecharModalNcm();
  }

  limparNcm(): void {
    this.produto.ncm = '';
  }

  atualizarNcm(): void {
    this.ncmService.refresh();
  }

  excluirProduto(id: number): void {
    if (confirm('Tem certeza que deseja excluir este produto?')) {
      this.produtoService.excluirProduto(id).subscribe({
        next: () => {
          this.carregarPagina(this.currentPage);
          alert('Produto excluido com sucesso!');
        },
        error: (err) => {
          console.error('Erro ao excluir:', err);
          alert('Erro ao excluir produto');
        },
      });
    }
  }

  salvar(): void {
    if (!this.produto.descricao || this.produto.valorUnitario <= 0 || !this.produto.ncm) {
      alert('Por favor, preencha todos os campos obrigatorios!');
      return;
    }

    if (this.produto.id === 0) {
      this.produtoService.salvar(this.produto).subscribe({
        next: () => {
          this.sidebarVisivel = false;
          this.limparFormulario();
          this.carregarPagina(this.currentPage);
          alert('Produto salvo com sucesso!');
        },
        error: (err) => {
          console.error('Erro ao salvar:', err);
          alert('Erro ao conectar com o servidor C#');
        },
      });

      return;
    }

    this.produtoService.atualizarProduto(this.produto).subscribe({
      next: () => {
        this.sidebarVisivel = false;
        this.limparFormulario();
        this.carregarPagina(this.currentPage);
        alert('Produto atualizado com sucesso!');
      },
      error: (err) => {
        console.error('Erro ao atualizar:', err);
        alert('Erro ao atualizar produto');
      },
    });
  }

  atualizarLista(): void {
    this.carregarPagina(this.currentPage);
  }

  fecharSidebar(): void {
    this.sidebarVisivel = false;
    this.modalNcmVisivel = false;
    this.limparFormulario();
  }

  listarTodosProdutos(): void {
    this.produtoService.listarTodosProdutos().subscribe();
  }

  private limparFormulario(): void {
    this.produto = this.criarProdutoVazio();
    this.termoBuscaNcm = '';
    this.atualizarFiltroNcm();
  }

  private criarProdutoVazio(): produtoModel {
    return {
      id: 0,
      descricao: '',
      valorUnitario: 0,
      estoque: 0,
      ncm: '',
    };
  }

  private atualizarFiltroNcm(): void {
    const termo = this.termoBuscaNcm.trim().toLowerCase();

    if (!termo) {
      this.ncmFiltrados = [...this.listaNcm];
      return;
    }

    this.ncmFiltrados = this.listaNcm.filter((item) => {
      return (
        item.ncm.includes(termo.replace(/\D/g, '')) ||
        item.descricao.toLowerCase().includes(termo) ||
        item.csosn.toString().includes(termo)
      );
    });
  }

  private carregarPagina(page: number): void {
    this.carregando = true;
    this.currentPage = page;

    this.produtoService.carregarProdutosPaginado(page, this.pageSize).subscribe({
      next: (response: PagedResponseProducts) => {
        this.produtosPaginados = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.produtosFiltrados = []; // Limpa filtro ao carregar nova página
        this.carregando = false;
      },
      error: (err) => {
        console.error('Erro ao carregar página:', err);
        this.produtosPaginados = [];
        this.carregando = false;
      },
    });
  }

  onPageChange(event: any): void {
    const page = event.page + 1; // PrimeNG usa 0-based, nossa API usa 1-based
    this.pageSize = event.rows;
    this.carregarPagina(page);
  }
}
