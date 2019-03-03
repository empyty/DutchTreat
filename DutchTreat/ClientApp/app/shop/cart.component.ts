﻿import { DataService } from '../shared/dataService';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: "the-cart",
    templateUrl: "cart.component.html",
    styleUrls: []
})
export class Cart {
    constructor(private data: DataService,
        private router: Router) { }

    public onCheckout() {
        if (this.data.loginRequired) {
            this.router.navigate(["login"]);
        } else {
            this.router.navigate(["checkout"]);
        }
    }
}