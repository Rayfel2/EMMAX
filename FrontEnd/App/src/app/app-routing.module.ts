import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CarComponent } from './pages/car/car.component';
import { ContactComponent } from './pages/contact/contact.component';
import { AboutComponent } from './pages/about/about.component';
import { ListComponent } from './pages/list/list.component';
import { ShopComponent } from './pages/shop/shop.component';
import { LoginRegisterComponent } from './auth/login-register/login-register.component';
import { ProductCompont } from './pages/product/product.component'; 


const routes: Routes = [
  {path:'', redirectTo: '/inicio', pathMatch:'full'},
  {path:'inicio',component:DashboardComponent},
  {path:'iniciar-sesion', component:LoginRegisterComponent},
  {path:'perfil', component:ProfileComponent},
  {path:'registro', component:RegisterComponent},
  {path:'carrito', component:CarComponent},
  {path:'contactos', component:ContactComponent},
  {path: 'quienes-somos', component:AboutComponent},
  {path:'deseos', component:ListComponent},
  {path: 'tienda', component:ShopComponent},
  {path: 'product/:idProducto', component:ProductCompont},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
