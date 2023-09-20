import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { MeniService } from '../_services/meni.service';
import { Meni } from '../_models/meni';
import { OrderService } from '../_services/order.service';
import { forkJoin } from 'rxjs';
import { OrderLocationOptions, OrderTimeOptions, ROLES } from '../globas';
import { Hrana } from '../_models/hrana';
import { Order } from '../_models/order';
import { FoodService } from '../_services/food.service';

@Component({
    selector: 'app-orders-report',
    templateUrl: './orders-report.component.html',
    styleUrls: ['./orders-report.component.less']
})
export class OrdersReportComponent implements OnInit {
    today: moment.Moment;
    menu: Meni;
    sideDishesMap = [];
    foodMap = [];
    allFood = [];
    orders = [];
    ordersForDisplay = [];
    foodSummary = [];
    foodSummaryForTime = [];
    constructor(private meniService: MeniService, private orderService: OrderService, private foodService: FoodService) { }

    ngOnInit(): void {
        this.today = moment();
        forkJoin({
            food: this.foodService.getAllFood(),
            sideDishes: this.foodService.getAllSideDishes(),
            menu: this.meniService.getMenu(this.today),
        }).subscribe((data) => {
            this.menu = new Meni(
                {
                    menuId: (<any>data.menu.body).menuId,
                    date: (<any>data.menu.body).date,
                    food: (<any>data.menu.body).food,
                    canOrder: (<any>data.menu.body).canOrder
                });
            this.getOrders();

            this.allFood = data.food;
            this.allFood.forEach(o => {
                this.foodMap[o.hranaId] = o.naziv;
            });

            data.sideDishes.forEach(o => {
                this.sideDishesMap[o.prilogId] = o.naziv;
            });
        });
    }

    onDateChange(event) {
        this.meniService.getMenu(event.value)
            .subscribe((data: any) => {
                this.menu = new Meni({
                    menuId: data.body.menuId,
                    date: data.body.date,
                    food: data.body.food,
                    canOrder: data.body.canOrder
                });
                this.getOrders();
            });
    }

    getOrders() {
        if (this.menu.menuId != 0) {
            this.orderService.getAll(this.menu.menuId).subscribe(data => {
                this.orders = data.body as any[];
                this.setFoodSummary();
                this.setOrdersForDisplay();
            })
        }
        else {
            this.orders = [];
        }
    }

    setFoodSummary() {
        this.foodSummary = [];
        this.foodSummaryForTime = [];
        for (let i = 0; i < this.allFood.length; i++) {
            let numberOfOrdersForFood = this.orders.filter(o => o.foodId == this.allFood[i].hranaId).length;
            if (numberOfOrdersForFood > 0) {
                this.foodSummary.push({ name: this.allFood[i].naziv, numberOfOrders: numberOfOrdersForFood });
            }
        }
        this.foodSummary.sort((a, b) => { return b.numberOfOrders - a.numberOfOrders });

        OrderTimeOptions.forEach((timeName, index) => {
            this.foodSummaryForTime[index] = { time: timeName, foodSummary: [] };
            for (let i = 0; i < this.allFood.length; i++) {
                let numberOfOrdersForFood = this.orders.filter(o => o.foodId == this.allFood[i].hranaId && o.timeId == index).length;
                if (numberOfOrdersForFood > 0) {
                    this.foodSummaryForTime[index].foodSummary.push({ name: this.allFood[i].naziv, numberOfOrders: numberOfOrdersForFood });
                }
            }
            this.foodSummaryForTime[index].foodSummary.sort((a, b) => { return b.numberOfOrders - a.numberOfOrders });
        });
    }

    setOrdersForDisplay() {
        this.ordersForDisplay = [];
        OrderTimeOptions.forEach((timeName, timeIndex) => {
            OrderLocationOptions.forEach((locationName, locationIndex) => {
                this.ordersForDisplay.push({
                    timeLocation: timeName + " - " + locationName,
                    orders: this.getOrdersForDisplay(this.orders.filter(o => o.locationId == locationIndex && o.timeId == timeIndex))
                })
            })
        })
    }

    getOrdersForDisplay(orders: any[]): any[] {
        let ordersForDisplay = [];
        for (let i = 0; i < orders.length; i++) {
            let order = orders[i];
            ordersForDisplay.push(
                {
                    foodName: this.foodMap[order.foodId],
                    user: order.user.email,
                    sideDishes: this.getSideDishes(order.sideDishes)
                })
        }
        ordersForDisplay = ordersForDisplay.sort((a, b) => {
            var nameA = a.foodName.toUpperCase(); 
            var nameB = b.foodName.toUpperCase();
            if (nameA < nameB) {
                return -1;
            }
            if (nameA > nameB) {
                return 1;
            }
            return 0;
        });
        return ordersForDisplay;
    }

    getSideDishes = (sideDishes: any[]): string => {
        let ret = "";
        sideDishes.forEach(sideDish => {
            ret += this.sideDishesMap[sideDish] + ", ";
        })
        ret = ret.substring(0, ret.length - 2);
        return ret;
    }

}
