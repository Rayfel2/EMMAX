import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  usuario: any = {}; // Cambia el tipo de dato a objeto vacío

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsuario(); // Recuperar un solo usuario
  }

  getUsuario(): void {
    const url = `http://localhost:5230/Usuario`;

    const token = localStorage.getItem('token'); // Recupera el token almacenado en el almacenamiento local
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get<any>(url, { headers }) // Cambia el tipo de dato a 'any'
    .subscribe(
      (data: any) => {
        // Maneja los datos recibidos aquí, por ejemplo, asignándolos a la propiedad 'usuario'
        console.log(data);
        this.usuario = data;
      },
      (error) => {
        console.error('Error al obtener el usuario', error);
      }
    );
  }
}
