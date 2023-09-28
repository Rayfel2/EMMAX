import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.css']
})

export class ShopComponent {
  datos: any[] = []; // Declaración de la propiedad datos

  constructor(private httpClient: HttpClient) { }

  getDataFromApi() {
    const apiUrl = 'http://localhost:5230/Producto';

    this.httpClient.get(apiUrl)
      .subscribe((data: any) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad del componente
        // data contiene la respuesta de la API
        console.log(data);
        this.datos = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });
  }

  ngOnInit() {
    this.getDataFromApi();
  }
}
