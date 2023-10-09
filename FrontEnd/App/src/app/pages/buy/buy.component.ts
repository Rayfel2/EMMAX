import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-buy',
  templateUrl: './buy.component.html',
  styleUrls: ['./buy.component.css']
})
export class BuyComponent implements OnInit {
  reciboData = {
    IdMetodoPago: null,
    IdCarrito: 0,
    Subtotal: 0,
    Impuestos: 0.18,
    Campo: ''
  };
  metodosDePago: any[] = [];
  carritoProductos: any[] = [];
  mostrarDetalles = false;
  total: number = 0;

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.getMetodosDePago();
    this.getCarritoProductos(1, 8);
  }

  calcularTotal(): void {
    let subtotal = 0;
    for (const producto of this.carritoProductos) {
      subtotal += producto.cantidad * producto.producto.precio;
    }
    this.reciboData.Subtotal = subtotal;
    this.total = subtotal + subtotal * this.reciboData.Impuestos;
    console.log(this.total + " " + subtotal)
  }

  getMetodosDePago(): void {
    // URL de la API para obtener los métodos de pago
    const apiUrl = 'http://localhost:5230/Metodo'; // Actualiza la URL

    // Obtiene el token del almacenamiento local (asume que ya está autenticado)
    const token = localStorage.getItem('token');

    // Crea un encabezado con el token de autorización
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    // Realiza la solicitud HTTP para obtener los métodos de pago
    this.http.get<any[]>(apiUrl, { headers }).subscribe(
      (data: any[]) => {
        // Almacena las opciones de método de pago en la variable metodosDePago
        console.log(data);
        this.metodosDePago = data;
      },
      (error) => {
        console.error('Error al obtener los métodos de pago', error);
      }
    );
  }


  get formattedImpuestos(): string {
    return (this.reciboData.Impuestos * 100).toFixed(0) + '%';
  }
  
  set formattedImpuestos(value: string) {
    // Convierte el valor del campo de texto a un número y almacénalo en reciboData.Impuestos
    this.reciboData.Impuestos = parseFloat(value) / 100;
  }

  onSubmit(): void {
    // Obtén el token almacenado en localStorage
    const token = localStorage.getItem('token');
    if (!token) {
      // Manejar el caso en el que no haya un token
      console.error('No se encontró un token en localStorage');
      return;
    }

    // Configura las cabeceras con el token
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    
    // Realiza la solicitud POST al controlador
    this.http.post('http://localhost:5230/Comprar', this.reciboData, { headers }).subscribe(
      (response: any) => {
        alert ('Compra hecha con exito');
        this.router.navigateByUrl('/inicio');

      },
      (error) => {
        alert(error.error);
        console.error('Error:', error);
        
      }
    );
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
        this.calcularTotal();
      },
      (error) => {
        console.error('Error al obtener los productos del carrito', error);
      }
    );
  }
}
