import { Component, OnInit, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginService } from 'src/app/services/auth/login.service';
import { FormBuilder, Validators } from '@angular/forms';
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

  loginForm=this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
      })

      registerForm=this.fb.group({
        nombre: ['', [Validators.required]],
        apellido: ['', [Validators.required]],
        direccion: ['', [Validators.required]],
        telefono: ['',[Validators.required]],
        correo: ['', [Validators.required, Validators.email]],
        nombreUsuario: ['', [Validators.required]],
        fechaNacimiento: ['', [Validators.required]],
        contraseña: ['', [Validators.required]],
          })
    

      loginError: string = "";

  constructor(private router: Router, private elementRef: ElementRef, private http: HttpClient, private loginService: LoginService, private fb: FormBuilder) { }
  
  get email(){
    return this.loginForm.controls.email;
  }
  get correo(){
    return this.registerForm.controls.correo;
  }

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
    this.registerForm.reset();
    this.loginForm.reset();
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
    this.registerForm.reset();
    this.loginForm.reset();
  }
  // Función para realizar el inicio de sesión
login() {
  if (this.loginForm.valid) {
    // Realizar la solicitud HTTP solo si el formulario es válido
    this.http.post('http://localhost:5230/login', this.loginRequest).subscribe(
      (response: any) => {
        // Manejar la respuesta del servidor, por ejemplo, guardar el token en el almacenamiento local.
        console.log('Login exitoso');
        localStorage.setItem('token', response.token);
        console.log(response.token);
        this.loginService.setUserLoginOn(true);
        this.registerForm.reset();
        this.loginForm.reset();
        this.router.navigateByUrl('/inicio');
        // Redireccionar a la página principal o realizar otras acciones necesarias.
      },
      (error) => {
        console.log(this.loginRequest);
        console.error('Error al obtener datos de la API', error);
      }
    );
  }
}

// Función para realizar el registro
registrar() {
  if (this.registerForm.valid) {
    // Realizar la solicitud HTTP solo si el formulario es válido
    this.http.post('http://localhost:5230/Registrar', this.registroRequest).subscribe(
      (response: any) => {
       alert('Registro exitoso');
       this.registerForm.reset();
       this.loginForm.reset();
       this.iniciarSesion();
      },
      (error) => {
        // El registro se realizó con éxito (estado 200)
        console.error('Error al obtener datos de la API', error);
      }
    );
  }
}
}
