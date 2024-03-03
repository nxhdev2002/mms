import { Component, HostListener, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { LgaBp2BigPartDSMDto, LgaBp2BigPartDSMServiceProxy } from "@shared/service-proxies/service-proxies";
import { filter } from "lodash-es";
import { finalize } from 'rxjs/operators';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    selector: 'app-bigpartjiririkokuscreen',
    templateUrl: './bigpartjiririkokuscreen.component.html',
    styleUrls: ['./bigpartjiririkokuscreen.component.less']
})
export class BigPartJiririkokuScreenComponent implements OnInit {

    prodLine:string = '';
    dataBigPartDSM: LgaBp2BigPartDSMDto[] = [];
    taktTime:number = 0;
    efficiency:number = 0;
    clearTimeLoadData;
    fn:CommonFunction = new CommonFunction();

    constructor(injector: Injector,
        private activatedRoute: ActivatedRoute,
        private _service: LgaBp2BigPartDSMServiceProxy,) { }


    ngOnInit() {
        //this.prodLine = this.activatedRoute.snapshot.queryParams ['pline'] || '';
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');

        this.loadForm();
        // this.getData();

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
            this.fn.showtime('time_now_log');

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    getData() {
            this._service.getDataBigPartDSM(this.prodLine).pipe(finalize(() => {}))
                .subscribe((result) => {

                    if (result.items) {
                        this.bindData(result.items);

                        this.clearTimeLoadData = setTimeout(() => {
                            this.timeoutData();
                        }, 1000);
                    }
                },(error) => {
                    console.log(error);
                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                });
    }

    widthRightContent: number = 0;
    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var widthLeftContent = (w * 30) / 100;
        var widthRightContent = w - widthLeftContent;
        var heightLeftCT1 = (h * 10) / 100;
        var heightLeftCT3 = h - heightLeftCT1 * 2;
        var heightCT3ITEM = heightLeftCT3 / 2;
        this.widthRightContent = widthRightContent;
        // var widthCT2ITEM = w * 16 /100;
        // var widthCT2ITEM = w * 16 /100;

        var BIGPART = document.querySelectorAll<HTMLElement>('.BIGPARTMONITORING');
        for (let i = 0; BIGPART[i]; i++) {
            BIGPART[i].style.width = w + 'px';
            BIGPART[i].style.height = h + 'px';
        }

        var LEFTCONTENT = document.querySelector<HTMLElement>('.LEFT-CONTENT');
        LEFTCONTENT.style.width = widthLeftContent + 'px';
        LEFTCONTENT.style.height = h + 'px';

        var RIGHTCONTENT = document.querySelector<HTMLElement>('.RIGHT-CONTENT');
        RIGHTCONTENT.style.width = widthRightContent + 'px';
        RIGHTCONTENT.style.height = h + 'px';

        var LEFTCT1 = document.querySelector<HTMLElement>('.LEFT-CT1');
        LEFTCT1.style.width = widthLeftContent + 'px';
        LEFTCT1.style.height = heightLeftCT1 + 'px';

        var LEFTCT2 = document.querySelector<HTMLElement>('.LEFT-CT2');
        LEFTCT2.style.width = widthLeftContent + 'px';
        LEFTCT2.style.height = heightLeftCT1 + 'px';

        var LCT2 = document.querySelectorAll<HTMLElement>('.LCT2T,' + '.LCT2P');
        for (let i = 0; LCT2[i]; i++) {
            LCT2[i].style.height = heightLeftCT1 + 'px';
        }

        var LEFTCT3 = document.querySelector<HTMLElement>('.LEFT-CT3');
        LEFTCT3.style.width = widthLeftContent + 'px';
        LEFTCT3.style.height = heightLeftCT3 + 'px';

        var LCT3T = document.querySelector<HTMLElement>('.LCT3T');
        LCT3T.style.height = heightLeftCT3 + 'px';

        var LCT3TITEM1 = document.querySelector<HTMLElement>('.LCT3P1');
        LCT3TITEM1.style.height = heightLeftCT3 + 'px';

        //#region set height col Ecar - prodLine = A1
        var ECAR = document.querySelector<HTMLElement>('.ECAR');
        ECAR.style.height = heightLeftCT3 + 'px';

        var ECAR_ITEM = document.querySelectorAll<HTMLElement>('.ECAR1,'+'.ECAR2');
        for (let i = 0; ECAR_ITEM[i]; i++) {
            ECAR_ITEM[i].style.height = heightLeftCT3/2 + 'px';
        //#endregion
        }

        var LEFTCT3ITEM1 = document.querySelector<HTMLElement>('.LCT3P-ITEM1');
        LEFTCT3ITEM1.style.height = heightCT3ITEM + 'px';

        var LEFTCT3ITEM2 = document.querySelector<HTMLElement>('.LCT3P-ITEM2');
        LEFTCT3ITEM2.style.height = heightCT3ITEM + 'px';

        var RIGHTCT1 = document.querySelector<HTMLElement>('.RIGHT-CT1');
        RIGHTCT1.style.width = widthRightContent + 'px';
        RIGHTCT1.style.height = heightLeftCT1 + 'px';

        var RIGHTCT2 = document.querySelector<HTMLElement>('.RIGHT-CT2');
        RIGHTCT2.style.width = widthRightContent + 'px';
        RIGHTCT2.style.height = heightLeftCT1 + 'px';

        var RIGHTCT3 = document.querySelector<HTMLElement>('.RIGHT-CT3');
        RIGHTCT3.style.width = widthRightContent + 'px';
        RIGHTCT3.style.height = heightCT3ITEM + 'px';

        var RIGHTCT4 = document.querySelector<HTMLElement>('.RIGHT-CT4');
        RIGHTCT4.style.width = widthRightContent + 'px';
        RIGHTCT4.style.height = heightCT3ITEM + 'px';
    }

