import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material';

@Component({
  selector: 'app-warning-bar',
    templateUrl: './warning-bar.component.html',
    styleUrls: ['./warning-bar.component.less']
})
export class WarningBarComponent {

  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) { }

}
