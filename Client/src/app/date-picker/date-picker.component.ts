import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatCalendarCellCssClasses } from '@angular/material';
import { Moment } from 'moment';
import * as moment from 'moment';
import { MeniForCalendar } from '../_models/meniForCalendar';
import { MeniService } from '../_services/meni.service';
import { Subject } from 'rxjs';
import { OrderService } from '../_services/order.service';

@Component({
    selector: 'app-date-picker',
    templateUrl: './date-picker.component.html',
    styleUrls: ['./date-picker.component.less']
})
export class DatePickerComponent implements OnInit {
    date: FormControl;
    datesWithMenue: Array<MeniForCalendar>;
    menusWihtOrders: number[];
    @Input() initDate: Moment;
    @Input() refresh: Subject<boolean>;
    @Output() dateChange: EventEmitter<any> = new EventEmitter();

    constructor(private meniService: MeniService, private orderService: OrderService) { }

    ngOnInit(): void {
        this.date = new FormControl(this.initDate.toDate());
        if (this.refresh) {
            this.refresh.subscribe(() => {
                this.setDatesWithDatas();
            });
        }
        this.setDatesWithDatas();
    }

    onDateChange(date) {
        this.dateChange.emit(date);
    }

    setDatesWithDatas = () => {
        this.meniService.getAllMenus().subscribe(response => {
            this.datesWithMenue = (<any[]>response.body).map(item => { return { meniId: item.meniId, datum: new Date(item.datum), canOrder: item.canOrder } });
        });
        this.orderService.getAllForUser().subscribe(response => {
            this.menusWihtOrders = (<number[]>response.body);
        });
    }

    dateClass = (d: Moment): MatCalendarCellCssClasses => {
        let retClasses = '';
        const date = d.toDate().getTime();
        const menu = this.datesWithMenue ? this.datesWithMenue.find(menue => menue.datum.getTime() === date) : null;
        if (menu) {
            if (this.menusWihtOrders && this.menusWihtOrders.some(o => o == menu.meniId)) {
                retClasses = 'date-with-order';
            }
            else {
                retClasses = 'date-with-menue';
            }
            if (!menu.canOrder) {
                retClasses += ' order-time-expired'
            }
        }

        return retClasses;
    }

}
