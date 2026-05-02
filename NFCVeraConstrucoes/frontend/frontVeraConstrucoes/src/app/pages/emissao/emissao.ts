import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { PaginatorModule } from 'primeng/paginator';
import { NFC } from '../../services/nfc';
import { NFCeRequest } from '../../models/interfaces/INfcRequest.interface';
import { finalize, Subscription } from 'rxjs';
import { ResultadoEmissao } from '../../models/interfaces/IResultadoEmissao.interface';
import { Produto } from '../../services/produto';
import { routes } from '../../app.routes';
import { Router } from '@angular/router';
import { PagedResponseProducts } from '../../models/interfaces/IProdutoModel.interface';

@Component({
  selector: 'app-emissao',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, NgxMaskDirective, PaginatorModule],
  templateUrl: './emissao.html',
  styleUrl: './emissao.css',
  providers: [provideNgxMask()],
})
export class Emissao implements OnInit {
  etapaAtual = 1;
  tipoCliente: string = 'anonimo';
  menuAtivo = 'nfce';
  emitindo = false;

  private nfceService = inject(NFC);
  private produtoService = inject(Produto);
  private route = inject(Router);

  private subscription: Subscription | undefined;

  // Formulários
  clienteForm!: FormGroup;

  // Produtos
  termoBusca = '';
  produtosPaginados: any[] = [];
  produtosFiltrados: any[] = [];
  carrinho: any[] = [];

  // Paginação
  currentPage = 1;
  pageSize = 4;
  totalCount = 0;
  totalPages = 0;

  // Valores
  subtotal = 0;
  desconto = 0;
  total = 0;
  resultado?: ResultadoEmissao;

  // Pagamento
  metodoSelecionado?: string;
  metodosPagamento = [
    { id: 'dinheiro', nome: 'Dinheiro', icone: '💵', descricao: 'Pagamento em espécie' },
    { id: 'credito', nome: 'Cartão Crédito', icone: '💳', descricao: 'Pagamento parcelado' },
    { id: 'debito', nome: 'Cartão Débito', icone: '🏧', descricao: 'Pagamento à vista' },
    { id: 'pix', nome: 'PIX', icone: '📱', descricao: 'Pagamento instantâneo' },
  ];

