import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, FormArray } from '@angular/forms';
import { MeniService } from '../_services/meni.service';
import { Prilog } from '../_models/prilog';
import { BarService } from '../_services/bar.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Hrana } from '../_models/hrana';
import { error } from '@angular/compiler/src/util';
import { FoodService } from '../_services/food.service';

@Component({
  selector: 'app-create-food-dialog',
  templateUrl: './create-food-dialog.component.html',
  styleUrls: ['./create-food-dialog.component.less']
})
export class CreateFoodDialogComponent implements OnInit {
  createFoodForm: FormGroup;
  submitted: boolean = false;
  sideDishes: Array<Prilog>;
  sideDishesMap: any[];
  mapVariantToVariantId = [];
  isEditMode = false;


    constructor(private formBuilder: FormBuilder, private barService: BarService,
        public dialogRef: MatDialogRef<CreateFoodDialogComponent>, private foodService: FoodService,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    this.mapVariantToVariantId['all'] = 0;
    this.mapVariantToVariantId['first'] = 1;
    this.mapVariantToVariantId['second'] = 2;

  }

  ngOnInit(): void {
    if (this.data.food) {
      this.isEditMode = true;
    }
    this.sideDishes = this.data.sideDishes;
    this.sideDishesMap = this.data.sideDishesMap;
    if (this.isEditMode) {
      this.createFoodForm = this.formBuilder.group({
        name: [this.data.food.naziv, Validators.required],
        permanent: [this.data.food.stalna],
        sideDishes: this.formBuilder.array(
          this.sideDishes.map(o => {
            const variant = this.getVariantForSideDish(o.prilogId);
            return this.formBuilder.group({
              selected: [variant != null, Validators.required],
              variant: ['' + (variant === null ? 0 : variant), Validators.required]
            });
          }))
      });
    }
    else {
      this.createFoodForm = this.formBuilder.group({
        name: [this.data.name, Validators.required],
        permanent: [this.data.permanent],
        sideDishes: this.formBuilder.array(
          this.sideDishes.map(o => this.formBuilder.group({
            selected: [false, Validators.required],
            variant: ['0', Validators.required]
          })
          ))
      });
    }
  }

  private getVariantForSideDish = (sideDishId: number): string => {
    let ret = null;
    for (let sideDish of this.data.food.prilozi) {
      if (sideDish.prilogId === sideDishId) {
        ret = sideDish.varijanta;
        break;
      }
    }
    return ret;
  }

  get f() {
    return this.createFoodForm.controls;
  }

  renderHelper = () => { };

  createSideDish(newSideDishInput) {
    if (newSideDishInput.value === '') {
      this.barService.showError('Morate unijeti naziv priloga.');
    }
    else {
        this.foodService.createSideDish({ naziv: newSideDishInput.value }).subscribe((sideDishId: number) => {
        (<FormArray>this.createFoodForm.get('sideDishes')).controls.push(this.formBuilder.group({
          selected: [true, Validators.required],
          variant: ['0', Validators.required]
        }));
        this.sideDishes.push(new Prilog({ naziv: newSideDishInput.value, prilogId: sideDishId }));
        this.sideDishesMap[sideDishId] = newSideDishInput.value;
        this.barService.showInfo("Prilog je uspješno kreiran.");
        newSideDishInput.value = '';
      })
    }

  }

  closeDialog = () => {
    this.dialogRef.close();
  }

  onSubmit() {
    this.submitted = true;

    if (!this.createFoodForm.invalid) {
      const formValue = this.createFoodForm.value;
      
      const food = {
        hranaId: this.data.food ? this.data.food.hranaId : 0,
        naziv: formValue.name,
        stalna: formValue.permanent,
        prilozi: this.getPrilozi((<FormArray>this.f.sideDishes).controls)
      };
      //this.loading = true;

        this.foodService.createFood(food).subscribe(() => {
        this.barService.showInfo("Hrana je uspješno snimljena.");
        this.dialogRef.close();
      }, (error) => {
        this.barService.showError("Dogidila se greška.");
      });

    }
  }

  getPrilozi(sideDishesFG: any[]): any[] {
    let ret = [];
    sideDishesFG.forEach((item, index) => {
      if (item.controls.selected.value) {
        ret.push({ varijanta: item.controls.variant.value, prilogId: this.data.sideDishes[index].prilogId });
      }
    });

    return ret;
  }


}
