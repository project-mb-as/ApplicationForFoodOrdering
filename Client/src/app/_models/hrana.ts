import { Prilog } from './prilog';
import { Comment } from './comment';

export class Hrana {
  hranaId: number;
  naziv: string;
  izabrana: boolean;
  stalna: boolean;
  narucena: boolean;
  rating: number;
  prilozi: Prilog[];
  numberOfComments: number;
  image: string;

  //public constructor(init?: Partial<Hrana>) {
  //  Object.assign(this, init);
  //}

  public constructor(hrana: any) {
    this.hranaId = hrana.hranaId;
    this.naziv = hrana.naziv;
    this.stalna = hrana.stalna;
    this.narucena = false;
    this.rating = hrana.rating;
    //izabrana: this.narudzba.hrana == hranaItem.hrana.hranaId,
    this.prilozi = this.getPrilozi(hrana.prilozi);
    this.numberOfComments = hrana.numberOfComments;
    this.image = hrana.image;
  }

  //public constructor(init?: Partial<Hrana>) {
  //  Object.assign(this, init);
  //}

  private getPrilozi(priloziItem: any[]): Array<Prilog> {
    let prilozi: Prilog[] = [];
    priloziItem.forEach((prilogItem) => {
      prilozi.push(
        new Prilog({
          prilogId: prilogItem.prilogId,
          // naziv: prilogItem.prilog.naziv,
          varijanta: prilogItem.varijanta,
          izabran: false,
          omogucen: true
        })
      );
    });
    return prilozi;
  }
}
