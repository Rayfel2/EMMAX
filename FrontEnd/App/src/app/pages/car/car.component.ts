import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-car',
  templateUrl: './car.component.html',
  styleUrls: ['./car.component.css']
})
export class CarComponent implements OnInit {
  carritoProductos: any[] = [];


  listaRequest = {
    idListaProducto: 0,
    idProducto: 0,
  };

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getCarritoProductos(1, 8); // Puedes ajustar los valores de página y pageSize según tus necesidades.
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

  
  eliminarProductoDelCarrito(index: number) {
    const idProductoAEliminar = index;

  
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.http.delete(`http://localhost:5230/CarritoProducto/${idProductoAEliminar}`, { headers }).subscribe(
      (response: any) => {
        alert('Producto eliminado del carrito');
        this.getCarritoProductos(1, 8);
      },
      (error) => {
        console.error('Error al eliminar producto del carrito', error);
      }
    );
  }

  lista(index: number) {
    this.listaRequest = {
      idProducto: index,
      idListaProducto: 0, 
    };
   
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.http.post('http://localhost:5230/ListaProducto', this.listaRequest, { headers }).subscribe(
      (response: any) => {
        alert('Producto agregado a la lista');
        this.getCarritoProductos(1, 8);
      },
      (error) => {
        console.error('Error al agregar producto a la lista', error);
      }
    );
  }
}
