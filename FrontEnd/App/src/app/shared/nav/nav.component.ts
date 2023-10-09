import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  searchText: string = ''; // Propiedad para almacenar el valor del campo de búsqueda

  constructor(private router: Router) {}

  // Función para redireccionar al componente ShopComponent con el valor de búsqueda
  searchProducts() {
    if (this.searchText) {
      // Redirige a la ruta de ShopComponent y pasa el valor de búsqueda como parámetro
      this.router.navigate(['/tienda'], { queryParams: { search: this.searchText } });
    } else {
      // Si el campo de búsqueda está vacío, redirige a la página de inicio
      this.router.navigate(['/inicio']);
    }
  }
}
