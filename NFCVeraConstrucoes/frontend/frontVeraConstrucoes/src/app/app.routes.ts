import { Routes } from '@angular/router';
import { Layout } from './componentes/layout/layout';
import { Emissao } from './pages/emissao/emissao';
import { Produtos } from './pages/produtos/produtos';
import { NfceDetalhe } from './componentes/nfce-detalhe/nfce-detalhe/nfce-detalhe';
import { NfcLista } from './componentes/nfc-lista/nfc-lista';
import { Configuracao } from './pages/configuracao/configuracao';
import { DashboardComponent } from './pages/dashboard/dashboard';


export const routes: Routes = [
  {
    path: '', // Rota Raiz
    component: Layout,
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent,
      },
      {
        path: 'home',
        redirectTo: 'dashboard',
        pathMatch: 'full',
      },
      {
        path: 'emissao', // Quando acessar /emissão
        component: Emissao, // Encaixe a FOTO da Emissão
      },
      {
        path: 'produtos',
        component: Produtos,
      },
      {
        path: 'nfce/lista',
        component: NfcLista,
      },
      {
        path: 'nfce/detalhe/:id',
        component: NfceDetalhe,
      },
      // Redireciona para emissão se não digitar nada
      {
        path: '',
        redirectTo: 'emissao',
        pathMatch: 'full',
      },
      { path: 'config', component: Configuracao },
      {
        path: '**',
        redirectTo: '',
        pathMatch: 'full',
      },
    ],
  },
];
