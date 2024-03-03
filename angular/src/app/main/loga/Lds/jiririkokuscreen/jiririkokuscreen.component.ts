import { finalize } from 'rxjs/operators';

import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { LotDirectSupplyAndonServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    selector: 'app-jiririkokuscreen',
    templateUrl: './jiririkokuscreen.component.html',
    styleUrls: ['./jiririkokuscreen.component.less']
})
export class JiririkokuScreenComponent implements OnInit {
    prodLine:string = '';
    //dataLotDirect: any[] = [];
    taktTime:string = '';
    efficiency:number = 0;
    clearTimeLoadData;
    fn:CommonFunction = new CommonFunction();

    constructor(injector: Injector,
        private activatedRoute: ActivatedRoute,
        private _service: LotDirectSupplyAndonServiceProxy,) { }

    ngOnInit() {
        //this.prodLine = this.activatedRoute.snapshot.queryParams ['pline'] || '';
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');
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

            this.getDataDirectSupplyMonitor();
            this.fn.showtime('time_now_log');

        } catch(ex){
            console.log('1: '+ ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }


    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        console.log('onWindowResize');

    }

    getDataDirectSupplyMonitor() {

        this._service.getDataDirectSupplyMonitor(this.prodLine)
            .subscribe((result) => {
                // this.dataLotDirect = result.items ?? [];

                this.loadForm(result.items ?? []);

                this.clearTimeLoadData = setTimeout(()=>{
                    this.timeoutData();
                }, 1000);
            },(error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(()=>{
                    this.timeoutData();
                }, 1000);
            });


    }


    loadForm(data:any[]) {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var hLct1 = h*10/100-1;
        var hLct2 = h*30/100-1;
        var hLct3 = h-hLct1-hLct2-1;
        var wLCt = w*16/100-2;
        var wRCt = w-wLCt-2;

        var JIRIRIKOKU = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU');
        (JIRIRIKOKU[0] as HTMLElement).style.width = w + 'px';
        (JIRIRIKOKU[0] as HTMLElement).style.height = h + 'px';

        var LEFTCT = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU .LEFT-CONTENT');
        for (let i = 0; LEFTCT[i]; i++) {
        (LEFTCT[i] as HTMLElement).style.width = wLCt + 'px';
        }

        var RIGHTCT = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU .RIGHT-CONTENT');
        for (let i = 0; RIGHTCT[i]; i++) {
        (RIGHTCT[i] as HTMLElement).style.width = wRCt + 'px';
        }

        var LEFTCT1 = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU .LEFT-CONTENT .LEFT-CT1,'
                                                            +'.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT1');
        for (let i = 0; LEFTCT1[i]; i++) {
        (LEFTCT1[i] as HTMLElement).style.height = hLct1 + 'px';
        }

        var LEFTCT2 = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU .LEFT-CONTENT .LEFT-CT2,'
                                                            +'.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT2');
        for (let i = 0; LEFTCT2[i]; i++) {
        (LEFTCT2[i] as HTMLElement).style.height = hLct2 + 'px';
        }

        var LEFTCT3 = document.querySelectorAll<HTMLElement>('.JIRIRIKOKU .LEFT-CONTENT .LEFT-CT3,'
                                                            +'.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT3');
        for (let i = 0; LEFTCT3[i]; i++) {
        (LEFTCT3[i] as HTMLElement).style.height = hLct3 + 'px';
        }

//load data
        let htmTitle = document.querySelector('.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT1');
        // var data =  this.dataLotDirect;

        if(data.length > 0)
        {
            this.taktTime = (data[0].taktTime * 1 / 60).toFixed(1) + "'";
            this.efficiency = data[0].efficiency;
            htmTitle.innerHTML = data[0].title;

            let htmRIGHTCT2 = document.querySelector('.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT2');
            var htm = "";
            var wRct2 = wRCt/data[0].totalCycle;
            for (var i = 0; i < data[0].totalCycle; i++)
                {

                    if(i < data.length){
                        var htm = htm + '<div style="height: 100%;border-right: 1px solid black;display: flex;justify-content: center;'
                                    +'align-items: center;text-align: center;width:' + (wRct2) + 'px' + '">'+(i+1)+'</div>';
                    }
                    else if(data.length < data[0].totalCycle){
                        var htm = htm + '<div style="height: 100%;border-right: 1px solid black;display: flex;justify-content: center;'
                                    +'align-items: center;text-align: center;width:' + (wRct2) + 'px' + '"></div>';
                    }
                }
            htmRIGHTCT2.innerHTML = htm;



            let htmRIGHTCT3 = document.querySelector('.JIRIRIKOKU .RIGHT-CONTENT .RIGHT-CT3');
            var htm1 = "";
            for (var i = 0; i < data[0].totalCycle; i++) {
                var wRct3 = wRCt/data[0].totalCycle;

                if(i < data.length)
                {
                    if(data[i].isDelay == "Y" ){
                        var converseTime = (data[i].delaySecond * 1 / 60).toFixed(1) + "'"

                        var htm1 = htm1 + '<div style="height: 100%;border-right: 1px solid black;background-color: red;font-size:20px;margin-bottom;position: relative;width:' + (wRct3) + 'px' + '"><div style="width : 100%; height : 20px; bottom:15px ;  position:absolute; color: black">'+ converseTime +'</div> </div>';
                    }
                    else if(data[i].status == "FINISHED"){
                        var htm1 = htm1 + '<div style="height: 100%;border-right: 1px solid black;background-color: #92D050;width:' + (wRct3) + 'px' + '"></div>';
                    }
                    else{
                        var htm1 = htm1 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                    }
                }
                else if(data.length < data[0].totalCycle)
                {
                    var htm1 = htm1 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                }

            }
            htmRIGHTCT3.innerHTML = htm1;
        }else{
            htmTitle.innerHTML = "LOT DIRECT SUPPLY MONITORING SCREEN";
        }
    }
}

