import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { NFCeRequest } from '../models/interfaces/INfcRequest.interface';
import { Observable } from 'rxjs';
import { NFCeResponse } from '../models/interfaces/INfcResponse.interface';
import { ConfiguracaoSistema } from '../models/interfaces/IConfiguracaoSistema.interface';
import { ResultadoEmissao } from '../models/interfaces/IResultadoEmissao.interface';
import { NFCeCompleta } from '../models/interfaces/INFCeCompleta.interface';
import { NFCeDetalhesResponse } from '../models/interfaces/INFCeDetalhesResponse.interface';
import { NFCeResumo } from '../models/interfaces/INFCResumo.interface';
import { CancelamentoRequest } from '../models/interfaces/ICancelamentoRequest.interface';
import { RetornoEventoArray } from '../models/interfaces/IRetornoEvento.interface';

@Injectable({
  providedIn: 'root',
})
export class NFC {
  // O HttpClient agora é injetado
  private http = inject(HttpClient);

  // O proxy.conf.json vai redirecionar '/api'
  private apiUrl = '/api/Nfc';

  emitirNfc(nfceRequest: NFCeRequest): Observable<ResultadoEmissao> {
    return this.http.post<ResultadoEmissao>(`${this.apiUrl}/transmitir`, nfceRequest);
  }

  getNfceCompleta(id: number): Observable<NFCeDetalhesResponse> {
    return this.http.get<NFCeDetalhesResponse>(`${this.apiUrl}/detalhes/${id}`);
  }

  getNfceList(): Observable<NFCeResumo[]> {
    return this.http.get<NFCeResumo[]>(`${this.apiUrl}/listarNotasFiscais/`);
  }

  cancelarNfc(cancelamento: CancelamentoRequest): Observable<RetornoEventoArray> {
    return this.http.post<RetornoEventoArray>(`${this.apiUrl}/cancelar`, cancelamento);
  }
}
