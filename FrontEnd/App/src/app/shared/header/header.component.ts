import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/auth/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  userLoginOn: boolean = false;

  constructor(private router: Router, private loginService: LoginService) {}

  logout() {
    localStorage.removeItem('token');
    this.loginService.setUserLoginOn(false);
    this.router.navigateByUrl('/inicio');
  }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {

   this.loginService.userLoginOn$.subscribe((userLoginOn) => {
      this.userLoginOn = userLoginOn; // Actualiza userLoginOn cuando cambie el estado de inicio de sesión
  }
)
const token = localStorage.getItem('token');
if (token) {
  // Si hay un token, establece userLoginOn en true
  this.userLoginOn = true;
}
};

}
