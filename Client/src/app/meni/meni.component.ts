import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDatepickerIntl, MatDatepickerInputEvent, MatCalendarCellCssClasses } from '@angular/material/datepicker';
import { MeniService } from '../_services/meni.service';
import { Moment } from 'moment';
import * as moment from 'moment';
import { Meni } from '../_models/meni';
import { Hrana } from '../_models/hrana';
import { Prilog } from '../_models/prilog';
import { MeniForCalendar } from '../_models/meniForCalendar';

@Component({
  selector: 'app-meni',
  templateUrl: './meni.component.html',
  styleUrls: ['./meni.component.less']
})
export class MeniComponent implements OnInit {
  step = 0;

  constructor(private meniService: MeniService) { }
  meni: Meni
  hrana: any;
  nextDay: Moment;
  datesWithMenue: Array<MeniForCalendar>;

  ngOnInit() {
    this.nextDay = moment().add(1, 'days');
    this.getHrana(this.nextDay);
  }

  onDateChange(date) {
    this.getHrana(date.value);
  }

  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }

  getHrana(date: Moment) {
    this.meniService.getMenu(date).subscribe(response => {
      //this.meni = new Meni(response);
    });
  }

  onPrilogChange(hrana: Hrana, prilog: Prilog) {
    if (prilog.varijanta != 0) {
      if (prilog.izabran) {
        let izabranaVarijanta = prilog.varijanta;
        hrana.prilozi.forEach(prilogItem => {
          if (prilogItem.varijanta != 0 && prilogItem.varijanta != izabranaVarijanta) {
            prilogItem.omogucen = false;
          }
        });
      } else {
        if (hrana.prilozi.every(prilogItem => { return !prilogItem.izabran || prilogItem.varijanta == 0 })) {
          hrana.prilozi.forEach(prilogItem => {
            prilogItem.omogucen = true;
          });
        }
      }
    }
  }



}
