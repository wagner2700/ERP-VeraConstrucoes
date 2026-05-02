import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NFCeResumo } from '../../models/interfaces/INFCResumo.interface';
import { NFC } from '../../services/nfc';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nfc-lista',
  imports: [CommonModule, FormsModule],
  templateUrl: './nfc-lista.html',
  styleUrl: './nfc-lista.css',
})
export class NfcLista {
lista: NFCeResumo[] = [];
  totalRegistros = 0;
  paginaAtual = 1;
  pageSize = 10;
  totalPaginas = 0;
  loading = false;

  filtros: any = {
    numero: null,
    cliente: '',
    dataInicio: '',
    dataFim: '',
    sucesso: undefined
  };

  ordenacao = {
    campo: 'dataEmissao',
    direcao: 'desc' as 'asc' | 'desc'
  };

  constructor(
    private nfceService: NFC,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregarLista();
  }

  carregarLista(): void {
    this.loading = true;
    // Passa filtros e ordenação para o serviço (se o backend suportar)
    //Neste exemplo, ordenação pode ser feita localmente ou enviada via params
    this.nfceService.getNfceList().subscribe({
      next: (result) => {
        console.error(result);
        this.lista = result;
        // this.totalRegistros = result.totalCount;
        // this.totalPaginas = Math.ceil(this.totalRegistros / this.pageSize);
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  aplicarFiltros(): void {
    this.paginaAtual = 1;
    this.carregarLista();
  }

  limparFiltros(): void {
    this.filtros = {
      numero: null,
      cliente: '',
      dataInicio: '',
      dataFim: '',
      sucesso: undefined
    };
    this.paginaAtual = 1;
    this.carregarLista();
  }

  mudarPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas) return;
    this.paginaAtual = pagina;
    this.carregarLista();
  }

  ordenarPor(campo: string): void {
    if (this.ordenacao.campo === campo) {
      // inverte direção
      this.ordenacao.direcao = this.ordenacao.direcao === 'asc' ? 'desc' : 'asc';
    } else {
      this.ordenacao.campo = campo;
      this.ordenacao.direcao = 'asc';
    }
    // Se o backend suportar ordenação, envie nos parâmetros
    // Por simplicidade, farei ordenação local
    this.lista = [...this.lista].sort((a, b) => {
      let valA = a[campo as keyof NFCeResumo];
      let valB = b[campo as keyof NFCeResumo];
      if (typeof valA === 'string' && typeof valB === 'string') {
        return this.ordenacao.direcao === 'asc' 
          ? valA.localeCompare(valB) 
          : valB.localeCompare(valA);
      }
      if (typeof valA === 'number' && typeof valB === 'number') {
        return this.ordenacao.direcao === 'asc' ? valA - valB : valB - valA;
      }
      if (campo === 'dataEmissao') {
        const da = new Date(a.dataEmissao).getTime();
        const db = new Date(b.dataEmissao).getTime();
        return this.ordenacao.direcao === 'asc' ? da - db : db - da;
      }
      return 0;
    });
  }

  visualizar(id: number): void {
    this.router.navigate(['/nfce/detalhe', id]);
  }

  imprimir(pdfPath: string): void {
    window.open(pdfPath, '_blank');
  }

  novaNFCe(): void {
    this.router.navigate(['/emissao']); // rota para emissão
  }

  formatarData(data: Date | string): string {
    return new Date(data).toLocaleString('pt-BR');
  }

  formatarMoeda(valor: number): string {
    return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }
}
