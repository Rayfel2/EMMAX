import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})

export class ShopComponent {
  datos: any[] = [];
  categorias: any[] = [];
  filterProducto: string = '--';
  page: number = 1;
  pageSize: number = 8;
  categoryFilter: string = '---';
  searchText: string = ''; // Propiedad para almacenar el valor de búsqueda

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {
    // Obtiene el valor de búsqueda desde la URL al inicializar el componente
    this.route.queryParams.subscribe(params => {
      this.searchText = params['search'] || '';
      // Llama a la función para cargar datos con el valor de búsqueda si es necesario
      if (this.searchText) {
        this.getDataWithSearch();
      } else {
        // En caso contrario, carga los datos sin filtrar
        this.getData();
      }
    });
  }

  hasMoreRecords: boolean = true;
  getData() {
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
        console.log(data);
        this.categorias = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });
  }

  getDataWithSearch() {
    // Realiza la búsqueda solo si searchText tiene datos
    if (this.searchText) {
      let productoApi: string;
    
      if (this.categoryFilter === '---') {
        productoApi = `http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&productFilter=${this.searchText}`;
      } else {
        productoApi = `http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&categoryFilter=${this.categoryFilter}&productFilter=${this.searchText}`;
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
          console.log(data);
          this.categorias = data;
        }, (error) => {
          console.error('Error al obtener datos de la API', error);
        });
    } else {
      
    }
  }

  onCategoriaChange(event: any) {
    this.filterProducto = event.target.value;
    console.log('Opción seleccionada:', this.filterProducto);
    // Aquí puedes realizar la lógica para filtrar los productos según la categoría seleccionada
    // Llama a la función correspondiente según si searchText tiene datos o no
    if (this.searchText) {
      this.getDataWithSearch();
    } else {
      this.getData();
    }
  }

  nextPage() {
    this.page++;
    // Llama a la función correspondiente según si searchText tiene datos o no
    if (this.searchText) {
      this.getDataWithSearch();
    } else {
      this.getData();
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      // Llama a la función correspondiente según si searchText tiene datos o no
      if (this.searchText) {
        this.getDataWithSearch();
      } else {
        this.getData();
      }
    }
  }
}
