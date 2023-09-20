import { Data } from '@angular/router';


export class Comment {
    user: string;
    content: string;
    time: Date;
    image: string;



    public constructor(init?: Partial<Comment>) {
        Object.assign(this, init);
    }



}
