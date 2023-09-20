import { Hrana } from './hrana';
import { Prilog } from './prilog';
import { Data } from '@angular/router';

export class Meni {
    menuId: number;
    date: Data;
    food: number[];
    canOrder: boolean;
    //narudzba: {
    //  lokacija: number;
    //  vrijeme: number;
    //  hrana: number;
    //  prilozi: number[];
    //  //datum: Date;
    //};



    //constructor(meniResponse: any) {
    //  //TODO remove init narudzba when API return narudzba
    //  //this.narudzba = { lokacija: 1, vrijeme: 1, hrana: 1, prilozi: [] };
    //  //this.hrana = [];
    //  //if (meniResponse && meniResponse.body) {
    //  //  meniResponse.body.hrana.forEach((hranaItem) => {
    //  //    let hrana = new Hrana(hranaItem.hrana);
    //  //    hrana.izabrana = (this.narudzba.hrana == hranaItem.hrana.hranaId);
    //  //    this.hrana.push(hrana)
    //  //  });
    //  //}
    //  //else {
    //  //  return null;
    //  //}
    //}

    public constructor(init?: Partial<Meni>) {
        Object.assign(this, init);
    }


}
