import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/auth/login.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit, OnDestroy {
 userLoginOn: boolean=false;
 constructor(private loginService:LoginService) {}
 logout() {
  this.loginService.logout();
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
  })
 }
}
