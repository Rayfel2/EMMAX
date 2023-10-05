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
}
  ngOnInit(): void {
   
  }
}
