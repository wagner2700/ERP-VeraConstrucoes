import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import {  FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfiguracaoService } from '../../services/configuracao/configuracao';
import { ConfiguracaoNFCe } from '../../models/interfaces/IConfiguracaoNFCe.interface';

@Component({
  selector: 'app-configuracao',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './configuracao.html',
  styleUrl: './configuracao.css',
})
export class Configuracao implements OnInit{
 configForm: FormGroup;
  loading = true;
  saving = false;
  error = '';
  success = '';
  regimes = [
    { value: 1, label: 'Simples Nacional - ME' },
    { value: 2, label: 'Simples Nacional – Excesso de sublimite' },
    { value: 3, label: 'Regime Normal' },
    { value: 4, label: 'Simples Nacional - MEI' }
  ];
  ambientes = [
    { value: 1, label: 'Produção' },
    { value: 2, label: 'Homologação' }
  ];

  constructor(
    private fb: FormBuilder,
    private configService: ConfiguracaoService,
    private router: Router
  ) {
    this.configForm = this.fb.group({
      // Certificado
      caminhoCertificado: ['', Validators.required],
      senhaCertificado: ['', Validators.required],

      // Série e numeração
      serieNota: [1, [Validators.required, Validators.min(1)]],
      ultimoNumeroNota: [0, [Validators.required, Validators.min(0)]],

      // Dados do emitente
      cnpjEmitente: ['', [Validators.required, Validators.pattern(/^\d{14}$/)]],
      razaoSocial: ['', Validators.required],
      nomeFantasia: ['', Validators.required],
      inscricaoEstadual: ['', Validators.required],
      regimeTributario: [1, Validators.required],

      // Endereço
      logradouroEmitente: ['', Validators.required],
      numeroEmitente: ['', Validators.required],
      bairroEmitente: ['', Validators.required],
      complementoEmitente: [''],
      codigoMunicipioIBGE: [3550308, [Validators.required, Validators.min(1000000)]],
      nomeMunicipio: ['', Validators.required],
      uf: ['SP', [Validators.required, Validators.maxLength(2)]],
      estado: [35, Validators.required],
      cep: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]],
      telefone: ['', [Validators.required, Validators.pattern(/^\d{10,11}$/)]],
      unidadeFederativaCodigo: [35, Validators.required],

      // Ambiente
      ambiente: [2],
      tipoEmissao: [1],

      // CSC
      csc: ['', Validators.required],
      idCSC: ['', Validators.required],

      // Timeouts e tentativas
      timeoutTransmissao: [30, [Validators.required, Validators.min(1)]],
      tentativasConsultaRecibo: [10, [Validators.required, Validators.min(1)]],
      intervaloConsultaRecibo: [3000, [Validators.required, Validators.min(500)]],

      // Pastas
      pastaXmlEnviados: ['XMLs/Enviados', Validators.required],
      pastaXmlAutorizados: ['XMLs/Autorizados', Validators.required],
      pastaXmlCancelados: ['XMLs/Cancelados', Validators.required],
      pastaDanfes: ['DANFEs', Validators.required],
      versao: ['4.00', Validators.required]
    });
  }

  ngOnInit(): void {
    this.carregarConfiguracao();
  }

  carregarConfiguracao(): void {
    this.loading = true;
    this.configService.obterConfiguracao().subscribe({
      next: (data) => {
        
        this.configForm.patchValue(data);
        this.loading = false;
      },
      error: (err) => {
        if (err.status === 404) {
          // Configuração não encontrada – usar valores padrão
          this.loading = false;
        } else {
          this.error = 'Erro ao carregar configurações.';
          console.error(err);
          this.loading = false;
        }
      }
    });
  }

  salvar(): void {
    if (this.configForm.invalid) {
      this.configForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.success = '';
    this.error = '';

    const config: ConfiguracaoNFCe = {
      id: 1,
      ...this.configForm.value,
      ambiente: Number(this.configForm.value.ambiente),
      regimeTributario: Number(this.configForm.value.regimeTributario),
      estado: Number(this.configForm.value.estado),
      unidadeFederativaCodigo: Number(this.configForm.value.unidadeFederativaCodigo),
      versao: Number(this.configForm.value.versao),
    };

    this.configService.salvarConfiguracao(config).subscribe({
      next: () => {
        this.success = 'Configurações salvas com sucesso!';
        this.saving = false;
        setTimeout(() => this.success = '', 3000);
      },
      error: (err) => {
        this.error = 'Erro ao salvar configurações.';
        
        this.saving = false;
        console.log(config)
        console.log(err)
        console.log('Detalhes da validação:', err.error);
      }
    });
  }

  voltar(): void {
    this.router.navigate(['/']);
  }
}
