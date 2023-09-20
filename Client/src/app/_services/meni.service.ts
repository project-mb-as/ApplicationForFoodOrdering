import { Injectable } from '@angular/core';
import { DataService } from '../_services/data.service';
import { HttpParams } from '@angular/common/http';
import { Moment } from 'moment';
import { Meni } from '../_models/meni';

@Injectable({
  providedIn: 'root'
})
export class MeniService {

  constructor(private dataService: DataService) { }

  getMenu(datum: Moment) {
    let params = new HttpParams().set('date', datum.format('YYYY-MM-DD'));
    return this.dataService.get('Meni/GetMeni', params);
  }

  getAllMenus() {
    return this.dataService.getAll('Meni/GetAllMenis');
  }

  createMenu(menu: Meni) {
    return this.dataService.post('meni/CreateOrUpdate', menu);
  }

}
