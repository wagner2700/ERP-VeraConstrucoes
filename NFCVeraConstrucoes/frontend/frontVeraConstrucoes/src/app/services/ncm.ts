import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

import { ncmModel } from '../models/interfaces/INcmModel.interface';

@Injectable({
  providedIn: 'root',
})
export class NcmService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = '/api/Ncm';

  private readonly loadingSubject = new BehaviorSubject<boolean>(false);
  readonly loading$ = this.loadingSubject.asObservable();

  private readonly ncmSubject = new BehaviorSubject<ncmModel[]>([]);
  readonly ncm$ = this.ncmSubject.asObservable();

  constructor() {
    this.refresh();
  }

  refresh(): void {
    this.loadingSubject.next(true);

    this.http.get<ncmModel[]>(this.apiUrl).subscribe({
      next: (response) => {
        this.ncmSubject.next(this.normalizar(response ?? []));
        this.loadingSubject.next(false);
      },
      error: (error) => {
        console.error('Erro ao carregar NCM:', error);
        this.ncmSubject.next([]);
        this.loadingSubject.next(false);
      },
    });
  }

  private normalizar(lista: ncmModel[]): ncmModel[] {
    return [...lista].sort((first, second) => first.ncm.localeCompare(second.ncm));
  }
}
