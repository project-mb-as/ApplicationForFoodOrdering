import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { MeniService } from '../_services/meni.service';
import { HttpResponse } from '@angular/common/http';
import { Hrana } from '../_models/hrana';
import { Prilog } from '../_models/prilog';
import { MatDialog } from '@angular/material';
import { CreateFoodDialogComponent } from '../create-food-dialog/create-food-dialog.component';
import { forkJoin, Subject } from 'rxjs';
import { Meni } from '../_models/meni';
import { BarService } from '../_services/bar.service';
import { AuthenticationService, UserService } from '../_services';
import { OrderLocationOptions, OrderTimeOptions, ROLES } from '../globas';
import { OrderService } from '../_services/order.service';
import { FoodService } from '../_services/food.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-novi-meni',
  templateUrl: './novi-meni.component.html',
  styleUrls: ['./novi-meni.component.less']
})
export class NoviMeniComponent implements OnInit {
  date: FormControl;
  today: moment.Moment;
  hranaArray: Hrana[];
  stalnaHranaArray: Hrana[];
  filterHrana: string = "";
  filterStalnaHrana: string = "";
  step: number = -1;
  stepStalna: number = -1;
  sideDishesMap: any[] = [];
  sideDishes: Prilog[];
  selectedFood: Hrana;
  foodForMenu: Hrana[] = [];
  menu: Meni;

  adminMode: boolean = false;
  isAdminOrCook: boolean = false;

  imagesUrl: string = environment.imagesUrl;

  // Order
  orderError: { time: boolean, place: boolean };
  orderLocation: number;
  orderTime: number;
  orderLocationOptions: string[];
  orderTimeOptions: string[];
  orderId: number = 0;
  refreshCalendar: Subject<boolean>;

  loading: boolean = false;
  initMenuLoading: boolean = false;
  setOrderLoading: boolean = false;

  constructor(private meniService: MeniService, private dialog: MatDialog, private barService: BarService, private foodService: FoodService,
    private authenticationService: AuthenticationService, private orderService: OrderService, private userService: UserService) { }

  ngOnInit() {
    this.today = moment();
    this.refreshCalendar = new Subject<boolean>();
    this.initFood();
    let user = this.authenticationService.currentUserValue;
    this.isAdminOrCook = (user.roles.indexOf(ROLES.admin) != -1) ||
      (user.roles.indexOf(ROLES.cook) != -1);
    //this.orderError = { time: false, place: false };
    this.orderLocationOptions = OrderLocationOptions;
    this.orderTimeOptions = OrderTimeOptions;

    this.userService.getOptions().subscribe(o => {
      this.orderLocation = o.locationId;
      this.orderTime = o.timeId;
    })

  }

  initFood() {
    this.loading = true;
    forkJoin({
      food: this.foodService.getAllFood(),
      sideDishes: this.foodService.getAllSideDishes(),
      menu: this.meniService.getMenu(this.menu ? moment(this.menu.date) : this.today),
    }).subscribe((data) => {
      this.menu = new Meni({ menuId: (<any>data.menu.body).menuId, date: (<any>data.menu.body).date, food: (<any>data.menu.body).food, canOrder: (<any>data.menu.body).canOrder })
      this.setFood(data.food);
      this.sideDishesMap = [];
      this.sideDishes = [...(data.sideDishes).map(o => new Prilog({ prilogId: o.prilogId, naziv: o.naziv, varijanta: o.varijanta }))];
      data.sideDishes.forEach(o => {
        this.sideDishesMap[o.prilogId] = o.naziv;
      });
      this.loading = false;
    });
  }

  initMenu(date: moment.Moment) {
    this.initMenuLoading = true;
    this.meniService.getMenu(date)
      .subscribe((data: any) => {
        this.menu = new Meni({ menuId: data.body.menuId, date: data.body.date, food: data.body.food, canOrder: data.body.canOrder });
        if (this.adminMode) {
          this.setFoodForMenu();
        } else {
          this.setOrder();
        }
        this.initMenuLoading = false;
      },
        () => this.initMenuLoading = false);
  }

  setFood = (data: Hrana[]) => {
    this.stalnaHranaArray = [];
    this.hranaArray = [];
    data.forEach(dataForHrana => {
      let hrana = new Hrana(dataForHrana);
      if (hrana.stalna) {
        this.stalnaHranaArray.push(hrana);
      } else {
        this.hranaArray.push(hrana);
      }
    });
    this.stalnaHranaArray.sort((a, b) => b.rating - a.rating);
    this.hranaArray.sort((a, b) => b.rating - a.rating);
    if (this.adminMode) {
      this.setFoodForMenu();
    }
    else {
      this.setOrder();
    }
  }

  setFoodForMenu() {
    this.foodForMenu = [];
    this.stalnaHranaArray = this.stalnaHranaArray.map(o => { o.izabrana = false; return o; });
    this.hranaArray = this.hranaArray.map(o => { o.izabrana = false; return o; });
    if (this.menu.food && this.menu.food.length > 0) {
      this.stalnaHranaArray.concat(this.hranaArray).filter(hrana => (this.menu.food.indexOf(hrana.hranaId) != -1)).forEach(hrana => {
        this.foodForMenu.push(hrana);
        hrana.izabrana = true;
      });
    }
    else {
      this.stalnaHranaArray.concat(this.hranaArray).filter(hrana => hrana.stalna).forEach(hrana => {
        this.foodForMenu.push(hrana);
        hrana.izabrana = true;
      });

    }
  }

