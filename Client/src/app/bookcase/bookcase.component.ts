import { Component, OnInit } from '@angular/core';
import { BookcaseService } from './bookcase.service';

export interface Tile {
    color: string;
    cols: number;
    rows: number;
    text: string;
    title: string;
}

@Component({
    selector: 'app-bookcase',
    templateUrl: './bookcase.component.html',
    styleUrls: ['./bookcase.component.less']
})
export class BookcaseComponent implements OnInit {

    constructor(private bookcaseService: BookcaseService) { }

    private retrivedata: Array<any> = [];
    tiles: Tile[] = [];

    //tiles: Tile[] = [
    //    { title: 'First title', text: 'One', cols: 3, rows: 1, color: 'lightblue' },
    //    { title: 'Second title', text: 'Two', cols: 1, rows: 2, color: 'lightgreen' },
    //    { title: 'Third title', text: 'Three', cols: 1, rows: 1, color: 'lightpink' },
    //    { title: 'Fourth title', text: 'Four', cols: 2, rows: 1, color: '#DDBDF1' },
    //];

    ngOnInit() {
        this.bookcaseService.getAllBooks().subscribe((response: any) => {

            for (let i = 0; i < response.body.length; i++) {
                let book = response.body[i];
                console.log(book);
                this.tiles.push({ title: book.name, text: book.name, cols: 3, rows: 1, color: 'lightblue' });
            }

        }); 
    }

}
