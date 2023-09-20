import { Injectable } from '@angular/core';
import { DataService } from '../_services/data.service';
import { HttpParams } from '@angular/common/http';
import { Meni } from '../_models/meni';


@Injectable({
    providedIn: 'root'
})
export class OrderService {

    constructor(private dataService: DataService) { }

    get(menuId: number) {
        let params = new HttpParams().set('menuId', menuId.toString());
        return this.dataService.get('Order/Get', params);
    }

    getAll(menuId: number) {
        let params = new HttpParams().set('menuId', menuId.toString());
        return this.dataService.get('Order/GetAll', params);
    }

    getAllForUser() {
        return this.dataService.getAll('Order/GetAllForUser');
    }

    create(data) {
        return this.dataService.post('Order/CreateOrUpdate', data);
    }

    delete(orderId) {
        return this.dataService.post('Order/Delete', orderId);
    }

}
