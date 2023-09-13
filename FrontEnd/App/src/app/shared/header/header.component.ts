import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/auth/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, OnDestroy {
  userLoginOn: boolean=false;
  constructor(private loginService:LoginService, private router: Router) {}
  logout() {
   this.loginService.logout();
   this.router.navigateByUrl('/inicio');
   // Puedes agregar cualquier redirección o lógica adicional aquí después de cerrar sesión.
 }
 
  ngOnDestroy(): void {
   this.loginService.currentUserData.unsubscribe();
   this.loginService.currentUserLoginOn.unsubscribe();  
 }
 
  ngOnInit(): void {
    this.loginService.currentUserLoginOn.subscribe({
     next:(userLoginOn) =>{
       this.userLoginOn= userLoginOn;
     }
   });
 
  }
}
