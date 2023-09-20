import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA } from '@angular/material';

@Component({
  selector: 'app-error-bar',
  templateUrl: './error-bar.component.html',
  styleUrls: ['./error-bar.component.less']
})
export class ErrorBarComponent {

  constructor(@Inject(MAT_SNACK_BAR_DATA) public data: any) { }

}
