import { Component, OnDestroy, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/auth/login.service';
import { User } from 'src/app/services/auth/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy{
  userLoginOn:boolean=false;
  userData?:User;
  constructor(private loginService:LoginService) {}
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
   this.loginService.currentUserData.subscribe({
    next:(userData)=>{
      this.userData=userData;
    }
   })
  }
}