  setOrder = () => {
    this.stalnaHranaArray = this.stalnaHranaArray.map(o => { o.izabrana = false; return o; });
    this.hranaArray = this.hranaArray.map(o => { o.izabrana = false; return o; });
    this.orderId = 0;
    this.selectedFood = null;
    if (this.menu.menuId) {
      this.setOrderLoading = true;
      this.orderService.get(this.menu.menuId).subscribe(data => {
        var order: any = data.body;
        if (order) {
          this.orderId = order.orderId;
          this.orderLocation = order.locationId;
          this.orderTime = order.timeId;
          var orderdFood = this.stalnaHranaArray.concat(this.hranaArray).find(o => o.hranaId === (order.foodId));
          orderdFood.izabrana = true;
          orderdFood.prilozi.filter(o => order.sideDishes.indexOf(o.prilogId) != -1).map(o => { o.izabran = true; return o; });
          this.selectedFood = orderdFood;
        }
        this.setOrderLoading = false;
      });
    }
  }

  adminModeChaged() {
    if (this.adminMode) {
      this.setFoodForMenu();
    } else {
      this.setOrder();
    }
  }

  izaberiHranu(event, hrana: Hrana) {
    event.stopPropagation();
    if (this.selectedFood && this.selectedFood != hrana) {
      this.selectedFood.izabrana = false;
    }
    hrana.izabrana = !hrana.izabrana;
    if (hrana.izabrana) {
      this.selectedFood = hrana;
    }
    else {
      this.selectedFood = null;
    }
  }

  isInMenu(hrana: Hrana): boolean {
    return (this.adminMode || this.menu.food.some(h => h === hrana.hranaId));
  }

  addFoodToMenu(event, food: Hrana) {
    event.stopPropagation();
    food.izabrana = !food.izabrana;
    if (food.izabrana) {
      this.foodForMenu.push(food);
    } else {
      this.foodForMenu.splice(this.foodForMenu.indexOf(food), 1);
    }
  }

  kreirajHranu(permanent: boolean): void {
    const dialogRef = this.dialog.open(CreateFoodDialogComponent, {
      width: '700px',
      height: '80%',
      disableClose: true,
      data: {
        sideDishesMap: this.sideDishesMap,
        sideDishes: this.sideDishes,
        name: permanent ? this.filterStalnaHrana : this.filterHrana,
        permanent: permanent
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.initFood();
    });
  }

  createMenu(): void {
    this.loading = true;
    this.menu.food = this.hranaArray.concat(this.stalnaHranaArray).filter(h => h.izabrana).map(o => o.hranaId);
    this.meniService.createMenu(this.menu).subscribe(
      (data: number) => {
        this.menu.menuId = data;
        this.barService.showInfo("Uspješno ste snimili meni.");
        this.refreshCalendar.next(true);
        this.loading = false;
      }, error => {
        this.barService.showError(error.message);
        this.loading = false;
      });

  }

  createOrder() {
    if (this.selectedFood) {
      this.loading = true;
      const order = {
        orderId: this.orderId,
        timeId: this.orderTime,
        locationId: this.orderLocation,
        menuId: this.menu.menuId,
        foodId: this.selectedFood.hranaId,
        sideDishes: this.selectedFood.prilozi.filter(o => o.izabran).map(o => o.prilogId)
      }

      this.orderService.create(order).subscribe((res: number) => {
        this.orderId = res;
        this.refreshCalendar.next(true);
        this.barService.showInfo(`Usješno ste naručili "${this.selectedFood.naziv}" na lokaciju "${this.orderLocationOptions[this.orderLocation]}"
                                      u vrijeme "${this.orderTimeOptions[this.orderTime]}".`);
        this.loading = false;
      },
        error => {
          this.barService.showError('Dogorila se greška. Narudžba nije kreirana. Detalji: \n' + error);
          this.loading = false;
        });
    } else {
      this.barService.showError("Niste izabrali hranu!");
    }
  }

  deleteOrder() {
    this.loading = true;
    this.orderService.delete(this.orderId).subscribe(rez => {
      this.barService.showWarning('Obrisali ste narudžbu.');
      this.refreshCalendar.next(true);
      this.setOrder();
      this.loading = false;
    }, error => {
      this.barService.showError('Dogodila se greška. Narudžba nije obrisana.');
      this.loading = false;
    });
  }



  editFood(food: Hrana): void {
    const dialogRef = this.dialog.open(CreateFoodDialogComponent, {
      width: '700px',
      height: '80%',
      disableClose: true,
      data: {
        food: food,
        sideDishesMap: this.sideDishesMap,
        sideDishes: this.sideDishes
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.initFood();
    });
  }


  onDateChange(event) {
    //TODO check if saved, and if not create alert
    this.initMenu(event.value);
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
  onRatingChange(event, foodId) {
    this.foodService.rateFood(foodId, event.rating).subscribe(() => {
      this.barService.showInfo(`Usješno ste ocijenili hranu.`);
    }, error => {
      this.barService.showError('Dogorila se greška. Molimo Vas pokušajte kasnije. Detalji: \n' + error);
    })
  }

  /// Steps region ///
  setStep(index: number) {
    this.step = index;
  }

  nextStep(event) {
    event.stopPropagation();
    this.step++;
  }

  prevStep(event) {
    event.stopPropagation();
    this.step--;
  }

  setStepStalna(index: number) {
    this.stepStalna = index;
  }

  nextStepStalna(event) {
    event.stopPropagation();
    this.stepStalna++;
  }

  prevStepStalna(event) {
    event.stopPropagation();
    this.stepStalna--;
  }

  stopPropagation(event) {
    event.stopPropagation();
  }
  /// end region ///
}
