import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductCompont implements OnInit {
  productId: any; // Variable para almacenar el ID del producto
  productDetails: any; // Variable para almacenar los detalles del producto
  productReviews: any[] = []; 

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
  


}
