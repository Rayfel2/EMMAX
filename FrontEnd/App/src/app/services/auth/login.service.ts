import { Injectable } from '@angular/core';
import { LoginRequest } from './loginRequest';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, catchError, BehaviorSubject, tap } from 'rxjs';
import { User } from './user';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
// Falta el session storage para mantener sesion
  currentUserLoginOn: BehaviorSubject <boolean> = new BehaviorSubject<boolean>(false);
  currentUserData: BehaviorSubject<User> = new BehaviorSubject<User>({id:0, email:''});

  login(credential:LoginRequest): Observable<User>{  //
   return this.http.get<User>('././assets/data.json').pipe(
    tap ( (userData: User) =>{
      this.currentUserLoginOn.next(true);
      this.currentUserData.next(userData);
    }),
    catchError(this.handleError)
   )
  }

  logout() {
    // Aquí puedes realizar cualquier limpieza necesaria al cerrar sesión.
    // Por ejemplo, puedes restablecer currentUserLoginOn y currentUserData.
    this.currentUserLoginOn.next(false);
    this.currentUserData.next({ id: 0, email: '' });

  }

  constructor(private http:HttpClient) { }
  // Manejador de error
  private handleError (error: HttpErrorResponse){
    if(error.status===0){
      console.error ('se ha producido un error', error.error)
    }else {
      console.error('backend retorno el codigo de estado', error.status, error.error)
    }
    return throwError (()=> new Error('Por favor intente nuevamente'))
  } //

  get userData():Observable<User>{
    return this.currentUserData.asObservable();
  }

  get userLoginOn():Observable<Boolean>{
    return this.currentUserLoginOn.asObservable();
  }
}
