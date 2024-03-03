import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { GridParams, FrameworkComponent } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwPupPxPUpPlanDto, LgwPupPxPUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    templateUrl: './module-unpacking-andon-screen.component.html',
    styleUrls: ['./module-unpacking-andon-screen.component.less'],
})
export class ModuleUnpackingAndonComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    selectedRow: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    saveSelectedRow: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    datas: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataUnpAdo: any[] = [];
    clearTimeLoadData;
    fn:CommonFunction = new CommonFunction();

    constructor(
        injector: Injector,
        private _service: LgwPupPxPUpPlanServiceProxy,
    ) {
        super(injector);
    }

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
            this.fn.showtime('time_now_log');

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    getData() {
        this._service.getPxpUnpackPlan().pipe(finalize(()=> {}))
        .subscribe((result) => {
            try{
                this.dataUnpAdo = result.items ?? [];
                this.loadData();

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);

        } catch (ex) {
            console.log('2: ' + ex);

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

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }



    loadForm(){
        console.log('loadform');

        let w = window.innerWidth;
        var h = window.innerHeight;

        let cssBody = document.querySelectorAll<HTMLElement>('.body');
        for (let i = 0; cssBody[i]; i++) {
            cssBody[i].style.width = w + "px";
            cssBody[i].style.height = h + "px";
        }

    }
    loadData(){
        console.log('loaddata');

        let w = window.innerWidth;
        var h = window.innerHeight;

        let htm_Module_W1 = document.querySelector('.body .rightUnpAdo .rW1 .rW1Bottom');
        let htm_Module_W2 = document.querySelector('.body .rightUnpAdo .rW2 .rW2Bottom');
        let htm_Module_W3 = document.querySelector('.body .rightUnpAdo .rW3 .rW3Bottom');
        var dataW1 = this.dataUnpAdo.filter(d=>d.prodLine === 'W1').filter(e=>e.isPreviousShiftDelay == 'N')
        var dataW2 = this.dataUnpAdo.filter(d=>d.prodLine === 'W2').filter(e=>e.isPreviousShiftDelay == 'N')
        var dataW3 = this.dataUnpAdo.filter(d=>d.prodLine === 'W3').filter(e=>e.isPreviousShiftDelay == 'N')

        var countW1 = dataW1.length;
        var countW2 = dataW2.length;
        var countW3 = dataW3.length;
        var htm1 = "";
        var htm2 = "";
        var htm3 = "";
        var lctOld1 = 0;
        var lctOld2 = 0;
        var lctOld3 = 0;

        //W1
        if(countW1 > 0){
            var totalBlock1 = dataW1[0].totalBlock;
            var planVol1 = dataW1[0].planVol;
            var actualVol1 = dataW1[0].actualVol;
            var wBlock1 = (w*90/100)/totalBlock1

            let htmBlock1 = document.querySelector('.body .rightUnpAdo .rW1 .rW1Top');
            var htmB1 = "";
                for (var i = 0; i < totalBlock1; i++) {
                    if(i < actualVol1)
                        {
                            var htmB1 = htmB1 + '<div style="height: 100%;background-color:#00B050;border-right:1px solid black;width:' + wBlock1 + 'px' + '"></div>';
                        }
                    if(actualVol1 <=i && i < planVol1)
                        {
                            var htmB1 = htmB1 + '<div style="height: 100%;background-color:#FFFF00;border-right:1px solid black;width:' + wBlock1 + 'px' + '"></div>';
                        }
                    if(planVol1 < actualVol1 && actualVol1 <= i && i < totalBlock1)
                        {
                            var htmB1 = htmB1 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock1 + 'px' + '"></div>';
                        }
                    else if(planVol1 > actualVol1 && planVol1 <=i && i < totalBlock1)
                        {
                            var htmB1 = htmB1 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock1 + 'px' + '"></div>';
                        }
                }
                htmBlock1.innerHTML = htmB1;

                for (var i = 0; i < countW1; i++) {

                    var lt = (dataW1[i].leadTime/dataW1[i].taktTime)*wBlock1
                    var bg_status = "white";
                    var lctStUnp = ((dataW1[i].seqLineIn - 1) * wBlock1) - lctOld1 - 1;
                    lctOld1 = lt + lctStUnp + lctOld1;
                    var v_caseNo = dataW1[i].caseNo.substring(0,2) + dataW1[i].caseNo.substring((dataW1[i].caseNo.length-2),dataW1[i].caseNo.length);

                    if(dataW1[i].isCalling == "Y"){
                        bg_status = "#F7D7F7"
                    }
                    else if(dataW1[i].isStarted == "Y"){
                        bg_status = "#C5D9F1"
                    }
                    else if(dataW1[i].isFinished == "Y"){
                        bg_status = "#92D14F"
                    }
                    else if(dataW1[i].isDelayed == "Y"){
                        bg_status = "#ED2D41"
                    }
                    var htm1 = htm1 + '<div style="height: 100%;border-left:1px solid black;border-right:1px solid black;display: flex;'
                                    +'align-items: center;justify-content: center;font-weight: bold;background-color: ' + bg_status + ';margin-left:' + lctStUnp + 'px'+';width:' + lt + 'px' + '">'
                                    +'<div style="transform: rotate(270deg);font-size: 30px;font-weight: bold;">' + v_caseNo + '</div></div>';
                }
                htm_Module_W1.innerHTML = htm1;
        }

         //W2
         if(countW2 > 0){
            var totalBlock2 = dataW2[0].totalBlock;
            var planVol2 = dataW2[0].planVol;
            var actualVol2 = dataW2[0].actualVol;
            var wBlock2 = (w*90/100)/totalBlock2

            let htmBlock2 = document.querySelector('.body .rightUnpAdo .rW2 .rW2Top');
            var htmB2 = "";

                for (var i = 0; i < totalBlock2; i++) {
                    if(i < actualVol2)
                        {
                            var htmB2 = htmB2 + '<div style="height: 100%;background-color:#00B050;border-right:1px solid black;width:' + wBlock2 + 'px' + '"></div>';
                        }
                    if(actualVol2 <=i && i < planVol2)
                        {
                            var htmB2 = htmB2 + '<div style="height: 100%;background-color:#FFFF00;border-right:1px solid black;width:' + wBlock2 + 'px' + '"></div>';
                        }
                    if(planVol2 < actualVol2 && actualVol2 <= i && i < totalBlock2)
                        {
                            var htmB2 = htmB2 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock2 + 'px' + '"></div>';
                        }
                    else if(planVol2 > actualVol2 && planVol2 <=i && i < totalBlock2)
                        {
                            var htmB2 = htmB2 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock2 + 'px' + '"></div>';
                        }
                }
                htmBlock2.innerHTML = htmB2;


                for (var i = 0; i < countW2; i++) {

                    var lt = (dataW2[i].leadTime/dataW2[i].taktTime)*wBlock2
                    var bg_status = "white";
                    var lctStUnp = ((dataW2[i].seqLineIn - 1) * wBlock2) - lctOld2 - 1;
                    lctOld2 = lt + lctStUnp + lctOld2;
                    var v_caseNo = dataW2[i].caseNo.substring(0,2) + dataW2[i].caseNo.substring((dataW2[i].caseNo.length-2),dataW2[i].caseNo.length);

                    if(dataW2[i].isCalling == "Y"){
                        bg_status = "#F7D7F7"
                    }
                    else if(dataW2[i].isStarted == "Y"){
                        bg_status = "#C5D9F1"
                    }
                    else if(dataW2[i].isFinished == "Y"){
                        bg_status = "#92D14F"
                    }
                    else if(dataW2[i].isDelayed == "Y"){
                        bg_status = "#ED2D41"
                    }
                    var htm2 = htm2 + '<div style="height: 100%;border-left:1px solid black;border-right:1px solid black;display: flex;'
                                    +'align-items: center;'+'justify-content: center;font-weight: bold;background-color: ' + bg_status + ';margin-left:' + lctStUnp + 'px'+';width:' + lt + 'px' + '">'
                                    +'<div style="transform: rotate(270deg);font-size: 30px;font-weight: bold;">' + v_caseNo + '</div></div>';
                }
                htm_Module_W2.innerHTML = htm2;
        }

         //W3
         if(countW3 > 0){
            var totalBlock3 = dataW3[0].totalBlock;
            var planVol3 = dataW3[0].planVol;
            var actualVol3 = dataW3[0].actualVol;
            var wBlock3 = (w*90/100)/totalBlock3

            let htmBlock3 = document.querySelector('.body .rightUnpAdo .rW3 .rW3Top');
            var htmB3 = "";

                for (var i = 0; i < totalBlock3; i++) {
                    if(i < actualVol3)
                        {
                            var htmB3 = htmB3 + '<div style="height: 100%;background-color:#00B050;border-right:1px solid black;width:' + wBlock3 + 'px' + '"></div>';
                        }
                    if(actualVol3 <=i && i < planVol3)
                        {
                            var htmB3 = htmB3 + '<div style="height: 100%;background-color:#FFFF00;border-right:1px solid black;width:' + wBlock3 + 'px' + '"></div>';
                        }
                    if(planVol3 < actualVol3 && actualVol3 <= i && i < totalBlock3)
                        {
                            var htmB3 = htmB3 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock3 + 'px' + '"></div>';
                        }
                    else if(planVol3 > actualVol3 && planVol3 <=i && i < totalBlock3)
                        {
                            var htmB3 = htmB3 + '<div style="height: 100%;background-color:#FFFFFF;border-right:1px solid black;width:' + wBlock3 + 'px' + '"></div>';
                        }
                }
                htmBlock3.innerHTML = htmB3;


                for (var i = 0; i < countW3; i++) {

                    var lt = (dataW3[i].leadTime/dataW3[i].taktTime)*wBlock3
                    var bg_status = "white";
                    var lctStUnp = ((dataW3[i].seqLineIn - 1) * wBlock3) - lctOld3 - 1;
                    lctOld3 = lt + lctStUnp + lctOld3;
                    var v_caseNo = dataW3[i].caseNo.substring(0,2) + dataW3[i].caseNo.substring((dataW3[i].caseNo.length-2),dataW3[i].caseNo.length);

                    if(dataW3[i].isCalling == "Y"){
                        bg_status = "#F7D7F7"
                    }
                    else if(dataW3[i].isStarted == "Y"){
                        bg_status = "#C5D9F1"
                    }
                    else if(dataW3[i].isFinished == "Y"){
                        bg_status = "#92D14F"
                    }
                    else if(dataW3[i].isDelayed == "Y"){
                        bg_status = "#ED2D41"
                    }
                    var htm3 = htm3 + '<div style="height: 100%;border-left:1px solid black;border-right:1px solid black;display: flex;'
                                    +'align-items: center;justify-content: center;font-weight: bold;background-color: ' + bg_status + ';margin-left:' + lctStUnp + 'px'+';width:' + lt + 'px' + '">'
                                    +'<div style="transform: rotate(270deg);font-size: 30px;font-weight: bold;">' + v_caseNo + '</div></div>';
                }
                htm_Module_W3.innerHTML = htm3;
        }


        //////
        var dataPsDelay = this.dataUnpAdo.filter(d=>d.isPreviousShiftDelay === 'Y')
        var countPsDelay = dataPsDelay.length;
        let htm_NotFinnish = document.querySelector('.body .rightUnpAdo .rNotFinnish');
        if(countPsDelay > 0){
        var wLtDelay = (w*90/100)/20


        var htmPsDelay = "";
            for (var i = 0; i < countPsDelay; i++) {
                var bg_status = "white";
                var lctDelay = wLtDelay * i - 1

                if(dataPsDelay[i].isCalling == "Y"){
                    bg_status = "#F7D7F7"
                }
                else if(dataPsDelay[i].isStarted == "Y"){
                    bg_status = "#C5D9F1"
                }
                else if(dataPsDelay[i].isFinished == "Y"){
                    bg_status = "#92D14F"
                }
                else if(dataPsDelay[i].isDelayed == "Y"){
                    bg_status = "#ED2D41"
                }
                var htmPsDelay = htmPsDelay + '<div style="height: 16%;text-align:center;border-right:1px solid black;padding-top:2%;position: absolute;z-index:'+i+''
                                    +'align-items: center;justify-content: center;font-weight: bold;background-color: ' + bg_status + ';margin-left:' + lctDelay + 'px'+';width:' + wLtDelay + 'px' + '">'
                                   // +'<span>' + dataPsDelay[i].caseNo.substring(0,2) + '</span><br>'
                                    //+'<span>' + dataPsDelay[i].caseNo.substring(2,dataPsDelay[i].caseNo.length) + '</span><br>'
                                    +'<div style="transform: rotate(270deg);margin-top:5px">'+ dataPsDelay[i].caseNo + '</div>'
                                    +'<div style="margin-top: 40%">(' + this.pipe.transform(dataPsDelay[i].workingDate,'MMM d') + ')</div>'
                                    +'</div>';
            }
            htm_NotFinnish.innerHTML = htmPsDelay;
        }

    }

}
