import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductCompont } from './product.component';

describe('ProductComponent', () => {
  let component: ProductCompont;
  let fixture: ComponentFixture<ProductCompont>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ProductCompont]
    });
    fixture = TestBed.createComponent(ProductCompont);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
