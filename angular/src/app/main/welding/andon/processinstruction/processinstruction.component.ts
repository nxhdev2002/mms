import { DatePipe } from '@angular/common';
import { DateTime } from 'luxon';
import { Component, Injector, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';


//add Service
import { WldAdoProcessInstructionServiceProxy, GetProcessInstructionDataOutput } from '@shared/service-proxies/service-proxies';


//app


//form edit

@Component({
    templateUrl: './processinstruction.component.html',
    styleUrls: [
        './processinstruction.component.less',
    ],
})

export class ProcessInstructionComponent extends AppComponentBase {

    isLoading: boolean = false;
    rowData: any[] = [];
    pipe = new DatePipe('en-US');
    Line: string;
    Process: string;
    wFullBar: number;


    constructor(
        injector: Injector,
        private _serviceProxy: WldAdoProcessInstructionServiceProxy,
    ) {
        super(injector);
        //alert('1: constructor');
    }

    ngOnInit(): void {
        //alert('2: ngOnInit');
    }

    ngAfterViewInit() {
        //alert('3: ngAfterViewInit')
        setTimeout(() => {
            this.layoutScreen();

            let urlParams = new URLSearchParams(window.location.search);
            this.Line = urlParams.get('Line');
            this.Process = urlParams.get('Screen');
            this.repeatBindData();
        }, 1000);
    }

    @HostListener('load', ['$event'])
    onLoad() {
        this.layoutScreen();
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        //alert('4: onWindowResize')
        this.layoutScreen();
            // this.notify.success('Height: ' + hButton);
    }
    ////end set width height

    ngOnDestroy(): void{
        clearTimeout(this.timerstorage);
    }
    timecount:number = 0;
    timerstorage;
    repeatBindData(){

        this.timecount = this.timecount + 1;
        this.PROCESS_INSTRUCTION_DATA();

        console.log("repeat this.repeatBindData():"+ this.timecount);

        this.timerstorage = setTimeout(()=>{
            this.repeatBindData();
        }, 1000);
    }

    PROCESS_INSTRUCTION_DATA() {
        this. _serviceProxy.getProcessInstructionData(this.Line, this.Process)
        .subscribe((result) => {
            if(result){
                this.PROCESS_INSTRUCTION_BindData(result);
            }
        });
    }

    PROCESS_INSTRUCTION_BindData(_data: GetProcessInstructionDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .content.tack'); //
        if(_data.length > 0){

            _d.querySelector('.devan.next .item .LOT_NO').textContent = _data[0].nextLotNo;
            _d.querySelector('.devan.next .item .BODY_NO').textContent = _data[0].nextBodyNo;

            _d.querySelector('.devan.current .item .LOT_NO').textContent = _data[0].currentLotNo;
            _d.querySelector('.devan.current .item .BODY_NO').textContent = _data[0].currentBodyNo;

            _d.querySelector('.devancontent .devan .item .NoOfCallPart').textContent = _data[0].noOfCallPart.toString();
            _d.querySelector('.devancontent .devan .item .ChangeTipPink').textContent = _data[0].changeTipPink.toString();
            _d.querySelector('.devancontent .devan .item .ChangeTipOrange').textContent = _data[0].changeTipOrange.toString();
            _d.querySelector('.devancontent .devan .item .ChangeTipYellow').textContent = _data[0].changeTipYellow.toString();
            _d.querySelector('.devancontent .devan .item .ChangeTipGreen').textContent = _data[0].changeTipGreen.toString();
            _d.querySelector('.devancontent .devan .item .ChangeTipWhite').textContent = _data[0].changeTipWhite.toString();

            _d.querySelector('.devancontent .devan .item .TatkTime').textContent = _data[0].tatkTime.toString();
            _d.querySelector('.devancontent .devan .item .PlanNum').textContent = _data[0].planNum.toString();
            _d.querySelector('.devancontent .devan .item .ActualNum').textContent = _data[0].actualNum.toString();
            _d.querySelector('.devancontent .devan .item .DelayNum').textContent = _data[0].delayNum.toString();
            _d.querySelector('.devancontent .devan .item .Eff').textContent = _data[0].eff.toString();


            //color     -phuongdv
            /*
            let _b = _d.querySelector('.devan .item.bodyno');
            for(let i=0; _b[i]; i++){ _b[i].classList.remove('K','V','I','F','C'); }

            _d.querySelector('.devan.next .item.bodyno').classList.add(_data[0].nextBodyNo.substring(0,2));
            _d.querySelector('.devan.current .item.bodyno').classList.add(_data[0].currentBodyNo.substring(0,2));
            */
            //progress bar

            let secondnow = (_data[0].currentTime.diff(_data[0].startTime,["seconds"]))['seconds'];
            let per = secondnow / (_data[0].tatkTime);
            let wProgress = this.wFullBar - (this.wFullBar * per);

            let countdown = Math.floor(_data[0].tatkTime - secondnow);


            let bar = document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar .item .barprocess');
            let bartxt = document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar .item .COUNTDOWN_SECOND');
            bar.style.width = wProgress + 'px';
            bartxt.textContent = countdown.toString();

            if(countdown < 0){
                if(!bartxt.classList.contains('DELAY'))  bartxt.classList.add('DELAY');
                else  bartxt.classList.remove('DELAY');

                bar.style.width = 0 + 'px';
            }
            else {
                bar.style.width = wProgress + 'px';
                bartxt.classList.remove('DELAY');
            }

        }
    }

    numbers:Array<any> = [];
    fornumbers(num:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+1);
        return this.numbers;
    }

    fornumbers_next(num:number, start:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+start);
        return this.numbers;
    }

    numbers_desc:Array<any> = [];
    fornumbers_desc(num:number) {
        this.numbers_desc = Array.from({length:num},(v,k)=>num-k);
        return this.numbers_desc;
    }

    fornumbersRangeDesc(start:number, stop:number, step:number){
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    fornumbersRange(start:number, stop:number, step:number){
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }


    layoutScreen(){
        ///////////////////////////design width
        let w = window.innerWidth -2;
        let w2 = Math.floor((w / 2) * 100) / 100;
        let wcol = Math.floor((w2 / 6) * 100) / 100 - 2;

        //body
        let _bd = document.querySelectorAll<HTMLElement>('.PTA, .PTA .content');
        for(let i=0; _bd[i]; i++){ _bd[i].style.width = w + 'px'; }

        //devan next, current
        let _devan = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .devan');
        for(let i=0; _devan[i]; i++){ _devan[i].style.width = w2 - 1 + 'px'; }

        // item progress bar
        document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar').style.width = (w - 3) + 'px';
        this.wFullBar = (w - 12);
        document.querySelector<HTMLElement>('.PTA .PTA_Content .devancontent.progressbar .item').style.width = this.wFullBar + 'px';

        //col bottom
        let _col1 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x1');
        for(let i=0; _col1[i]; i++){ _col1[i].style.width = wcol + 'px'; }

        let _col2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x2');
        for(let i=0; _col2[i]; i++){ _col2[i].style.width = wcol*2 + 'px'; }

        let _col4 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x4');
        for(let i=0; _col4[i]; i++){ _col4[i].style.width = wcol*4 + 'px'; }

        let _col5 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item .col-x5');
        for(let i=0; _col5[i]; i++){ _col5[i].style.width = wcol*5 + 'px'; }




        ///////////////////////////design height
        let h = window.innerHeight - 24;
        let item = Math.floor((h / 9) * 100) / 100;

        //item small
        let _ismall = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.small, ' +
                                                             '.PTA .PTA_Content .devancontent .item .col');
        for(let i=0; _ismall[i]; i++){ _ismall[i].style.height = item  + 'px'; }

        //item lager
        let _ilager = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.lager');
        for(let i=0; _ilager[i]; i++){ _ilager[i].style.height = (item * 3)/2 + 'px'; }

        //item big
        let _ibig = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .devancontent .item.big');
        for(let i=0; _ibig[i]; i++){ _ibig[i].style.height = (item * 3) + 'px'; }





    }


    setAttributes(element:Element, attributes:any) {
        Object.keys(attributes).forEach(attr => {
          element.setAttribute(attr, attributes[attr]);
        });
    }


    //datetime format
    toDDMMYYY(d:DateTime){
        if(d){
            let _d = d.day.toString() + '/' + d.month.toString() + '/' + d.year.toString();
            return _d;
        }
        return '';
    }
    toHHSS(d:DateTime){
        if(d){
            let _d = d.hour.toString() + ':' + d.second.toString();
            return _d;
        }
        return '';
    }

}
