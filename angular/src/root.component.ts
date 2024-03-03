import { Component } from '@angular/core';
import { NgxSpinnerTextService } from '@app/shared/ngx-spinner-text.service';

@Component({
    selector: 'app-root',
    template: `
        <router-outlet></router-outlet>
        <ngx-spinner type="ball-clip-rotate" size="medium" color="#5ba7ea">
            <!-- <p class="fas fa-file-excel" *ngIf="ngxSpinnerText">{{ getSpinnerText() }}</p> -->

            <img style="width: 100px" src="/assets/common/images/Timer.gif"  *ngIf="ngxSpinnerText" alt="">
        </ngx-spinner>
    `,
})
export class RootComponent {
    ngxSpinnerText: NgxSpinnerTextService;

    constructor(_ngxSpinnerText: NgxSpinnerTextService) {
        this.ngxSpinnerText = _ngxSpinnerText;
    }

    getSpinnerText(): string {
        return this.ngxSpinnerText.getText();
    }
}
