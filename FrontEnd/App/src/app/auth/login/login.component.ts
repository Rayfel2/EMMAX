import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent { 
  loginForm=this.fb.group({
email: ['email.com', [Validators.required, Validators.email]],
password: ['', [Validators.required]],
  })
  constructor(private fb: FormBuilder) { }

  login(){
    if (this.loginForm.valid){
      console.log("aqui se llamaria al servicio de login");
    } else {
      alert ("error al ingresar los datos");
    }

  }
}
