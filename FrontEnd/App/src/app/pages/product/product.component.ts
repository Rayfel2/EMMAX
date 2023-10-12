import { Component, OnInit, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductCompont implements OnInit {
  productId: any; // Variable para almacenar el ID del producto
  productDetails: any; // Variable para almacenar los detalles del producto
  productReviews: any[] = []; 

  
  carritoRequest = {
    idProducto: 0,
    idCarrito: 0,
    Precio: 0,
    Cantidad: 0,
  };
  listaRequest = {
    idListaProducto: 0,
    idProducto: 0,
  };

  comentarioRequest = {
    IdUsuario:0,
    IdProducto: 0, 
    ValorReseña:0,
    Comentario: "",
  };
  
  cantidadProducto: number = 1; // Valor predeterminado inicial



  constructor(private route: ActivatedRoute, private httpClient: HttpClient) { }

  ngOnInit() {
    // Obtener el parámetro 'idProducto' de la URL
    this.route.params.subscribe(params => {
      this.productId = +params['idProducto']; // Convertir el ID a número

      // Llamar al servicio para obtener los detalles del producto por su ID
      this.getProductDetails();
      this.getProductReviews();
    });
  }

  getProductDetails() {
    const productApi = `http://localhost:5230/Producto/${this.productId}`;

    this.httpClient.get(productApi)
      .subscribe((data: any) => {
        // Manejar los datos recibidos del producto
        this.productDetails = data;
        console.log(data);
      }, (error) => {
        console.error('Error al obtener detalles del producto', error);
      });
  }

  getProductReviews() {
    const reviewApi = `http://localhost:5230/reseña/${this.productId}`;


    this.httpClient.get(reviewApi)
      .subscribe((data: any) => {
        // Manejar los datos recibidos de las reseñas
        this.productReviews = data;
        console.log(data);
      }, (error) => {
        console.error('Error al obtener reseñas del producto', error);
      });
  }

  getStarsArray(rating: number): number[] {
    // Crea un array con 'rating' elementos, donde 'rating' es el valor de la reseña.
    return Array.from({ length: rating });
  }
  
  carrito() {
    
    if (this.productDetails.stock == null || this.productDetails.stock == 0){
      alert('De este producto no hay stock');
      return;
    }
  
    if (this.cantidadProducto <= 0) {
      alert('La cantidad debe ser mayor que cero.');
      return; 
    } else if (this.cantidadProducto > this.productDetails.stock){
      alert('La cantidad debe ser menor o igual al stock');
      return;
    } 
  
    // Define el objeto carritoRequest con los datos necesarios
    this.carritoRequest = {
      idProducto: this.productId,
      idCarrito: 0, 
      Precio: this.productDetails.precio,
      Cantidad: this.cantidadProducto, 
    };
  
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.httpClient.post('http://localhost:5230/CarritoProducto', this.carritoRequest, { headers }).subscribe(
      (response: any) => {
        alert('Producto agregado al carrito');
      },
      (error) => {
        console.error('Error al agregar producto al carrito', error);
      }
    );
  }

  lista() {
    this.listaRequest = {
      idProducto: this.productId,
      idListaProducto: 0, 
    };
   
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.httpClient.post('http://localhost:5230/ListaProducto', this.listaRequest, { headers }).subscribe(
      (response: any) => {
        alert('Producto agregado a la lista');
      },
      (error) => {
        console.error('Error al agregar producto a la lista', error);
      }
    );
  }


  valorar(valor: number) {
    this.comentarioRequest.ValorReseña = valor;
    this.comentarioRequest.IdProducto = this.productId;
  }
  

  comentario() {
  
    if (this.comentarioRequest.ValorReseña == 0){
      alert ('Debes poner una calificacion')
      return;
    }
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.httpClient.post('http://localhost:5230/Comentarios', this.comentarioRequest, { headers }).subscribe(
      (response: any) => {
        alert('Has comentado con exito');
        this.getProductReviews();
      },
      (error) => {
        console.error('Error al agregar comentario', error);
      }
    );
  }
}
