export class Options {
    locationId: number;
    timeId: number;
    receiveOrderConfirmationEmails: boolean;
    receiveOrderWarningEmails: boolean;


    public constructor(init?: Partial<Options>) {
        Object.assign(this, init);
    }
}
