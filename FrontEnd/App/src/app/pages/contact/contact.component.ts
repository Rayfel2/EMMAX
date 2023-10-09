import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  usuario: string = '';
  gmail: string = '';
  comentarios: string = '';

  constructor(private http: HttpClient) {}

  enviarCorreo() {
    const formData = {
      usuario: this.usuario,
      gmail: this.gmail,
      comentarios: this.comentarios
    };

    this.http.post('http://localhost:4200/enviar-correo', formData)
      .subscribe((response: any) => {
        console.log('Correo enviado con éxito:', response);
        
      }, (error) => {
        console.error('Error al enviar el correo:', error);
     
      });
  }
}
