import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private userLoginOnSubject = new BehaviorSubject<boolean>(false);
  userLoginOn$ = this.userLoginOnSubject.asObservable();

  setUserLoginOn(value: boolean) {
    this.userLoginOnSubject.next(value);
  }
}