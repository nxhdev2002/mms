import { finalize } from 'rxjs/operators';
import { AppComponentBase } from "@shared/common/app-component-base";
import { Component, HostListener, Injector} from '@angular/core';
import { GetDetailsOutput, PtsAdoLineRealTimeControlServiceProxy } from "@shared/service-proxies/service-proxies";
import { DatePipe } from '@angular/common';
import { wrap } from 'module';

@Component({
    templateUrl: './linerealtimecontrol.component.html',
    styleUrls: ['./linerealtimecontrol.component.less'],
})

export class LineRealTimeControlComponent extends AppComponentBase
{
    dataLineRealTime: any[] = [];
    pipe = new DatePipe('en-US');
    clearTimeLoadData;

    constructor(
        injector: Injector,
        private _service: PtsAdoLineRealTimeControlServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.onWindowResize();
    }

    ngAfterViewInit() {
       this.timeoutData();
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
        this._service.getDetails().pipe(finalize(()=> {}))
        .subscribe((result) => {
            try{

            this.dataLineRealTime = result.items ?? [];
            this.bindDataLineRealTimeControl();

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

        //design width
        var w = window.innerWidth;
        var h = window.innerHeight;
        var wcol1 = w/100*16;
        var wcol28 = w/100*12;

        var wcol282 = w/100*6;
        var hrow1 = h/100*16;
        var hrow1Atop = (h/100*16)/100*40;
        var hrow1Abottom = (h/100*16)/100*60;
        var hrow2 = h/100*6;
        var hrow3 = h/100*6;
        var hrow4 = h/100*10;
        var hrow5 = h/100*10;
        var hrow6 = h/100*10;
        var hrow7 = h/100*6;
        var hrow8 = h/100*6;
        var hrow9 = h/100*6;
        var hrow10 = h/100*6;
        var hrow11 = h-hrow1-hrow2-hrow3-hrow4-hrow5-hrow6-hrow7-hrow8-hrow9-hrow10-2;
        var hrow11a = hrow11-2;
        var hrow11a_title = hrow11a/4;
        var hrow11a_value = hrow11a - hrow11a_title;


//#region row1_Line
        var col1Line = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .line .wkDate");
        for(let i=0; col1Line[i]; i++){
            (col1Line[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Line[i] as HTMLElement).style.height = hrow1 + "px";
        }

        var col28Line = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .line .lineW,"
                                                        + ".lineRealTimeControl .line .lineT,"
                                                        + ".lineRealTimeControl .line .lineAtotal,"
                                                        + ".lineRealTimeControl .line .lineIns,"
                                                        + ".lineRealTimeControl .line .linePdi"
                                                        );
        for(let i=0; col28Line[i]; i++){
            (col28Line[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Line[i] as HTMLElement).style.height = hrow1 + "px";
        }

        var col28Line = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .line .lineA1,"
                                                                + ".lineRealTimeControl .line .lineA2"
                                                                );
            for(let i=0; col28Line[i]; i++){
            (col28Line[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Line[i] as HTMLElement).style.height = hrow1 + "px";
            }

        var colAtop = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .line .lineA1 .top,"
                                                             + ".lineRealTimeControl .line .lineA2 .top"
                                                            );
            for(let i=0; colAtop[i]; i++){
                (colAtop[i] as HTMLElement).style.height = hrow1Atop + "px";
            }

        var colAbottom = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .line .lineA1 .bottom,"
                                                                + ".lineRealTimeControl .line .lineA2 .bottom"
                                                                );
            for(let i=0; colAbottom[i]; i++){
                (colAbottom[i] as HTMLElement).style.height = hrow1Abottom + "px";
            }
//#endregion

//#region row2_ShiftVolPlan
        var col1Svp = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .shiftvolplan .svpName");

        for(let i=0; col1Svp[i]; i++){
            (col1Svp[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Svp[i] as HTMLElement).style.height = hrow2 + "px";
        }

        var col28Svp = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .shiftvolplan .svpW,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpT,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpA1,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpA2,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpAtotal,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpIns,"
                                                        + ".lineRealTimeControl .shiftvolplan .svpPdi"
                                                        );
        for(let i=0; col28Svp[i]; i++){
            (col28Svp[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Svp[i] as HTMLElement).style.height = hrow2 + "px";
        }
//#endregion

//#region row3_VolTarget
        var col1Vt = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .voltarget .vtName");

        for(let i=0; col1Vt[i]; i++){
            (col1Vt[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Vt[i] as HTMLElement).style.height = hrow3 + "px";
        }

        var col28Vt = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .voltarget .vtW,"
                                                        + ".lineRealTimeControl .voltarget .vtT,"
                                                        + ".lineRealTimeControl .voltarget .vtA1,"
                                                        + ".lineRealTimeControl .voltarget .vtA2,"
                                                        + ".lineRealTimeControl .voltarget .vtAtotal,"
                                                        + ".lineRealTimeControl .voltarget .vtIns,"
                                                        + ".lineRealTimeControl .voltarget .vtPdi"
                                                        );
        for(let i=0; col28Vt[i]; i++){
            (col28Vt[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Vt[i] as HTMLElement).style.height = hrow3 + "px";
        }
//#endregion

//#region row4_VolActual
        var col1Va = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .volactual .vaName");
        for(let i=0; col1Va[i]; i++){
            (col1Va[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Va[i] as HTMLElement).style.height = hrow4 + "px";
        }

        var col28Va = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .volactual .vaW,"
                                                        + ".lineRealTimeControl .volactual .vaT,"
                                                        + ".lineRealTimeControl .volactual .vaA1,"
                                                        + ".lineRealTimeControl .volactual .vaA2,"
                                                        + ".lineRealTimeControl .volactual .vaAtotal,"
                                                        + ".lineRealTimeControl .volactual .vaIns,"
                                                        + ".lineRealTimeControl .volactual .vaPdi"
                                                        );
        for(let i=0; col28Va[i]; i++){
            (col28Va[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Va[i] as HTMLElement).style.height = hrow4 + "px";
        }
//#endregion

//#region row5_VolBalance
        var col1Vb = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .volbalance .vbName");

        for(let i=0; col1Vb[i]; i++){
            (col1Vb[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Vb[i] as HTMLElement).style.height = hrow5 + "px";
        }

        var col28Vb = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .volbalance .vbW,"
                                                        + ".lineRealTimeControl .volbalance .vbT,"
                                                        + ".lineRealTimeControl .volbalance .vbA1,"
                                                        + ".lineRealTimeControl .volbalance .vbA2,"
                                                        + ".lineRealTimeControl .volbalance .vbAtotal,"
                                                        + ".lineRealTimeControl .volbalance .vbIns,"
                                                        + ".lineRealTimeControl .volbalance .vbPdi"
                                                        );
        for(let i=0; col28Vb[i]; i++){
            (col28Vb[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Vb[i] as HTMLElement).style.height = hrow5 + "px";
        }
//#endregion

//#region row6_StopTime
        var col1St = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .stoptime .stName");

        for(let i=0; col1St[i]; i++){
            (col1St[i] as HTMLElement).style.width = wcol1 + "px";
            (col1St[i] as HTMLElement).style.height = hrow6 + "px";
        }

        var col28St = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .stoptime .stW,"
                                                        + ".lineRealTimeControl .stoptime .stT,"
                                                        + ".lineRealTimeControl .stoptime .stA1,"
                                                        + ".lineRealTimeControl .stoptime .stA2,"
                                                        + ".lineRealTimeControl .stoptime .stAtotal,"
                                                        + ".lineRealTimeControl .stoptime .stIns,"
                                                        + ".lineRealTimeControl .stoptime .stPdi"
                                                        );
        for(let i=0; col28St[i]; i++){
            (col28St[i] as HTMLElement).style.width = wcol28 + "px";
            (col28St[i] as HTMLElement).style.height = hrow6 + "px";

        }
//#endregion

//#region row7_Efficiency
        var col1Eff = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .efficiency .effName");

        for(let i=0; col1Eff[i]; i++){
            (col1Eff[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Eff[i] as HTMLElement).style.height = hrow7 + "px";
        }

        var col28Eff = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .efficiency .effW,"
                                                        + ".lineRealTimeControl .efficiency .effT,"
                                                        + ".lineRealTimeControl .efficiency .effA1,"
                                                        + ".lineRealTimeControl .efficiency .effA2,"
                                                        + ".lineRealTimeControl .efficiency .effAtotal,"
                                                        + ".lineRealTimeControl .efficiency .effIns,"
                                                        + ".lineRealTimeControl .efficiency .effPdi"
                                                        );
        for(let i=0; col28Eff[i]; i++){
            (col28Eff[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Eff[i] as HTMLElement).style.height = hrow7 + "px";
        }
//#endregion

//#region row8_Takttime
        var col1Tt = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .talktime .ttName");

        for(let i=0; col1Tt[i]; i++){
            (col1Tt[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Tt[i] as HTMLElement).style.height = hrow8 + "px";
        }

        var col28Tt = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .talktime .ttW,"
                                                        + ".lineRealTimeControl .talktime .ttT,"
                                                        + ".lineRealTimeControl .talktime .ttA1,"
                                                        + ".lineRealTimeControl .talktime .ttA2,"
                                                        + ".lineRealTimeControl .talktime .ttAtotal,"
                                                        + ".lineRealTimeControl .talktime .ttIns,"
                                                        + ".lineRealTimeControl .talktime .ttPdi"
                                                        );
        for(let i=0; col28Tt[i]; i++){
            (col28Tt[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Tt[i] as HTMLElement).style.height = hrow8 + "px";
        }
//#endregion

//#region row9_Overtime
        var col1Ot = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .overtime .otName");

        for(let i=0; col1Ot[i]; i++){
            (col1Ot[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Ot[i] as HTMLElement).style.height = hrow9 + "px";
        }

        var col28Ot = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .overtime .otW,"
                                                        + ".lineRealTimeControl .overtime .otT,"
                                                        + ".lineRealTimeControl .overtime .otA1,"
                                                        + ".lineRealTimeControl .overtime .otA2,"
                                                        + ".lineRealTimeControl .overtime .otAtotal,"
                                                        + ".lineRealTimeControl .overtime .otIns,"
                                                        + ".lineRealTimeControl .overtime .otPdi"
                                                        );
        for(let i=0; col28Ot[i]; i++){
            (col28Ot[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Ot[i] as HTMLElement).style.height = hrow9 + "px";
        }
//#endregion

//#region row10_Non-prod activity
        var col1Npa = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .nonprodact .npaName");


        for(let i=0; col1Npa[i]; i++){
            (col1Npa[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Npa[i] as HTMLElement).style.height = hrow10 + "px";
        }

        var col28Npa = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .nonprodact .npaW,"
                                                        + ".lineRealTimeControl .nonprodact .npaT,"
                                                        + ".lineRealTimeControl .nonprodact .npaA1,"
                                                        + ".lineRealTimeControl .nonprodact .npaA2,"
                                                        + ".lineRealTimeControl .nonprodact .npaAtotal,"
                                                        + ".lineRealTimeControl .nonprodact .npaIns,"
                                                        + ".lineRealTimeControl .nonprodact .npaPdi"
                                                        );
        for(let i=0; col28Npa[i]; i++){
            (col28Npa[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Npa[i] as HTMLElement).style.height = hrow10 + "px";
        }
//#endregion

//#region row11_OffLine
        var col1Off = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offName");


        for(let i=0; col1Off[i]; i++){
            (col1Off[i] as HTMLElement).style.width = wcol1 + "px";
            (col1Off[i] as HTMLElement).style.height = hrow11 + "px";
        }

        var col28Off = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offW,"
                                                        + ".lineRealTimeControl .offline .offT,"
                                                        + ".lineRealTimeControl .offline .offA1,"
                                                        + ".lineRealTimeControl .offline .offA2,"
                                                        + ".lineRealTimeControl .offline .offAtotal,"
                                                        + ".lineRealTimeControl .offline .offIns"
                                                        );
        for(let i=0; col28Off[i]; i++){
            (col28Off[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Off[i] as HTMLElement).style.height = hrow11 + "px";
        }

        var col28Off = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offPdi"
                                                                );
            for(let i=0; col28Off[i]; i++){
            (col28Off[i] as HTMLElement).style.width = wcol28 + "px";
            (col28Off[i] as HTMLElement).style.height = hrow11 + "px";
            }

        var colOff = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offW .offWx2,"
                                                            + ".lineRealTimeControl .offline .offIns .offInsx2"
                                                            );
            for(let i=0; colOff[i]; i++){
            (colOff[i] as HTMLElement).style.height = hrow11a + "px";
        }

        var col28tille = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offW .offWx2 .vectron .vectron_title,"
                                                                + ".lineRealTimeControl .offline .offW .offWx2 .wrepair .wrepair_title,"
                                                                + ".lineRealTimeControl .offline .offIns .offInsx2 .samp .samp_title,"
                                                                + ".lineRealTimeControl .offline .offIns .offInsx2 .audit .audit_title,"
                                                                + ".lineRealTimeControl .offline .offT .offT_title,"
                                                                + ".lineRealTimeControl .offline .offA1 .offA1_title,"
                                                                + ".lineRealTimeControl .offline .offA2 .offA2_title,"
                                                                + ".lineRealTimeControl .offline .offAtotal .offAtotal_title"
                                                                );
            for(let i=0; col28tille[i]; i++){
            (col28tille[i] as HTMLElement).style.height = hrow11a_title + "px";
            }

            var col28value = document.querySelectorAll<HTMLElement>(".lineRealTimeControl .offline .offW .offWx2 .vectron .vectron_value,"
                                                                    + ".lineRealTimeControl .offline .offW .offWx2 .wrepair .wrepair_value,"
                                                                    + ".lineRealTimeControl .offline .offIns .offInsx2 .samp .samp_value,"
                                                                    + ".lineRealTimeControl .offline .offIns .offInsx2 .audit .audit_value,"
                                                                    + ".lineRealTimeControl .offline .offT .offT_value,"
                                                                    + ".lineRealTimeControl .offline .offA1 .offA1_value,"
                                                                    + ".lineRealTimeControl .offline .offA2 .offA2_value,"
                                                                    + ".lineRealTimeControl .offline .offAtotal .offAtotal_value"
                                                                    );
            for(let i=0; col28value[i]; i++){
            (col28value[i] as HTMLElement).style.height = hrow11a_value + "px";
            }


//#endregion

}

    bindDataLineRealTimeControl(){

            //bind data
            this.dataLineRealTime.forEach(element => {

                    var o = (element as GetDetailsOutput);
//#region row1_Line
                    //workingdate
                    let wkdobj = document.getElementById("wkDate");
                    if ( wkdobj!= null  && o.line != null){
                        wkdobj.innerHTML = this.pipe.transform(o.workingDate, 'd - MMM (h:mm)');
                    }

                    //lineW
                    let r1_lineW_obj = document.getElementById("lineW");
                    if ( r1_lineW_obj!= null && o.line != null && o.line == "W"){
                        r1_lineW_obj.innerHTML = o.line.toString();
                    }

                    //lineT
                    let r1_lineT_obj = document.getElementById("lineT");
                    if ( r1_lineT_obj!= null && o.line != null && o.line == "T"){
                        r1_lineT_obj.innerHTML = o.line.toString();
                    }

                    //lineA1
                    let r1_lineA1_obj = document.getElementById("lineA1");
                    if ( r1_lineA1_obj!= null && o.line != null && o.line == "A1"){
                        r1_lineA1_obj.innerHTML = o.line.toString();
                    }

                    //lineA2
                    let r1_lineA2_obj = document.getElementById("lineA2");
                    if ( r1_lineA2_obj!= null && o.line != null && o.line == "A2"){
                        r1_lineA2_obj.innerHTML = o.line.toString();
                    }

                     //lineAtotal
                     let r1_lineAtotal_obj = document.getElementById("lineAtotal");
                     if ( r1_lineAtotal_obj!= null && o.line != null && o.line == "A"){
                        r1_lineAtotal_obj.innerHTML = "A Total";
                     }

                      //lineIns
                    let r1_lineIns_obj = document.getElementById("lineIns");
                    if ( r1_lineIns_obj!= null && o.line != null && o.line == "I"){
                        r1_lineIns_obj.innerHTML = o.line.toString() + "NS.";
                    }

                      //linePdi
                      let r1_linePdi_obj = document.getElementById("linePdi");
                      if ( r1_linePdi_obj!= null && o.line != null && o.line == "P"){
                        r1_linePdi_obj.innerHTML = o.line.toString()+"DI";
                      }
//#endregion

//#region row2_ShiftVolPlan
                    //name ShiftVolPlan
                    let svpNameobj = document.getElementById("svpName");
                    if ( svpNameobj!= null ){
                        svpNameobj.innerHTML =  "Shift Vol.plan";
                    }

                    //ShiftVolPlan_W
                    let r2_svpW_obj = document.getElementById("svpW");
                    if ( r2_svpW_obj!= null  && o.shiftVolPlan != null && o.line == "W"){
                        r2_svpW_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpW_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_T
                    let r2_svpT_obj = document.getElementById("svpT");
                    if ( r2_svpT_obj!= null  && o.shiftVolPlan != null && o.line == "T"){
                        r2_svpT_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpT_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_A1
                    let r2_svpA1_obj = document.getElementById("svpA1");
                    if ( r2_svpA1_obj!= null  && o.shiftVolPlan != null && o.line == "A1"){
                        r2_svpA1_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpA1_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_A2
                    let r2_svpA2_obj = document.getElementById("svpA2");
                    if ( r2_svpA2_obj!= null  && o.shiftVolPlan != null && o.line == "A2"){
                        r2_svpA2_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpA2_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_Atotal
                    let r2_svpAtotal_obj = document.getElementById("svpAtotal");
                    if ( r2_svpAtotal_obj!= null  && o.shiftVolPlan != null && o.line == "A"){
                        r2_svpAtotal_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpAtotal_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_Ins
                    let r2_svpIns_obj = document.getElementById("svpIns");
                    if ( r2_svpIns_obj!= null  && o.shiftVolPlan != null && o.line == "I"){
                        r2_svpIns_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpIns_obj.style.color = "red";
                        }
                    }

                    //ShiftVolPlan_Pdi
                    let r2_svpPdi_obj = document.getElementById("svpPdi");
                    if ( r2_svpPdi_obj!= null  && o.shiftVolPlan != null && o.line == "P"){
                        r2_svpPdi_obj.innerHTML = o.shiftVolPlan.toString();
                        if(o.shiftVolPlan < 0){
                            r2_svpPdi_obj.style.color = "red";
                        }
                    }
//#endregion

//#region row3_VolTarget
                    //name ShiftVolPlan
                    let vtNameobj = document.getElementById("vtName");
                    if ( vtNameobj!= null ){
                        vtNameobj.innerHTML =  "Vol.Target(98%)";
                      vtNameobj.setAttribute("voltarget", o.shift.toString());
                    }

                    //ShiftVolPlan_W
                    let r3_vtW_obj = document.getElementById("vtW");
                    if ( r3_vtW_obj!= null && o.volTarget != null && o.line == "W"){
                        r3_vtW_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_T
                    let r3_vtT_obj = document.getElementById("vtT");
                    if ( r3_vtT_obj!= null && o.volTarget != null && o.line == "T"){
                        r3_vtT_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_A1
                    let r3_vtA1_obj = document.getElementById("vtA1");
                    if ( r3_vtA1_obj!= null && o.volTarget != null && o.line == "A1"){
                        r3_vtA1_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_A2
                    let r3_vtA2_obj = document.getElementById("vtA2");
                    if ( r3_vtA2_obj!= null && o.volTarget != null && o.line == "A2"){
                        r3_vtA2_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_Atotal
                    let r3_vtAtotal_obj = document.getElementById("vtAtotal");
                    if ( r3_vtAtotal_obj!= null && o.volTarget != null && o.line == "A"){
                        r3_vtAtotal_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_Ins
                    let r3_vtIns_obj = document.getElementById("vtIns");
                    if ( r3_vtIns_obj!= null && o.volTarget != null && o.line == "I"){
                        r3_vtIns_obj.innerHTML = o.volTarget.toString();
                    }

                    //ShiftVolPlan_Pdi
                    let r3_vtPdi_obj = document.getElementById("vtPdi");
                    if ( r3_vtPdi_obj!= null && o.line == "P"){
                        r3_vtPdi_obj.innerHTML = o.volTarget.toString();
                    }
//#endregion

//#region row4_VolActual
                    //name VolActual
                    let vaNameobj = document.getElementById("vaName");
                    if ( vaNameobj!= null){
                        vaNameobj.innerHTML =  "Actual";
                    }

                    //VolActual_W
                    let r4_vaW_obj = document.getElementById("vaW");
                    if ( r4_vaW_obj!= null && o.volActual != null && o.line == "W"){
                        r4_vaW_obj.innerHTML = o.volActual.toString();
                    }

                    //VolActual_T
                    let r4_vaT_obj = document.getElementById("vaT");
                    if ( r4_vaT_obj!= null && o.volActual != null && o.line == "T"){
                        r4_vaT_obj.innerHTML = o.volActual.toString();
                    }

                    //VolActual_A1
                    let r4_vaA1_obj = document.getElementById("vaA1");
                    if ( r4_vaA1_obj!= null && o.volActual != null && o.line == "A1"){
                        r4_vaA1_obj.innerHTML = o.volActual.toString();
                    }

                     //VolActual_A2
                    let r4_vaA2_obj = document.getElementById("vaA2");
                    if ( r4_vaA2_obj!= null && o.volActual != null && o.line == "A2"){
                        r4_vaA2_obj.innerHTML = o.volActual.toString();
                    }

                    //VolActual_Atotal
                    let r4_vaAtotal_obj = document.getElementById("vaAtotal");
                    if ( r4_vaAtotal_obj!= null && o.volActual != null && o.line == "A"){
                        r4_vaAtotal_obj.innerHTML = o.volActual.toString();
                    }

                    //VolActual_Ins
                    let r4_vaIns_obj = document.getElementById("vaIns");
                    if ( r4_vaIns_obj!= null && o.volActual != null && o.line == "I"){
                        r4_vaIns_obj.innerHTML = o.volActual.toString();
                    }

                    //VolActual_Pdi
                    let r4_vaPdi_obj = document.getElementById("vaPdi");
                    if ( r4_vaPdi_obj!= null && o.volActual != null && o.line == "P"){
                        r4_vaPdi_obj.innerHTML = o.volActual.toString();
                    }
//#endregion

//#region row5_VolBalance
                    //name VolBalance
                    let vbNameobj = document.getElementById("vbName");
                    if ( vbNameobj!= null ){
                        vbNameobj.innerHTML =  "Balance";
                    }

                    //VolBalance_W
                    let r5_vbW_obj = document.getElementById("vbW");
                    if ( r5_vbW_obj!= null && o.volBalance != null && o.line == "W"){
                        r5_vbW_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbW_obj.style.color = "red";
                        }
                    }

                    //VolBalance_T
                    let r5_vbT_obj = document.getElementById("vbT");
                    if ( r5_vbT_obj!= null && o.volBalance != null && o.line == "T"){
                        r5_vbT_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbT_obj.style.color = "red";
                        }
                    }

                    //VolBalance_A1
                    let r5_vbA1_obj = document.getElementById("vbA1");
                    if ( r5_vbA1_obj!= null && o.volBalance != null && o.line == "A1"){
                        r5_vbA1_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbA1_obj.style.color = "red";
                        }
                    }

                    //VolBalance_A2
                    let r5_vbA2_obj = document.getElementById("vbA2");
                    if ( r5_vbA2_obj!= null && o.volBalance != null && o.line == "A2"){
                        r5_vbA2_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbA2_obj.style.color = "red";
                        }
                    }

                    //VolBalance_Atotal
                    let r5_vbAtotal_obj = document.getElementById("vbAtotal");
                    if ( r5_vbAtotal_obj!= null && o.volBalance != null && o.line == "A"){
                        r5_vbAtotal_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbAtotal_obj.style.color = "red";
                        }
                    }

                    //VolBalance_Ins
                    let r5_vbIns_obj = document.getElementById("vbIns");
                    if ( r5_vbIns_obj!= null && o.volBalance != null && o.line == "I"){
                        r5_vbIns_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbIns_obj.style.color = "red";
                        }
                    }

                    //VolBalance_Pdi
                    let r5_vbPdi_obj = document.getElementById("vbPdi");
                    if ( r5_vbPdi_obj!= null && o.volBalance != null && o.line == "P"){
                        r5_vbPdi_obj.innerHTML = o.volBalance.toString();
                        if(o.volBalance < 0){
                            r5_vbPdi_obj.style.color = "red";
                        }
                    }
//#endregion

//#region row6_StopTime
                    //name StopTime
                    let stNameobj = document.getElementById("stName");
                    if ( stNameobj!= null ){
                        stNameobj.innerHTML =  "Stop time (Min.)";
                    }

                    //StopTime_W
                    let r5_stW_obj = document.getElementById("stW");
                    if ( r5_stW_obj!= null && o.stopTime != null && o.line == "W"){
                        r5_stW_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stW_obj.style.color = "red";
                        }

                    }

                    //StopTime_T
                    let r5_stT_obj = document.getElementById("stT");
                    if ( r5_stT_obj!= null && o.stopTime != null && o.line == "T"){
                        r5_stT_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stT_obj.style.color = "red";
                        }
                    }

                    //StopTime_A1
                    let r5_stA1_obj = document.getElementById("stA1");
                    if ( r5_stA1_obj!= null && o.stopTime != null && o.line == "A1"){
                        r5_stA1_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stA1_obj.style.color = "red";
                        }
                    }

                    //StopTime_A2
                    let r5_stA2_obj = document.getElementById("stA2");
                    if ( r5_stA2_obj!= null && o.stopTime != null && o.line == "A2"){
                        r5_stA2_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stA2_obj.style.color = "red";
                        }
                    }

                    //StopTime_Atotal
                    let r5_stAtotal_obj = document.getElementById("stAtotal");
                    if ( r5_stAtotal_obj!= null && o.stopTime != null && o.line == "A"){
                        r5_stAtotal_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stAtotal_obj.style.color = "red";
                        }
                    }

                    //StopTime_Ins
                    let r5_stIns_obj = document.getElementById("stIns");
                    if ( r5_stIns_obj!= null && o.stopTime != null && o.line == "I"){
                        r5_stIns_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stIns_obj.style.color = "red";
                        }
                    }

                    //StopTime_Pdi
                    let r5_stPdi_obj = document.getElementById("stPdi");
                    if ( r5_stPdi_obj!= null && o.stopTime != null && o.line == "P"){
                        r5_stPdi_obj.innerHTML = o.stopTime.toString();
                        if(o.stopTime > 10){
                            r5_stPdi_obj.style.color = "red";
                        }
                    }
//#endregion

//#region row7_Efficiency
                    //name Efficiency
                    let effNameobj = document.getElementById("effName");
                    if ( effNameobj!= null ){
                        effNameobj.innerHTML =  "Efficiency (%)";

                    }

                    //Efficiency_W
                    let r5_effW_obj = document.getElementById("effW");
                    if ( r5_effW_obj!= null && o.efficiency != null && o.line == "W"){
                        r5_effW_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_T
                    let r5_effT_obj = document.getElementById("effT");
                    if ( r5_effT_obj!= null && o.efficiency != null && o.line == "T"){
                        r5_effT_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_A1
                    let r5_effA1_obj = document.getElementById("effA1");
                    if ( r5_effA1_obj!= null && o.efficiency != null && o.line == "A1"){
                        r5_effA1_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_A2
                    let r5_effA2_obj = document.getElementById("effA2");
                    if ( r5_effA2_obj!= null && o.efficiency != null && o.line == "A2"){
                        r5_effA2_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_Atotal
                    let r5_effAtotal_obj = document.getElementById("effAtotal");
                    if ( r5_effAtotal_obj!= null && o.efficiency != null && o.line == "A"){
                        r5_effAtotal_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_Ins
                    let r5_effIns_obj = document.getElementById("effIns");
                    if ( r5_effIns_obj!= null && o.efficiency != null && o.line == "I"){
                        r5_effIns_obj.innerHTML = o.efficiency.toString();

                    }

                    //Efficiency_Pdi
                    let r5_effPdi_obj = document.getElementById("effPdi");
                    if ( r5_effPdi_obj!= null && o.efficiency != null && o.line == "P"){
                        r5_effPdi_obj.innerHTML = o.efficiency.toString();

                    }
//#endregion

//#region row8_Takttime
                    //name Takttime
                    let ttNameobj = document.getElementById("ttName");
                    if ( ttNameobj!= null ){
                        ttNameobj.innerHTML =  "Takt time (Min.)";

                    }

                    //Takttime_W
                    let r5_ttW_obj = document.getElementById("ttW");
                    if ( r5_ttW_obj!= null && o.taktTime != null && o.line == "W"){
                        r5_ttW_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_T
                    let r5_ttT_obj = document.getElementById("ttT");
                    if ( r5_ttT_obj!= null && o.taktTime != null && o.line == "T"){
                        r5_ttT_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_A1
                    let r5_ttA1_obj = document.getElementById("ttA1");
                    if ( r5_ttA1_obj!= null && o.taktTime != null && o.line == "A1"){
                        r5_ttA1_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_A2
                    let r5_ttA2_obj = document.getElementById("ttA2");
                    if ( r5_ttA2_obj!= null && o.taktTime != null && o.line == "A2"){
                        r5_ttA2_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_Atotal
                    let r5_ttAtotal_obj = document.getElementById("ttAtotal");
                    if ( r5_ttAtotal_obj!= null && o.taktTime != null && o.line == "A"){
                        r5_ttAtotal_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_Ins
                    let r5_ttIns_obj = document.getElementById("ttIns");
                    if ( r5_ttIns_obj!= null && o.taktTime != null && o.line == "I"){
                        r5_ttIns_obj.innerHTML = o.taktTime.toString();

                    }

                    //Takttime_Pdi
                    let r5_ttPdi_obj = document.getElementById("ttPdi");
                    if ( r5_ttPdi_obj!= null && o.taktTime != null && o.line == "P"){
                        r5_ttPdi_obj.innerHTML = o.taktTime.toString();

                    }
//#endregion

//#region row9_Overtime
                    //name Overtime
                    let otNameobj = document.getElementById("otName");
                    if ( otNameobj!= null ){
                        otNameobj.innerHTML =  "Overtime (Hr.)";

                    }

                    //Overtime_W
                    let r5_otW_obj = document.getElementById("otW");
                    if ( r5_otW_obj!= null && o.overtime != null && o.line == "W"){
                        r5_otW_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_T
                    let r5_otT_obj = document.getElementById("otT");
                    if ( r5_otT_obj!= null && o.overtime != null && o.line == "T"){
                        r5_otT_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_A1
                    let r5_otA1_obj = document.getElementById("otA1");
                    if ( r5_otA1_obj!= null && o.overtime != null && o.line == "A1"){
                        r5_otA1_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_A2
                    let r5_otA2_obj = document.getElementById("otA2");
                    if ( r5_otA2_obj!= null && o.overtime != null && o.line == "A2"){
                        r5_otA2_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_Atotal
                    let r5_otAtotal_obj = document.getElementById("otAtotal");
                    if ( r5_otAtotal_obj!= null && o.overtime != null && o.line == "A"){
                        r5_otAtotal_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_Ins
                    let r5_otIns_obj = document.getElementById("otIns");
                    if ( r5_otIns_obj!= null && o.overtime != null && o.line == "I"){
                        r5_otIns_obj.innerHTML = o.overtime.toString();

                    }

                    //Overtime_Pdi
                    let r5_otPdi_obj = document.getElementById("otPdi");
                    if ( r5_otPdi_obj!= null && o.overtime != null && o.line == "P"){
                        r5_otPdi_obj.innerHTML = o.overtime.toString();

                    }
//#endregion

//#region row10_Non_Product_Action
                    //name Non_Product_Action
                    let npaNameobj = document.getElementById("npaName");
                    if ( npaNameobj!= null ){
                        npaNameobj.innerHTML =  "Non-prod activity(Hr.)";
                    }

                    //Non_Product_Action_W
                    let r10_npaW_obj = document.getElementById("npaW");
                    if ( r10_npaW_obj!= null && o.nonProdAct != null && o.line == "W"){
                        r10_npaW_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Non_Product_Action_T
                    let r10_npaT_obj = document.getElementById("npaT");
                    if ( r10_npaT_obj!= null && o.nonProdAct != null && o.line == "T"){
                        r10_npaT_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Overtime_A1
                    let r10_npaA1_obj = document.getElementById("npaA1");
                    if ( r10_npaA1_obj!= null && o.nonProdAct != null && o.line == "A1"){
                        r10_npaA1_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Overtime_A2
                    let r10_npaA2_obj = document.getElementById("npaA2");
                    if ( r10_npaA2_obj!= null && o.nonProdAct != null && o.line == "A2"){
                        r10_npaA2_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Overtime_Atnpaal
                    let r10_otAtotal_obj = document.getElementById("otAtotal");
                    if ( r10_otAtotal_obj!= null && o.nonProdAct != null && o.line == "A"){
                        r10_otAtotal_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Overtime_Ins
                    let r10_npaIns_obj = document.getElementById("npaIns");
                    if ( r10_npaIns_obj!= null && o.nonProdAct != null && o.line == "I"){
                        r10_npaIns_obj.innerHTML = o.nonProdAct.toString();

                    }

                    //Overtime_Pdi
                    let r10_npaPdi_obj = document.getElementById("npaPdi");
                    if ( r10_npaPdi_obj!= null && o.nonProdAct != null && o.line == "P"){
                        r10_npaPdi_obj.innerHTML = o.nonProdAct.toString();

                    }
//#endregion

//#region row11_OffLine
                    //name OffLine
                    let offNameobj = document.getElementById("offName");
                    if ( offNameobj!= null ){
                        offNameobj.innerHTML =  "Off-Line";
                    }

                    //OffLine_W
                    let r11_vectron_obj = document.getElementById("vectron");
                    if ( r11_vectron_obj!= null && o.offLine1 != null && o.line == "W"){
                        r11_vectron_obj.innerHTML = o.offLine1.toString();

                    }

                    //OffLine_W
                    let r11_wrepair_obj = document.getElementById("wrepair");
                    if ( r11_wrepair_obj!= null && o.offLine2 != null && o.line == "W"){
                        r11_wrepair_obj.innerHTML = o.offLine2.toString();
                        if(o.offLine2 > 0){
                            document.getElementById("wrepair").style.color = "red";
                        }
                    }

                      //OffLine_T
                    let r11_offT_obj = document.getElementById("offT");
                    if ( r11_offT_obj!= null && o.offLine3 != null && o.line == "T"){
                        r11_offT_obj.innerHTML = o.offLine3.toString();
                        if(o.offLine3 > 0){
                            document.getElementById("offT").style.color = "red";
                        }
                    }

                      //OffLine_A1
                    let r11_offA1_obj = document.getElementById("offA1");
                    if ( r11_offA1_obj!= null && o.offLine4 != null && o.line == "A1"){
                        r11_offA1_obj.innerHTML = o.offLine4.toString();
                        if(o.offLine4 > 0){
                            document.getElementById("offA1").style.color = "red";
                        }
                    }

                      //OffLine_A2
                    let r11_offA2_obj = document.getElementById("offA2");
                    if ( r11_offA2_obj!= null && o.offLine5 != null && o.line == "A2"){
                        r11_offA2_obj.innerHTML = o.offLine5.toString();
                        if(o.offLine5 > 0){
                            document.getElementById("offA2").style.color = "red";
                        }
                    }

                      //OffLine_Atoffal
                    let r11_otAtotal_obj = document.getElementById("offAtotal");
                    if ( r11_otAtotal_obj!= null && o.offLine6 != null && o.line == "A"){
                        r11_otAtotal_obj.innerHTML = o.offLine6.toString();
                        if(o.offLine6 > 0){
                            document.getElementById("offAtotal").style.color = "red";
                        }
                    }

                     //OffLine_Ins
                    let r11_samp_obj = document.getElementById("samp");
                    if ( r11_samp_obj!= null && o.offLine7 != null && o.line == "I"){
                        r11_samp_obj.innerHTML = o.offLine7.toString();
                        if(o.offLine7 > 0){
                            document.getElementById("samp").style.color = "red";
                        }
                    }

                      //OffLine_Ins
                    let r11_audit_obj = document.getElementById("audit");
                    if ( r11_audit_obj!= null && o.offLine8 != null && o.line == "I"){
                        r11_audit_obj.innerHTML = o.offLine8.toString();
                        if(o.offLine8 > 0){
                            document.getElementById("audit").style.color = "red";
                        }
                    }
//#endregion


        });
    }
}
