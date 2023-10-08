import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit{
  listaProductos: any[] = [];

  carritoRequest = {
    idProducto: 0,
    idCarrito: 0,
    Precio: 0,
    Cantidad: 0,
  };
  cantidadProducto: number = 1;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getListaProductos(1, 8);
  }

  getListaProductos(page: number, pageSize: number): void {
    const url = `http://localhost:5230/ListaProducto?page=${page}&pageSize=${pageSize}`;

    const token = localStorage.getItem('token'); // Recupera el token almacenado en el almacenamiento local
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any[]>(url, { headers })
    .subscribe(
      (data: any[]) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad carritoProductos
        console.log(data);
        this.listaProductos = data;
        // Manejar cantidades individuales
        this.listaProductos = data.map((producto) => ({
          ...producto,
          cantidad: 1, // Inicializa la cantidad en 1
        }));
      },
      (error) => {
        console.error('Error al obtener los productos del carrito', error);
      }
    );
}

eliminarProductoDeLista(index: number) {
  const idProductoAEliminar = index;


  const token = localStorage.getItem('token');
  const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

  this.http.delete(`http://localhost:5230/ListaProducto/${idProductoAEliminar}`, { headers }).subscribe(
    (response: any) => {
      alert('Producto eliminado de la lista');
      this.getListaProductos(1, 8);
    },
    (error) => {
      console.error('Error al eliminar producto de la lista', error);
    }
  );
}

carrito(producto: number, precio:number, stock:number, cantidad:number) {
  if (stock == null || stock == 0){
    alert('De este producto no hay stock');
    return;
  }

  if (cantidad <= 0) {
    alert('La cantidad debe ser mayor que cero.');
    return; 
  } else if (cantidad > stock){
    console.log(cantidad + " " + stock);
    alert('La cantidad debe ser menor o igual al stock');
    return;
  }

  this.carritoRequest = {
    idProducto: producto,
    idCarrito: 0, 
    Precio: precio,
    Cantidad: cantidad, 
  };


 
  const token = localStorage.getItem('token');
  const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

  this.http.post('http://localhost:5230/CarritoProducto', this.carritoRequest, { headers }).subscribe(
    (response: any) => {
      alert('Producto agregado al carrito');
      this.getListaProductos(1, 8); 
    },
    (error) => {
      console.error('Error al agregar producto al carrito', error);
    }
  );
}

}
