
import { Component, HostListener, Injector, OnInit } from '@angular/core';
import {  WldAdoPunchQueueIndicatorDto, WldAdoPunchQueueIndicatorServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize, filter } from 'rxjs/operators';

@Component({
    selector: 'app-wdpunchindicator',
    templateUrl: './punchqueueindicator.component.html',
    styleUrls: ['./punchqueueindicator.component.less'],
})
export class PunchQueueIndicatorComponent implements OnInit {
    rowStorage: WldAdoPunchQueueIndicatorDto = new WldAdoPunchQueueIndicatorDto();
    dataWdPunchIndicator: any[] = [];
    timeInverval = null;
    clearTimeLoadData

    constructor(injector: Injector,
                private _service: WldAdoPunchQueueIndicatorServiceProxy) {}

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

    timecount: number = 0;
    refeshPage: number = 600;
    timeoutData() {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getData();

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    getData() {
    this._service.getWldAdoPunchQueueIndicator().pipe(finalize(()=> {}))
    .subscribe((result) => {
        try{

            this.dataWdPunchIndicator = result.items ?? [];
            this.loadForm();
            this.bindDataWdPunchIndicator();

        this.clearTimeLoadData = setTimeout(() => {
            this.timeoutData();
        }, 1000);

    } catch (ex) {
        console.log('2: ' + ex);

        this.clearTimeLoadData = setTimeout(() => {
            this.timeoutData();
        }, 1000);
    }
    })
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    loadForm() {
        var h = window.innerHeight;
        var w = window.innerWidth;
        var hConten = h - h / 100 - 4;
        var wContent = w - w / 100;
        var hWarpTitle = hConten / 10;
        var wWarpTitle = wContent - 4;
        var wTitle = wContent / 3;
        var hTitle = (hConten / 10) * 4;
        var hiPunch = hConten - 2 * hTitle - hWarpTitle - 4;

        var content = document.querySelectorAll<HTMLElement>('.wdPunh .content');
        if(content){
            for (let i = 0; content[i]; i++) {
                (content[i] as HTMLElement).style.height = hConten + 'px';
                (content[i] as HTMLElement).style.width = wContent + 'px';
            }
        }

        var warpTitle = document.querySelectorAll<HTMLElement>('.wdPunh .content .warpTitle');
        if(warpTitle){
        for (let i = 0; warpTitle[i]; i++) {
            (warpTitle[i] as HTMLElement).style.height = hWarpTitle + 'px';
            (warpTitle[i] as HTMLElement).style.width = wWarpTitle + 'px';
        }
    }

        var title = document.querySelectorAll<HTMLElement>(
            '.wdPunh .content .warpTitle .title1,' +
                '.wdPunh .content .warpTitle .title2,' +
                '.wdPunh .content .warpTitle .title3'
        );
        if(title){
        for (let i = 0; title[i]; i++) {
            (title[i] as HTMLElement).style.height = hWarpTitle + 'px';
            (title[i] as HTMLElement).style.width = wTitle + 'px';
        }}

        var warp = document.querySelectorAll<HTMLElement>(
            '.wdPunh .content .warpLotNo,' + '.wdPunh .content .warpBody'
        );
        if(warp){
        for (let i = 0; warp[i]; i++) {
            (warp[i] as HTMLElement).style.height = hTitle + 'px';
            (warp[i] as HTMLElement).style.width = wWarpTitle + 'px';
        }}

        var lotNo = document.querySelectorAll<HTMLElement>(
            '.wdPunh .content .warpLotNo .lotNo1,' +
                '.wdPunh .content .warpLotNo .lotNo2,' +
                '.wdPunh .content .warpLotNo .lotNo3'
        );
        if(lotNo){
        for (let i = 0; lotNo[i]; i++) {
            (lotNo[i] as HTMLElement).style.height = hTitle + 'px';
            (lotNo[i] as HTMLElement).style.width = wTitle + 'px';
        }}

        var body = document.querySelectorAll<HTMLElement>(
            '.wdPunh .content .warpBody .body1,' +
                '.wdPunh .content .warpBody .body2,' +
                '.wdPunh .content .warpBody .body3'
        );
       
        for (let i = 0; body[i]; i++) {
            if(body){
            (body[i] as HTMLElement).style.height = hTitle + 'px';
            (body[i] as HTMLElement).style.width = wTitle + 'px';
        }}
        var iPunch = document.querySelectorAll<HTMLElement>(
            '.wdPunh .content .warpIpunch .iPunch1,' +
                '.wdPunh .content .warpIpunch .iPunch2,' +
                '.wdPunh .content .warpIpunch .iPunch3'
        );
        for (let i = 0; iPunch[i]; i++) {
            if(iPunch[i]){
            (iPunch[i] as HTMLElement).style.height = hiPunch + 'px';
            (iPunch[i] as HTMLElement).style.width = wTitle + 'px';
            }
        }

        var footer = document.getElementById("bottom-footer-bar");
        if(footer){
        footer.style.display = "none";
        }

      //  (document.getElementById("kt_aside_menu_wrapper")).style.display = "none";
       // (document.getElementById("kt_header")).style.display = "none";


        var footer = document.getElementById("body-router-outlet");
        if(footer){
        footer.style.backgroundColor = "white";
        }


    }

    bindDataWdPunchIndicator() {
        //bind data

        this.dataWdPunchIndicator.forEach((element) => {
            var o = element as WldAdoPunchQueueIndicatorDto;
            var i = o.filler;
            var lotNoValue = 'lotNo' + i;
            var bodyValue = 'body' + i;
            var iPunchValue = 'iPunch' + i;

            var objItemLotNo = document.querySelectorAll<HTMLElement>(
                '.wdPunh .content .warpLotNo .' + lotNoValue + ''
            );
            if (objItemLotNo != null && o.lotNo != null) {
                for (let i = 0; objItemLotNo[i]; i++) {
                    (objItemLotNo[i] as HTMLElement).innerHTML = o.lotNo;
                }
            }

            var objItemBodyNo = document.querySelectorAll<HTMLElement>('.wdPunh .content .warpBody .' + bodyValue + '');
            if (objItemBodyNo != null && o.bodyNo != null) {
                for (let i = 0; objItemBodyNo[i]; i++) {
                    (objItemBodyNo[i] as HTMLElement).innerHTML = o.bodyNo;
                }
            }

            var objItemiPunch = document.querySelectorAll<HTMLElement>(
                '.wdPunh .content .warpIpunch .' + iPunchValue + ''
            );
            if (objItemiPunch != null && o.punchIndicator != null && o.punchFlag == 'Y') {
                for (let i = 0; i < 2; i++) {
                    (objItemiPunch[i] as HTMLElement).innerHTML = o.punchIndicator;
                    //  (objItemiPunch[i] as HTMLElement).innerHTML = "test"+i+"";
                }
            }

            var objItemlot3 = document.querySelectorAll<HTMLElement>(
                '.wdPunh .content .warpLotNo .lotNo3,' + '.wdPunh .content .warpBody .body3'
            );
            var display = document.getElementById('countdown');

            if (display != null && o.weldSignal != null) {
                switch (o.weldSignal) {
                    case 'CALL':
                        if (this.timeInverval == null) {
                            this.startTimer(o.wTalkTime, display, objItemlot3);
                        }
                        break;
                    case 'CF':
                        if (this.timeInverval != null) {
                            clearInterval(this.timeInverval);
                            this.timeInverval = null;
                        }

                        this.EndFLASH(objItemlot3);
                        break;
                }
            }
        });
    }
    EndFLASH(obj) {
        var t = 0;
        for (let i = 0; i < 2; i++) {
            if(obj[i]){
            obj[i].style.backgroundColor = (t==0) ?  'green' : 'white'
            }
        }
        t = (t==0) ?  1 : 0
    }

    startTimer(duration, display, obj) {
        var timer = duration,
            minutes,
            seconds;
        var t = 0;

        this.timeInverval = setInterval(function () {
            minutes = this.parseInt(timer / 60, 10);
            seconds = this.parseInt(timer % 60, 10);
            minutes = minutes < 10 ? '0' + minutes : minutes;
            seconds = seconds < 10 ? '0' + seconds : seconds;

            if (timer < 0) {
                seconds = seconds.toString().replace(/-/gi, '');
                if (minutes < 1) {
                    minutes = '-00';
                } else {
                    minutes = minutes.slice(1);
                }
                if (seconds > 9) {
                    seconds = seconds.toString().replace(/-/gi, '').slice(1);
                }
            }
            display.innerHTML = minutes + ':' + seconds;
            timer--;
            for (let i = 0; i < 2; i++) {
                if(obj[i]){
                    obj[i].style.backgroundColor =  'yellow'
                    obj[i].style.opacity = (t==0) ?  1 : 0
                }
            }
            t = (t==0) ?  1 : 0
            // for (let i = 0; i < 2; i++) {
            //     obj[i].style.backgroundColor = (t==0) ?  'yellow' : 'white'
            // }
            // t = (t==0) ?  1 : 0
        }, 1000);
    }
}
