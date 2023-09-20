import { Injectable } from '@angular/core';
import { DataService } from '../_services/data.service';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BookcaseService {

    constructor(private dataService: DataService) { }

    getAllBooks(): Observable<HttpResponse<Object>> {
        return this.dataService.getAll('/Book/GetAll');
    }
}
