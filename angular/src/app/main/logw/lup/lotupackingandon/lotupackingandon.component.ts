import { DatePipe } from '@angular/common';
import { DateTime } from 'luxon';
import { Component, Injector, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';


//add Service
import { LgwLupLotUnPackingAndonServiceProxy, GetLotUnPackingAndonOutput } from '@shared/service-proxies/service-proxies';


//app
import { CommonFunction } from "@app/main/commonfuncton.component";

//form edit

@Component({
    templateUrl: './lotupackingandon.component.html',
    styleUrls: [
        './lotupackingandon.component.less',
    ],
})

export class LotUpackingAndonComponent extends AppComponentBase {

    isLoading: boolean = false;
    rowData: any[] = [];
    pipe = new DatePipe('en-US');
    Line: string;
    Process: string;
    wFullBar: number;
    fn:CommonFunction = new CommonFunction();


    constructor(
        injector: Injector,
        private _serviceProxy: LgwLupLotUnPackingAndonServiceProxy,
    ) {
        super(injector);
        // console.log('1: constructor');
    }

    ngOnInit(): void {
        // console.log('2: ngOnInit');
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.layoutScreen();
            let urlParams = new URLSearchParams(window.location.search);
            this.Line = urlParams.get('Line');
            this.Process = urlParams.get('Screen');
        }, 500);
        this.repeatBindData();

    }

    @HostListener('load', ['$event'])
    onLoad() {
        console.log('onLoad');




    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        console.log('4: onWindowResize')
        this.layoutScreen();
    }
    ////end set width height

    ngOnDestroy(): void {
        clearTimeout(this.timerstorage);
    }
    timecount: number = 0;
    refeshPage: number = 600;
    timerstorage;
    secondDelay = 1000;
    repeatBindData() {
        // document.querySelector('.PTA .log').textContent = this.timecount.toString();
        try{
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.PROCESS_INSTRUCTION_DATA();
            this.fn.showtime('time_now_log');

        }catch(ex){
            console.error(ex);
            this.timerstorage = setTimeout(() => {
                this.repeatBindData();
            }, this.secondDelay);
        }
    }

    PROCESS_INSTRUCTION_DATA() {
        this._serviceProxy.lgwLupLotUnPackingAndonGetData(this.Line)
        .subscribe((result) => {
            if (result) {
                console.log('result data ok: ' + this.timecount);
                this.BIND_DATA(result);
            }

            this.timerstorage = setTimeout(()=>{
                this.repeatBindData();
            }, this.secondDelay);

        },(error) => {
            console.log(error);
            this.timerstorage = setTimeout(()=>{
                this.repeatBindData();
            }, this.secondDelay);
        });

    }

    BIND_DATA(_data: GetLotUnPackingAndonOutput[]) {

        this.CLEAR_NEXT_CURRENT('current');
        if(_data.length > 0){
            this.BIND_NEXT_CURRENT(_data[0], 'current');
            this.BIND_PROGRESS_BAR(_data[0], 'current');
        }

        this.CLEAR_NEXT_CURRENT('next');
        if(_data.length > 1){
            this.BIND_NEXT_CURRENT(_data[1], 'next');
            if(_data[1].upStatus == 'CALLED'){
                this.BIND_PROGRESS_BAR(_data[1], 'next');
            }
        }

    }


    BIND_NEXT_CURRENT(_data: GetLotUnPackingAndonOutput, thislot : string){

        //ot
        document.querySelector<HTMLElement>('.PTA .devancontent .cont .item .TitleOT .val').textContent = _data.otTime.toString();

        let _gradeVal = '';
        let _no = '';
        if(_data.lotNo != ''){
            let lots = _data.lotNo.split('-');
            _gradeVal = lots[0];
            _no = lots[1];
        }
        let grade = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .BODY_NO');
        if(grade){ grade.textContent =_gradeVal; }
        let no = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .QTY');
        if(no){ no.textContent =_no; }
        let uplot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .UNPACK_LOT');
        if(uplot){ uplot.textContent =_data.finishedMkModuleCount.toString().replace('-',''); }
        let totallot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .TOTAL_LOT');
        if(totallot){ totallot.textContent =_data.mkModuleCount.toString(); }

        let tpm = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .TMP');

        if (_data.tpm == '') {
            if(tpm){ tpm.style.display = 'none'; }
        }else{
            if(tpm){ tpm.style.display = 'flex'; }
        }

        let body = document.querySelectorAll<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont');
        for (let i = 0; body[i]; i++) { body[i].classList.remove("STARTED", "CALLED", "FINISHED"); }
        if(_data.upStatus){
            for (let i = 0; body[i]; i++) { body[i].classList.add(_data.upStatus); }
        }
    }

    BIND_PROGRESS_BAR(_data: GetLotUnPackingAndonOutput, thislot : string) {
        //Countdown
        let _countdown = document.querySelector<HTMLElement>('.PTA .devancontent.progressbar .devan.'+thislot+' .cont .item .COUNTDOWN_SECOND');
        if(_countdown){ _countdown.textContent = (_data.unpackingLeadTime - _data.actualTime).toString() + 's'; }

        // let ac  = (_data.actualTime == 0) ? 120: _data.actualTime;
        let ac  = _data.actualTime;
        let countdowPer =  ac / (_data.unpackingLeadTime);

        let wProgress = this.wFullBar - (this.wFullBar * countdowPer);
        document.querySelector<HTMLElement>('.PTA .devancontent.progressbar .devan.'+thislot+' .cont .item .barprocess').style.width = wProgress + 'px' ;

        //countdown lot
        // let aclot =  (_data.finishedMkModuleCount == 0) ? 2: _data.finishedMkModuleCount;
        let aclot = _data.finishedMkModuleCount;
        let perlot = (_data.mkModuleCount - aclot)  / (_data.mkModuleCount);
        let wlot = this.wFullBar - (this.wFullBar * perlot);
        wlot = (wlot > this.wFullBar) ? this.wFullBar : wlot;

        let countdownlot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .countdown_lot');
        countdownlot.style.width = wlot + 'px' ;
        countdownlot.classList.remove("DELAY");

        if (_data.isDelay == 'Y') {
            countdownlot.classList.add("DELAY");
        }
    }

    CLEAR_NEXT_CURRENT(thislot : string){
        let grade = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .BODY_NO');
        if(grade){ grade.textContent =''; }
        let no = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .QTY');
        if(no){ no.textContent =''; }
        let uplot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .UNPACK_LOT');
        if(uplot){ uplot.textContent ='0'; }
        let totallot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .TOTAL_LOT');
        if(totallot){ totallot.textContent ='0'; }

        let tpm = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .item .TMP');
        if(tpm){ tpm.style.display = 'none'; }

        let body = document.querySelectorAll<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont');
        for (let i = 0; body[i]; i++) { body[i].classList.remove("STARTED", "CALLED", "FINISHED"); }

        //Countdown
        let _countdown = document.querySelector<HTMLElement>('.PTA .devancontent.progressbar .devan.'+thislot+' .cont .item .COUNTDOWN_SECOND');
        if(_countdown){ _countdown.textContent = ''; }

        let wProgress = this.wFullBar;
        document.querySelector<HTMLElement>('.PTA .devancontent.progressbar .devan.'+thislot+' .cont .item .barprocess').style.width = wProgress + 'px' ;

        let countdownlot = document.querySelector<HTMLElement>('.PTA .devancontent .devan.'+thislot+' .cont .countdown_lot');
        countdownlot.style.width = 0 + 'px' ;
        countdownlot.classList.remove("DELAY");

    }




    numbers: Array<any> = [];
    fornumbers(num: number) {
        this.numbers = Array.from({ length: num }, (v, k) => k + 1);
        return this.numbers;
    }

    fornumbers_next(num: number, start: number) {
        this.numbers = Array.from({ length: num }, (v, k) => k + start);
        return this.numbers;
    }

    numbers_desc: Array<any> = [];
    fornumbers_desc(num: number) {
        this.numbers_desc = Array.from({ length: num }, (v, k) => num - k);
        return this.numbers_desc;
    }

    fornumbersRangeDesc(start: number, stop: number, step: number) {
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }


    layoutScreen() {
        ///////////////////////////design width
        let wmain = window.innerWidth ;
        let wleft = Math.floor((wmain / 7) * 100) / 100;

        let w = wmain - wleft;
        let w2 = Math.floor((w / 2) * 100) / 100;
        let wcol = Math.floor((w2 / 6) * 100) / 100 ;

        //body
        let blf = document.querySelectorAll<HTMLElement>('.PTA .PTA_Left');
        for (let i = 0; blf[i]; i++) { blf[i].style.width = wleft + 'px'; }
        let br = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content');
        for (let i = 0; br[i]; i++) { br[i].style.width = w-1 + 'px'; }

        let _bd = document.querySelectorAll<HTMLElement>('.PTA .content');
        for (let i = 0; _bd[i]; i++) { _bd[i].style.width = w + 'px'; }

        //devan next, current
        let _devan = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .devan');
        for (let i = 0; _devan[i]; i++) { _devan[i].style.width = w2 - 1 + 'px'; }

        // item progress bar
        // document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar').style.width = (w - 3) + 'px';
        this.wFullBar = w2 - 1;

        // let _countdownlot = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .countdown_lot');
        // for (let i = 0; _countdownlot[i]; i++) { _countdownlot[i].style.width = w2 - 2 + 'px'; }


        // document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar .item').style.width = this.wFullBar + 'px';

        //col bottom
        let _col1 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x1');
        for (let i = 0; _col1[i]; i++) { _col1[i].style.width = wcol + 'px'; }

        let _col2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x2');
        for (let i = 0; _col2[i]; i++) { _col2[i].style.width = wcol * 2 + 'px'; }

        let _col4 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x4');
        for (let i = 0; _col4[i]; i++) { _col4[i].style.width = wcol * 4 + 'px'; }

        let _col5 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x5');
        for (let i = 0; _col5[i]; i++) { _col5[i].style.width = wcol * 5 + 'px'; }




        ///////////////////////////design height
        let h = window.innerHeight - 16
        let item = Math.floor((h / 9) * 100) / 100;

        //item small
        let _ismall = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.small, ' +
            '.PTA .PTA_Content .devancontent .item .col');
        for (let i = 0; _ismall[i]; i++) { _ismall[i].style.height = item  + 'px'; }

        //item lager
        let _ilager = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.lager, ' +
            '.PTA .PTA_Left .devancontent .item.lager');
        for (let i = 0; _ilager[i]; i++) { _ilager[i].style.height = (item * 3) / 2 + 'px'; }

        //item big
        let _ibig = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.big');
        for (let i = 0; _ibig[i]; i++) { _ibig[i].style.height = (item * 5) + 'px'; }

        //item bigxxl
        let _bigxxl = document.querySelectorAll<HTMLElement>('.PTA .PTA_Left .devancontent .item.bigxxl');
        for (let i = 0; _bigxxl[i]; i++) { _bigxxl[i].style.height = (item * 5) + (item * 3) / 2 + 'px'; }
        //item normal
        let _item_normal = document.querySelectorAll<HTMLElement>('.PTA .devancontent .item.normal, ' +
                                                                                                         ' .PTA .devancontent .item.normal .COUNTDOWN_SECOND, ' +
                                                                                                         ' .PTA .devancontent .item.normal .barprocess');
        for (let i = 0; _item_normal[i]; i++) { _item_normal[i].style.height = (item + 12) + 'px'; }

        //TPM
        let _tpm = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .TMP');
        for (let i = 0; _tpm[i]; i++) { _tpm[i].style.height = (item + 12)/2  + 'px'; }

        //countdown lot
        let _countdownlot = document.querySelectorAll<HTMLElement>('.PTA .devancontent .cont .countdown_lot');
        for (let i = 0; _countdownlot[i]; i++) { _countdownlot[i].style.height = (item * 5) + ((item * 3) / 2) -1 + 'px'; }

    }


    setAttributes(element: Element, attributes: any) {
        Object.keys(attributes).forEach(attr => {
            element.setAttribute(attr, attributes[attr]);
        });
    }


    //datetime format
    toDDMMYYY(d: DateTime) {
        if (d) {
            let _d = d.day.toString() + '/' + d.month.toString() + '/' + d.year.toString();
            return _d;
        }
        return '';
    }
    toHHSS(d: DateTime) {
        if (d) {
            let _d = d.hour.toString() + ':' + d.second.toString();
            return _d;
        }
        return '';
    }

}
