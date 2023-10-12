import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/header/header.component';
import { FooterComponent } from './shared/footer/footer.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { LoginComponent } from './auth/login/login.component';
import { NavComponent } from './shared/nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ProfileComponent } from './pages/profile/profile.component';
import { ContactComponent } from './pages/contact/contact.component';
import { AboutComponent } from './pages/about/about.component';
import { ShopComponent } from './pages/shop/shop.component';
import { CarComponent } from './pages/car/car.component';
import { ListComponent } from './pages/list/list.component';
import { RegisterComponent } from './auth/register/register.component';
import { LoginRegisterComponent } from './auth/login-register/login-register.component';
import { ProductCompont } from './pages/product/product.component';
import { BuyComponent } from './pages/buy/buy.component';
import { HistorialComponent } from './pages/historial/historial.component';
import { GraphQLModule } from './graphql.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    DashboardComponent,
    LoginComponent,
    NavComponent,
    ProfileComponent,
    ContactComponent,
    AboutComponent,
    ShopComponent,
    CarComponent,
    ListComponent,
    RegisterComponent,
    LoginRegisterComponent,
    ProductCompont,
    BuyComponent,
    HistorialComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    GraphQLModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
