import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ConfiguracaoNFCe } from '../../models/interfaces/IConfiguracaoNFCe.interface';
import { environment } from '../../../enviroments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class ConfiguracaoService {
  private apiUrl = environment.apiUrl; // '/api/Configuracao';

  constructor(private http: HttpClient) {}

  obterConfiguracao(): Observable<ConfiguracaoNFCe> {
    return this.http.get<ConfiguracaoNFCe>(`${this.apiUrl}/Configuracao`);
  }

  salvarConfiguracao(config: ConfiguracaoNFCe): Observable<ConfiguracaoNFCe> {
    return this.http.post<ConfiguracaoNFCe>(`${this.apiUrl}/Configuracao`, config);

    
  }
}
