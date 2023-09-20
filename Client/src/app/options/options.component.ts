import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services';
import { Options } from '../_models/options';
import { OrderLocationOptions, OrderTimeOptions } from '../globas';
import { BarService } from '../_services/bar.service';

@Component({
  selector: 'app-options',
  templateUrl: './options.component.html',
  styleUrls: ['./options.component.less']
})
export class OptionsComponent implements OnInit {
  options: Options;
  loading: boolean;

  orderLocationOptions: string[];
  orderTimeOptions: string[];

  constructor(private userService: UserService, private barService: BarService) { }

  ngOnInit(): void {
    this.orderLocationOptions = OrderLocationOptions;
    this.orderTimeOptions = OrderTimeOptions;
    this.userService.getOptions().subscribe(o => {
      this.options = o;
    })
  }

  optionsChanged(): void {
    this.loading = true;
    this.userService.setOptions(this.options).subscribe(o => {
      this.barService.showInfo("Uspješno ste ažurirali opcije.");
      this.loading = false;
    }, error => {
      this.loading = false;
      this.barService.showError("Dogodila se greška! \nDetalji:" + error);
    });
  }

}
