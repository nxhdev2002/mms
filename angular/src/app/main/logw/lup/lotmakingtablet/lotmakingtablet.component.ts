import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotMakingTabletDto, LotMakingTabletServiceProxy } from '@shared/service-proxies/service-proxies';
import { Paginator } from 'primeng/paginator';
import { LotMakingTabletModalComponent } from './lotmakingtablet-modal.component';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-lotmakingtablet',
    templateUrl: './lotmakingtablet.component.html',
    styleUrls: ['./lotmakingtablet.component.less']
})
export class LotMakingTabletComponent implements OnInit {
    @ViewChild('popUpLotMk', { static: true }) popUpLotMk: | LotMakingTabletModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    dataLotMK_W1: LotMakingTabletDto[] = [];
    dataLotMK_W2: LotMakingTabletDto[] = [];
    dataLotMK_W3: LotMakingTabletDto[] = [];
    dataCountW1: number = 0;
    dataCountW2: number = 0;
    dataCountW3: number = 0;
    clearTimeLoadData;

    constructor(private _service: LotMakingTabletServiceProxy,) { }
    ngOnInit(): void {
        this.loadForm();
        console.log('ngOnInit');
    }

    ngAfterViewInit() {
        this.timeoutData();
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }

    timecount:number = 0;
    refeshPage: number = 600;
    timeoutData()
    {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getMkDataLotUpPlan();

        } catch(ex){
            console.log('1: '+ ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
        this.getMkDataLotUpPlan();
    }

    getMkDataLotUpPlan() {
        try{
            console.log('getdata');
            this._service.getMkDataLotUpPlan().pipe(finalize(() => {
            })).subscribe((result) => {
                if (result) {
                    this.dataLotMK_W1 = (result.items ?? []).filter(a => a.prodLine === "W1");
                    this.dataLotMK_W2 = (result.items ?? []).filter(a => a.prodLine === "W2");
                    this.dataLotMK_W3 = (result.items ?? []).filter(a => a.prodLine === "W3");
                    this.dataCountW1 = this.dataLotMK_W1.length;
                    this.dataCountW2 = this.dataLotMK_W2.length;
                    this.dataCountW3 = this.dataLotMK_W3.length;

                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                }
            })
        }catch (ex){
            console.log('2: '+ ex);
            this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
            }, 1000);
        }

    }

    showPopup(status:string, prodLine:string , lotNo:string , id :number){
        if(status=="NONE")
            this.popUpLotMk.show(prodLine,lotNo,id);
    }

    fornumbersRange() {
        let numRange: number[] = [];
        for (let i = 1; i <= 5; i++) {
            numRange.push(i);0
        }
        return numRange;
    }

    dataLotMkW1() {
        let numRange: LotMakingTabletDto[] = [];
        for (let i = 0; i < this.dataCountW1 && i < 5;) {
            numRange.push(this.dataLotMK_W1[i]);
            i = i + 1;
        }
        return numRange;
    }

    dataLotMkW2() {
        let numRange: LotMakingTabletDto[] = [];
        for (let i = 0; i < this.dataCountW2 && i < 5;) {
            numRange.push(this.dataLotMK_W2[i]);
            i = i + 1;
        }
        return numRange;
    }

    dataLotMkW3() {
        let numRange: LotMakingTabletDto[] = [];
        for (let i = 0; i < this.dataCountW3 && i < 5;) {
            numRange.push(this.dataLotMK_W3[i]);
            i = i + 1;
        }
        return numRange;
    }

    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var widthBoxA1 = w * 10 / 100;
        var widthBoxA2 = w - widthBoxA1;

        var PTA = document.querySelectorAll<HTMLElement>('.ALL');
        (PTA[0] as HTMLElement).style.width = w + 'px';
        (PTA[0] as HTMLElement).style.height = h + 'px';

        var ALL_BOX1 = document.querySelectorAll<HTMLElement>('.ALL .BOX1');
        for (let i = 0; ALL_BOX1[i]; i++) {
            (ALL_BOX1[i] as HTMLElement).style.width = widthBoxA1 + 'px';
            (ALL_BOX1[i] as HTMLElement).style.height = h + 'px';
        }

        var ALL_BOX2 = document.querySelectorAll<HTMLElement>('.ALL .BOX2');
        for (let i = 0; ALL_BOX2[i]; i++) {
            (ALL_BOX2[i] as HTMLElement).style.width = widthBoxA2 + 'px';
            (ALL_BOX2[i] as HTMLElement).style.height = h + 'px';
        }
    }

}
