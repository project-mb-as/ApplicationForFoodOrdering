import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'app-delete-dialog',
    templateUrl: './delete-dialog.component.html',
    styleUrls: ['./delete-dialog.component.less']
})
export class DeleteDialogComponent implements OnInit {
    constructor(public dialogRef: MatDialogRef<DeleteDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any) { }

    ngOnInit(): void {
    }

    closeDialog = () => {
        this.dialogRef.close(false);
    }

    delete() {
        this.dialogRef.close(true);
    }

}
