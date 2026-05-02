import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Necessário para *ngIf e *ngFor
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router'; // Necessário para navegação
import { AvatarModule } from 'primeng/avatar'; // Para a foto do usuário
import { ButtonModule } from 'primeng/button'; // Botões bonitos
import { BadgeModule } from 'primeng/badge'; // Para o número de notificações

@Component({
  selector: 'app-layout',
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    AvatarModule,
    ButtonModule,
    BadgeModule,
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout {
  // Controle para abrir/fechar menu em telas pequenas
  sidebarOpen = true;

  // Lista e menus
  menuItems = [
    { label: 'Home', icon: 'pi pi-home', link: '/dashboard' },
    { label: 'Notas Fiscais', icon: 'pi pi-shopping-cart', link: 'nfce/lista' },
    { label: 'Emissão NFC-e', icon: 'pi pi-ticket', link: '/emissao' }, // Nossa tela principal}
    { label: 'Configurações', icon: 'pi pi-cog', link: '/config' },
    { label: 'Financeiro', icon: 'pi pi-wallet', link: '/financeiro' },
    { label: 'Produtos', icon: 'pi pi-box', link: '/produtos' },
  ];

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }
}
