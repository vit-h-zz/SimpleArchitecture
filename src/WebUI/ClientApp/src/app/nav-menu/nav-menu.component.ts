import { Component, Inject } from '@angular/core';
import { API_BASE_URL } from "../web-api-client";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  isExpanded = false;
  apiBaseUrl: string;

  constructor(@Inject(API_BASE_URL) baseUrl?: string) {
    this.apiBaseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
