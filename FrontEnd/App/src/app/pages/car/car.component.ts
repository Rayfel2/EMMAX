import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-car',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.css']
})
export class CarComponent implements OnInit {
  carritoProductos: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getCarritoProductos(1, 10); // Puedes ajustar los valores de página y pageSize según tus necesidades.
  }

  getCarritoProductos(page: number, pageSize: number): void {
    const url = `http://localhost:5230/CarritoProducto?page=${page}&pageSize=${pageSize}`;

    const token = localStorage.getItem('token'); // Recupera el token almacenado en el almacenamiento local
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any[]>(url, { headers })
    .subscribe(
      (data: any[]) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad carritoProductos
        console.log(data);
        this.carritoProductos = data;
      },
      (error) => {
        console.error('Error al obtener los productos del carrito', error);
      }
    );
  }
}