  valorPagamento = 0;
  valorRecebido = 0;
  troco = 0;
  parcelas = 1;
  mensagem: string = '';
  // Status
  statusSefaz = 'Online';
  statusSefazClass = 'status-online';

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.carregarPaginaProdutos(1);
    this.inicializarFormularios();
  }

  ngOnDestroy(): void {
    // Evita vazamento de memória
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  inicializarFormularios() {
    this.clienteForm = this.fb.group({
      documento: [''],
      nome: [''],
      email: ['', Validators.email],
      telefone: [''],
    });
  }
  atualizarValidadores(): void {
    const documentoControl = this.clienteForm.get('documento');
    const nomeControl = this.clienteForm.get('nome');

    // Limpa validadores anteriores
    documentoControl?.clearValidators();
    nomeControl?.clearValidators();

    if (this.tipoCliente === 'cpf') {
      // CPF: obrigatório, tamanho mínimo (após remover máscara) e validar CPF (opcional)
      documentoControl?.setValidators([Validators.required, Validators.minLength(11)]);
      nomeControl?.setValidators([Validators.required]);
    } else if (this.tipoCliente === 'cnpj') {
      // CNPJ: obrigatório, tamanho mínimo
      documentoControl?.setValidators([Validators.required, Validators.minLength(14)]);
      nomeControl?.setValidators([Validators.required]);
    } else {
      // Anônimo: nenhum campo obrigatório
      documentoControl?.setValidators(null);
      nomeControl?.setValidators(null);
    }

    // Atualiza o estado de validade dos campos
    documentoControl?.updateValueAndValidity();
    nomeControl?.updateValueAndValidity();
  }

  selecionarTipoCliente(tipo: string) {
    this.tipoCliente = tipo;
    this.atualizarValidadores();
  }

  buscarCliente() {
    // Implementar consulta de cliente
    console.log('Buscando cliente...');
  }

  carregarPaginaProdutos(page: number) {
    this.currentPage = page;
    this.produtoService.carregarProdutosPaginado(page, this.pageSize).subscribe({
      next: (response: PagedResponseProducts) => {
        this.produtosPaginados = response.items.map((p) => ({
          ...p,
          quantidade: 1,
          noCarrinho: false,
        }));
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;
        this.produtosFiltrados = [...this.produtosPaginados];
      },
      error: (err) => {
        console.error('Erro ao carregar produtos:', err);
        this.produtosPaginados = [];
        this.produtosFiltrados = [];
      },
    });
  }

  buscarProdutos() {
    if (this.termoBusca.trim() === '') {
      this.produtosFiltrados = [...this.produtosPaginados];
    } else {
      // Se há termo de busca, usar busca específica da API
      this.produtoService.BuscarProdutoPorDescritivoOuCodigo(this.termoBusca).subscribe({
        next: (produtos) => {
          this.produtosFiltrados = produtos.map((p) => ({
            ...p,
            quantidade: 1,
            noCarrinho: false,
          }));
        },
        error: (err) => {
          console.error('Erro ao buscar produtos:', err);
          this.produtosFiltrados = [];
        },
      });
    }
  }

  diminuirQuantidade(produto: any) {
    if (produto.quantidade > 1) {
      produto.quantidade--;
    }
  }

  aumentarQuantidade(produto: any) {
    if (produto.quantidade < produto.estoque) {
      produto.quantidade++;
    }
  }

  adicionarAoCarrinho(produto: any) {
    const item = {
      ...produto,
      quantidade: produto.quantidade || 1,
      valorUnitario: produto.valorUnitario,
      total: produto.valorUnitario * (produto.quantidade || 1),
    };
    this.carrinho.push(item);

    this.calcularTotais();
  }

  removerDoCarrinho(index: number) {
    this.carrinho.splice(index, 1);
    this.calcularTotais();
  }

  atualizarTotalItem(index: number) {
    const item = this.carrinho[index];
    item.total = item.quantidade * item.valorUnitario;
    this.calcularTotais();
  }

  calcularTotais() {
    this.subtotal = this.carrinho.reduce((sum, item) => sum + item.total, 0);
    this.total = this.subtotal - this.desconto;
    this.valorPagamento = this.total;
  }

  proximaEtapa() {
    if (this.etapaAtual === 1 && this.tipoCliente !== 'anonimo') {
      // Marca todos os campos como tocados para exibir os erros
      this.clienteForm.markAllAsTouched();
      if (this.clienteForm.invalid) {
        return; // não avança
      }
    }
    if (this.etapaAtual === 2) {
      if (this.carrinho.length <= 0) {
        alert('Necessário inserir itens na nota fiscal');
        return;
      }
    }
    if (this.etapaAtual < 4) {
      this.etapaAtual++;
    }
  }

  etapaAnterior() {
    if (this.etapaAtual > 1) {
      this.etapaAtual--;
    }
  }

  selecionarMetodo(metodoId: string) {
    this.metodoSelecionado = metodoId;
  }

  calcularTroco() {
    this.troco = Math.max(0, this.valorRecebido - this.valorPagamento);
  }

  emitirNFCe() {
    if (this.emitindo) {
      return;
    }

    this.emitindo = true;

    const clienteData =
      this.tipoCliente !== 'anonimo'
        ? {
            ...this.clienteForm.value,
            tipoDocumento: this.tipoCliente, // 'cpf' ou 'cnpj'
          }
        : null;

    // Chama a API para emitir NFC-e
    const dadosEmissao: NFCeRequest = {
      cliente: clienteData,
      produtos: this.carrinho.map((item) => ({
        id: item.id,
        descricao: item.descricao,
        quantidade: item.quantidade,
        valorUnitario: item.valorUnitario,
        valorTotal: item.valorTotal,
        ncm: item.ncm || '21069090', // valor padrão
        cfop: item.cfop || '5102', // valor padrão
        unidade: item.unidade || 'UN',
        cest: item.cest || '',
      })),
      pagamentos: [
        {
          metodo: this.metodoSelecionado!,
          valor: this.valorPagamento,
          parcelas: this.parcelas > 1 ? this.parcelas : undefined,
          troco: this.troco > 0 ? this.troco : undefined,
          //bandeira: this.metodoSelecionado === 'credito' ? //this.ba : undefined
        },
      ],
      observacoes: '', // campo opcional
      desconto: this.desconto,
      acrescimo: 0,
    };

    console.log('🚀 Dados enviados (JSON):', JSON.stringify(dadosEmissao, null, 2));
    this.nfceService
      .emitirNfc(dadosEmissao)
      .pipe(
        finalize(() => {
          this.emitindo = false;
        }),
      )
      .subscribe({
        next: (resultado: ResultadoEmissao) => {
          this.resultado = resultado;
          if (resultado.sucesso) {
            console.log('resultado ', resultado);
            this.route.navigate(['/nfce/detalhe/', resultado.id]);
            this.carrinho = [];
            this.calcularTotais();
            this.etapaAtual = 1;
          } else {
            // tratamento de erro retornado pela API
            console.log('debug erro ', resultado);

            alert('Erro na emissão: ' + resultado.mensagemErro);
          }
        },
        error: () => {
          console.error('Erro na comunicação com o servidor:');
          alert('Erro na comunicação com o servidor. Verifique a conexão e tente novamente.');
        },
      });
  }

  getMetodoPagamentoNome(): string {
    const metodo = this.metodosPagamento.find((m) => m.id === this.metodoSelecionado);
    return metodo ? metodo.nome : 'Não selecionado';
  }

  onPageChangeProdutos(event: any): void {
    const page = event.page + 1; // PrimeNG usa 0-based, nossa API usa 1-based
    this.pageSize = event.rows;
    this.carregarPaginaProdutos(page);
  }

  abrirCatalogo() {
    // Implementar abertura do catálogo
    console.log('Abrindo catálogo de produtos...');
  }
}