    bindData(data: LgaBp2BigPartDSMDto[]){

        //load data
        let htmTitle = document.querySelector('.RIGHT-CT1');
        let htmTaktTime = document.querySelector('.LEFT-CT1');
        let htmEFF = document.querySelector('.BIGPARTMONITORING .LEFT-CT3 .LCT3T');
        // let htmECARNAME = document.querySelector('.LCT3P1');
        // var data =  this.dataBigPartDSM;

        if(data.length > 0)
        {
            htmTaktTime.innerHTML = "T.T : "+(data[0].taktTime * 1 / 60).toFixed(1) + "'"
            htmTitle.innerHTML = data[0].title;
            htmEFF.innerHTML = data[0].efficiency.toString();
            var v_totalCycle = (data[0].totalCycle);
            let processNamePK = data.filter(e => e.processName === "PK")
            let processNameDEL = data.filter(e => e.processName === "DEL")



            let htmRIGHTCT2 = document.querySelector('.RIGHT-CT2');
            var htm = "";
            for (var i = 0; i < v_totalCycle; i++)
                {
                    var wRct2 = this.widthRightContent/v_totalCycle;
                    if(i < processNamePK.length){
                        var htm = htm + '<div style="height: 100%;border-right: 1px solid black;display: flex;justify-content: center;'
                                    +'align-items: center;text-align: center;width:' + (wRct2) + 'px' + '">'+(i+1)+'</div>';
                    }
                    else if(processNamePK.length < v_totalCycle){
                        var htm = htm + '<div style="height: 100%;border-right: 1px solid black;display: flex;justify-content: center;'
                                    +'align-items: center;text-align: center;width:' + (wRct2) + 'px' + '"></div>';
                    }
                }
            htmRIGHTCT2.innerHTML = htm;



                //RIGHTCT3
            let htmRIGHTCT3 = document.querySelector('.RIGHT-CT3');
                var htm3 = "";
                var wRct3 = this.widthRightContent/v_totalCycle;
                for (var i = 0; i < v_totalCycle; i++) {
                    if(i < processNamePK.length)
                    {
                        if(processNamePK[i].isDelay == "Y" ){
                            var converseTime = (data[i].delaySecond * 1 / 60).toFixed(1) + "'"
                            var htm3 = htm3 + '<div style="height: 100%;border-right: 1px solid black;background-color: red;font-size:20px;margin-bottom;position: relative;width:' + (wRct3) + 'px' + '">'
                                            + '<div style="width : 100%;font-weight:bold; height : 20px; bottom:15px ; position:absolute; color: black">'+ converseTime +'</div> </div>';
                        }
                        else if(processNamePK[i].status == "FINISHED"){
                            var htm3 = htm3 + '<div style="height: 100%;border-right: 1px solid black;background-color: #92D050;width:' + (wRct3) + 'px' + '"></div>';
                        }
                        else{
                            var htm3 = htm3 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                        }
                    }
                    else if(processNamePK.length <= i)
                    {
                        var htm3 = htm3 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                    }
                }
            htmRIGHTCT3.innerHTML = htm3;

            //RIGHTCT4
            let htmRIGHTCT4 = document.querySelector('.RIGHT-CT4');
            var htm4 = "";
                var wRct3 = this.widthRightContent/v_totalCycle;
                for (var i = 0; i < v_totalCycle; i++) {
                    if(i < processNameDEL.length)
                    {
                        if(processNameDEL[i].isDelay == "Y" ){
                            var converseTime = (data[i].delaySecond * 1 / 60).toFixed(1) + "'"
                            var htm4 = htm4 + '<div style="height: 100%;border-right: 1px solid black;background-color: red;font-size:20px;margin-bottom;position: relative;width:' + (wRct3) + 'px' + '">'
                                            + '<div style="width : 100%;font-weight:bold; height : 20px; bottom:15px ; position:absolute; color: black">'+ converseTime +'</div> </div>';
                        }
                        else if(processNameDEL[i].status == "FINISHED"){
                            var htm4 = htm4 + '<div style="height: 100%;border-right: 1px solid black;background-color: #92D050;width:' + (wRct3) + 'px' + '"></div>';
                        }
                        else{
                            var htm4 = htm4 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                        }
                    }
                    else if(processNameDEL.length <= i)
                    {
                        var htm4 = htm4 + '<div style="height: 100%;border-right: 1px solid black;width:' + (wRct3) + 'px' + '"></div>';
                    }
                }
            htmRIGHTCT4.innerHTML = htm4;

        } else{
            htmTitle.innerHTML = "LOT DIRECT SUPPLY MONITORING SCREEN";
        }

        const distinctThings = data.filter(
            (thing, i, arr) => arr.findIndex(t => t.ecarName === thing.ecarName) === i
          );
        var countEcarname = distinctThings.length

        if(countEcarname == 2)
        {
            var LCT3TITEM1 = document.querySelector<HTMLElement>('.LCT3P1');
            LCT3TITEM1.style.display = 'none';
        }
        else if(countEcarname == 1) {
            var ECAR = document.querySelector<HTMLElement>('.ECAR');
            ECAR.style.display = 'none';
        }
    }
    //load data

    showtime(){
        let _d = new Date();
        document.querySelector<HTMLElement>('.time_now_log').textContent = _d.getHours() + ":" + _d.getMinutes() + "" ;
    }
}
