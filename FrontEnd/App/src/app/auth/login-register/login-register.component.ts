import { Component, OnInit, ElementRef } from '@angular/core';

@Component({
  selector: 'app-login-register',
  templateUrl: './login-register.component.html',
  styleUrls: ['./login-register.component.css']
})
export class LoginRegisterComponent implements OnInit {

  // Declarando variables
  formulario_login: any;
  formulario_register: any;
  contenedor_login_register: any;
  caja_trasera_login: any;
  caja_trasera_register: any;

  constructor(private elementRef: ElementRef) { }
  
  ngOnInit() {
      // Obtener elementos después de que el componente haya sido renderizado
      this.formulario_login = this.elementRef.nativeElement.querySelector(".formulario__login");
      this.formulario_register = this.elementRef.nativeElement.querySelector(".formulario__register");
      this.contenedor_login_register = this.elementRef.nativeElement.querySelector(".contenedor__login-register");
      this.caja_trasera_login = this.elementRef.nativeElement.querySelector(".caja__trasera-login");
      this.caja_trasera_register = this.elementRef.nativeElement.querySelector(".caja__trasera-register");
  
      // Ejecutar la función anchoPage al inicializar el componente
      this.anchoPage();
  
      // Agregar event listeners si los elementos existen
      const btnIniciarSesion = this.elementRef.nativeElement.querySelector("#btn__iniciar-sesion");
      if (btnIniciarSesion) {
        btnIniciarSesion.addEventListener("click", this.iniciarSesion.bind(this));
      }
  
      const btnRegistrarse = this.elementRef.nativeElement.querySelector("#btn__registrarse");
      if (btnRegistrarse) {
        btnRegistrarse.addEventListener("click", this.register.bind(this));
      }
  
      window.addEventListener("resize", this.anchoPage.bind(this));
  }

  // Función para ajustar el diseño según el ancho de la página
  anchoPage() {
    if (window.innerWidth > 850) {
      this.caja_trasera_register.style.display = "block";
      this.caja_trasera_login.style.display = "block";
    } else {
      this.caja_trasera_register.style.display = "block";
      this.caja_trasera_register.style.opacity = "1";
      this.caja_trasera_login.style.display = "none";
      this.formulario_login.style.display = "block";
      this.contenedor_login_register.style.left = "0px";
      this.formulario_register.style.display = "none";
    }
  }

  // Función para mostrar el formulario de inicio de sesión
  iniciarSesion() {
    if (window.innerWidth > 850) {
      this.formulario_login.style.display = "block";
      this.contenedor_login_register.style.left = "10px";
      this.formulario_register.style.display = "none";
      this.caja_trasera_register.style.opacity = "1";
      this.caja_trasera_login.style.opacity = "0";
    } else {
      this.formulario_login.style.display = "block";
      this.contenedor_login_register.style.left = "0px";
      this.formulario_register.style.display = "none";
      this.caja_trasera_register.style.display = "block";
      this.caja_trasera_login.style.display = "none";
    }
  }

  // Función para mostrar el formulario de registro
  register() {
    if (window.innerWidth > 850) {
      this.formulario_register.style.display = "block";
      this.contenedor_login_register.style.left = "410px";
      this.formulario_login.style.display = "none";
      this.caja_trasera_register.style.opacity = "0";
      this.caja_trasera_login.style.opacity = "1";
    } else {
      this.formulario_register.style.display = "block";
      this.contenedor_login_register.style.left = "0px";
      this.formulario_login.style.display = "none";
      this.caja_trasera_register.style.display = "none";
      this.caja_trasera_login.style.display = "block";
      this.caja_trasera_login.style.opacity = "1";
    }
  }
}
