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
  filterProducto: string = '--'; // Declaración de la constante filterProducto
  page: number = 1; // Página actual
  pageSize: number = 8; // Tamaño de página
  categoryFilter: string = '---'; // Filtro de categoría

  constructor(private httpClient: HttpClient) { }
  hasMoreRecords: boolean = true; 
  getDataFromApi() {
    let productoApi: string;
    if (this.categoryFilter === '---') {
      productoApi = `http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}`;
    } else {
      productoApi = `http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&categoryFilter=${this.categoryFilter}`;
    }
    const resenaApi = 'http://localhost:5230/Categoria';


    this.httpClient.get(productoApi)
    .subscribe((data: any) => {
      if (data.length < this.pageSize) {
        this.hasMoreRecords = false;
      } else {
        this.hasMoreRecords = true;
      }
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
  onCategoriaChange(event: any) {
    this.filterProducto = event.target.value;
    console.log('Opción seleccionada:', this.filterProducto);
    // Aquí puedes realizar la lógica para filtrar los productos según la categoría seleccionada
  }
  
  nextPage() {
    this.page++;
    this.getDataFromApi();
  }
  
  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.getDataFromApi();
    }
  }
  
  

  ngOnInit() {
    this.getDataFromApi();
  }
}
