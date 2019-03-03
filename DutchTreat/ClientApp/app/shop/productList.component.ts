import { Component, OnInit } from "@angular/core";
import { DataService } from '../shared/dataService';
import { IProduct } from '../shared/product';

@Component({
    selector: "product-list",
    templateUrl: "productList.component.html",
    styleUrls: [ "productList.component.css" ]
})
export class ProductList implements OnInit {

    constructor(private data: DataService) {
    }

    public products: IProduct[] = [];

    ngOnInit(): void {
        this.data.loadProducts()
            .subscribe(success => {
                if (success) {
                    this.products = this.data.products;
                }
            });
    }

    addProduct(product: IProduct) {
        this.data.addToOrder(product);
    }

}