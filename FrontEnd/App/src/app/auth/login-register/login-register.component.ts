import { Component, OnInit, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginService } from 'src/app/services/auth/login.service';
import { Router } from '@angular/router';

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

  mostrarFormularioLogin = true;
  mostrarFormularioRegistro = false;
  loginRequest = {
    Email: '',
    Contraseña: ''
  };
  registroRequest = {
    Nombre: '',
    Apellido: '',
    Dirección: '',
    Teléfono: '',
    Email: '',
    NombreUsuario: '',
    FechaNacimiento: '',
    Contraseña: ''
  };

  constructor(private elementRef: ElementRef, private http: HttpClient, private loginService: LoginService, private router: Router) { }
  
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
  // Función para realizar el inicio de sesión
  login() {
    // Realizar la solicitud HTTP para iniciar sesión utilizando this.loginRequest
    this.http.post('http://localhost:5230/login', this.loginRequest).subscribe(
      (response: any) => {
        // Manejar la respuesta del servidor, por ejemplo, guardar el token en el almacenamiento local.
        console.log('Login exitoso');
        localStorage.setItem('token', response.token);
        this.loginService.setUserLoginOn(true);
        this.router.navigateByUrl('/inicio');
        // Redireccionar a la página principal o realizar otras acciones necesarias.
      },
      (error) => {
        console.log(this.loginRequest);
        console.error('Error al obtener datos de la API', error);
      }
    );
  }

  // Función para realizar el registro
  registrar() {
    // Realizar la solicitud HTTP para registrarse utilizando this.registroRequest
    this.http.post('http://localhost:5230/Registrar', this.registroRequest).subscribe(
      (response: any) => {
        console.log('Registro exitoso');
      },
      (error) => {
        
          // El registro se realizó con éxito (estado 200)
        console.error('Error al obtener datos de la API', error);
      }
    );
  }
}
