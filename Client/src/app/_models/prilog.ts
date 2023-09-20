export class Prilog {
  prilogId: number;
  naziv: string;
  izabran: boolean;
  omogucen: boolean;
  varijanta: number;

  public constructor(init?: Partial<Prilog>) {
    Object.assign(this, init);
  }
}
