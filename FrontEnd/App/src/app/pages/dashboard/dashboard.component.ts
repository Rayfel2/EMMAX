import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  category: any[] = [];
  recent: any[] = [];
  popular: any[] = [];
  page: number = 1;
  pageSize: number = 4;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute) {
  }

  // Mueve esta función fuera del constructor y dentro de la clase DashboardComponent
  getData() {
    this.httpClient.get(`http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&categoryProduct=${true}`)
      .subscribe((data: any) => {
        console.log(data);
        this.category = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });

      this.httpClient.get(`http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&recentProduct=${true}`)
      .subscribe((data: any) => {
        console.log(data);
        this.recent = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });

      this.httpClient.get(`http://localhost:5230/Producto?page=${this.page}&pageSize=${this.pageSize}&reviewProduct=${true}`)
      .subscribe((data: any) => {
        console.log(data);
        this.popular = data;
      }, (error) => {
        console.error('Error al obtener datos de la API', error);
      });
  }

  ngOnInit(): void {
    // Llama a la función getData desde ngOnInit
    this.getData();
  }
}
