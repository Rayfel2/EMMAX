import { Component, OnInit } from '@angular/core';
import { Apollo } from 'apollo-angular';
import gql from 'graphql-tag';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-historial',
  templateUrl: './historial.component.html',
  styleUrls: ['./historial.component.css']
})
export class HistorialComponent implements OnInit {
  data: any;
  loading: boolean = true;

  constructor(private apollo: Apollo, private httpClient: HttpClient) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.httpClient.get<number>('http://localhost:5230/ID', { headers }).subscribe(
      (userId: number) => {
        this.fetchUserData(userId);
      },
      (error) => {
        console.error('Error al obtener el ID del usuario', error);
      }
    );
  }

  fetchUserData(userId: number) {
    this.apollo
      .watchQuery({
        query: gql`
          query GetUser($userId: Int!) {
            usuario(userId: $userId) {
              nombre
              apellido
              nombreUsuario
              carritos {
                recibos {
                  oMetodoPago {
                    tipoMetodo
                  }
                  subtotal
                  impuestos
                }
                carritoProductos {
                  oProducto {
                    nombre
                    descripcion
                    oCategorium {
                      nombre
                    }
                    precio
                  }
                }
              }
            }
          }
        `,
        variables: {
          userId: userId
        },
      })
      .valueChanges.subscribe((result) => {
        this.data = result.data;
        this.loading = result.loading;
        this.loading = false;
        console.log(this.data);
      });
  }
}
