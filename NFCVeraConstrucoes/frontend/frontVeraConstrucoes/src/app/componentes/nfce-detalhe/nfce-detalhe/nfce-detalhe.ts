import { Component } from '@angular/core';
import { NFCeCompleta } from '../../../models/interfaces/INFCeCompleta.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { NFC } from '../../../services/nfc';
import { CommonModule } from '@angular/common';
import { NFCeDetalhesResponse } from '../../../models/interfaces/INFCeDetalhesResponse.interface';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CancelamentoRequest } from '../../../models/interfaces/ICancelamentoRequest.interface';
import { RetornoEvento } from '../../../models/interfaces/IRetornoEvento.interface';

@Component({
  selector: 'app-nfce-detalhe',
  imports: [CommonModule],
  templateUrl: './nfce-detalhe.html',
  styleUrl: './nfce-detalhe.css',
})
export class NfceDetalhe {
  nfceCompleta?: NFCeDetalhesResponse;
  loading = true;
  error = false;
  qrCodeSafeUrl?: SafeResourceUrl;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private nfceService: NFC,
    private sanitizer: DomSanitizer,
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.carregarDados(+id);
    } else {
      this.error = true;
      this.loading = false;
    }
  }

  carregarDados(id: number): void {
    this.loading = true;
    this.nfceService.getNfceCompleta(id).subscribe({
      next: (data) => {
        console.log(data);
        if (data.qrCodeUrl) {
          this.qrCodeSafeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(data.qrCodeUrl);
        }
        this.nfceCompleta = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = true;
        this.loading = false;
      },
    });
  }

  getSituacaoTexto(situacao: string): string {
    const mapaTexto: { [key: string]: string } = {
      F: 'Autorizada',
      R: 'Rejeitada',
      C: 'Cancelada',
    };
    return mapaTexto[situacao] || 'Desconhecido';
  }

  getSituacaoClasse(situacao: string): string {
    const mapaClasse: { [key: string]: string } = {
      F: 'badge-success',
      R: 'badge-danger',
      C: 'badge-secondary',
    };
    return mapaClasse[situacao] || 'badge-secondary';
  }

  cancelar() {
    if(!window.confirm('Deseja cancelar esta nota fiscal ?')){
      return; // Se o usuário clicar em "Cancelar", interrompe a execução
    }

    this.loading = true; // Ativa indicador de carregamento (se houver)
    this.error = false;  // Limpa erro anterior

    const requestCancelamento: CancelamentoRequest = {
      idNota: this.nfceCompleta?.id,
      chaveNfe: this.nfceCompleta?.chaveAcesso,
      protocoloAutorizacao: this.nfceCompleta?.protocolo,
    };

    this.nfceService.cancelarNfc(requestCancelamento).subscribe({
      next: (data: RetornoEvento[]) => {
        this.loading = false;
        // Verifica o primeiro evento (geralmente só um)
      const evento = data[0]?.infEvento;
      if (!evento) {
        alert('Resposta inválida do servidor.');
        return;
      }

      // Códigos de sucesso: 135 (cancelamento efetuado) ou 101 (lote processado)
      if (evento.cStat === 135 || evento.cStat === 101 || evento.cStat === 573 ) {
        // Sucesso
        if (this.nfceCompleta) {
          this.nfceCompleta.situacaoNota = 'C'; // Atualiza localmente
        }
        alert(`Nota cancelada com sucesso! Protocolo: ${evento.nProt} (cStat: ${evento.cStat}) ${evento.xMotivo} `);
      } else {
        // Erro na operação (ex: 501 - prazo excedido, 573 - duplicidade)
        alert(`Erro no cancelamento: ${evento.xMotivo} (cStat: ${evento.cStat})`);
        // Opcional: manter a nota com status original
      }

      },
      error: (err) => {
        console.error(err);
        this.error = true;
        this.loading = false;
      },
    });
  }

  // Formatação de moeda
  formatarMoeda(valor: number): string {
    return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  // Mapeamento do código de pagamento para nome amigável
  getMetodoPagamento(codigo: string): string {
    const mapa: Record<string, string> = {
      '01': 'Dinheiro',
      '02': 'Cheque',
      '03': 'Cartão de Crédito',
      '04': 'Cartão de Débito',
      '05': 'Crédito Loja',
      '10': 'Vale Alimentação',
      '11': 'Vale Refeição',
      '12': 'Vale Presente',
      '13': 'Vale Combustível',
      '14': 'Duplicata Mercantil',
      '15': 'Boleto Bancário',
      '16': 'Sem Pagamento',
      '17': 'Outro',
      '18': 'Fatura',
      '19': 'PIX',
    };
    return mapa[codigo] || codigo;
  }

  // Formatação de data/hora
  formatarData(data: Date): string {
    return new Date(data).toLocaleString('pt-BR');
  }

  // Cálculo do total dos produtos (opcional, mas já temos no objeto)
  calcularTotalProdutos(): number {
    return (
      this.nfceCompleta?.produtos.reduce((acc, p) => acc + p.quantidade * p.valorUnitario, 0) || 0
    );
  }

  // Ações
  voltar(): void {
    this.router.navigate(['/nfce']); // ou para lista
  }

  imprimirDanfe(): void {
    if (this.nfceCompleta?.pdfPath) {
      window.open(this.nfceCompleta.pdfPath, '_blank');
    }
  }
}
