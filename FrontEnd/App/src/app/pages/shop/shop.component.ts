import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})

export class ShopComponent {
  datos: any[] = []; // Declaración de la propiedad datos
  categorias: any[] = [];
  filterProducto: string = 'all'; // Declaración de la constante filterProducto

  constructor(private httpClient: HttpClient) { }

  getDataFromApi() {
    const productoApi = 'http://localhost:5230/Producto';
    const resenaApi = 'http://localhost:5230/Categoria';

    this.httpClient.get(productoApi)
      .subscribe((data: any) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad del componente
        // data contiene la respuesta de la API
        console.log(data);
        this.datos = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });

    this.httpClient.get(resenaApi)
      .subscribe((data: any) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad del componente
        // data contiene la respuesta de la API
        console.log(data);
        this.categorias = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });
  }

  // Función para manejar el cambio en la selección
  onCategoriaChange(selectedCategoria: string) {
    // Almacena la opción seleccionada en la constante filterProducto
    this.filterProducto = selectedCategoria;
    console.log('Opción seleccionada:', this.filterProducto);

    // Aquí puedes realizar la lógica para filtrar los productos según la categoría seleccionada
  }

  ngOnInit() {
    this.getDataFromApi();
  }
}
