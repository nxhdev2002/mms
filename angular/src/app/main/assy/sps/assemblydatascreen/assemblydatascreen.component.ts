import { AsyAdoAssemblyScreenDataOutputDto, AsyAdoAssemblyScreenServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { finalize, timeout } from 'rxjs/operators';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonFunction } from "@app/main/commonfuncton.component";


@Component({
    selector: 'app-assemblydatascreen',
    templateUrl: './assemblydatascreen.component.html',
    styleUrls: ['./assemblydatascreen.component.less']
})
export class AssemblyDataScreenComponent implements OnInit {

    dataScreen: AsyAdoAssemblyScreenDataOutputDto[] = [];
    prodLine: string = '';
    clearTimeLoadForm;
    datenow;
    timenow;
    clearTimeLoadData;
    countData: number = 0;
    c_DataTop: number = 0;

    constructor(
        private _assemblydatascreen: AsyAdoAssemblyScreenServiceProxy
    ) { }

    ngOnInit() {
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('screen_code');
        if(this.prodLine.toLowerCase()=="a1")
        {
            document.querySelector<HTMLElement>('.REMARK1').style.display='none'
        }else if(this.prodLine.toLowerCase()=="a2"){
            document.querySelector<HTMLElement>('.REMARK').style.display='none'
        }
        this.getData();
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.loadForm();
            this.timeoutData();
        }, 2000);
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }

    getDateTimeNow() {
        var get_date = new Date;
        this.datenow = get_date.getDate() + '/' + (get_date.getMonth() + 1) + '/' + get_date.getFullYear();
        this.timenow = get_date.getHours() + ":" + get_date.getMinutes() + ":" + get_date.getSeconds();
    }

    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }

    timecount: number = 0;
    refeshPage: number = 600;
    timeoutData() {
        try {

            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getScreenData();
            this.getDateTimeNow();
            //  this.fn.showtime('time_now_log');

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    getData() {
        this._assemblydatascreen.getScreenData(this.prodLine)
            .subscribe((result) => {
                this.dataScreen = result ?? [];
                this.c_DataTop = Math.round(this.dataScreen.length / 2);
            });
    }

    getScreenData() {
        this._assemblydatascreen.getScreenData(this.prodLine)
            .subscribe((result) => {
                this.dataScreen = result ?? [];
                this.byData(this.dataScreen);
                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            }, (error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            });
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm()
    }

    loadForm() {
        var w = window.innerWidth;
        var ccs_col_process = document.querySelectorAll<HTMLElement>('.BODY_CONTENT .col_process');
        var ccs_col_process2 = document.querySelectorAll<HTMLElement>('.BODY_CONTENT2 .col_process');


        let w_col_process = w / (ccs_col_process.length)


        for (let i = 0; ccs_col_process[i]; i++) {
            ccs_col_process[i].style.width = w_col_process + 'px';
        }

        for (let i = 0; ccs_col_process2[i]; i++) {
            ccs_col_process2[i].style.width = w_col_process + 'px';
        }
    }

    byData(jdata: AsyAdoAssemblyScreenDataOutputDto[]) {
        let tmpPartTypeName = '';

        var count_col_process = document.querySelectorAll<HTMLElement>('.BODY_CONTENT .col_process');
        let count_col = Number(count_col_process.length);

        //var item;
        jdata.forEach((element) => {

            var _grade = element.grade + ((element.isTrd == 'Y') ? '-TRD' : "")
            var _seqno = '';
            try { _seqno = element.seqNo.substring(element.seqNo.length - 3, element.seqNo.length); } catch (ex) { }

            if (Number(element.rowNo) <= count_col) {
                let item = document.querySelector<HTMLElement>('.BODY_CONTENT .col_process.loc_col_' + element.rowNo);
                if(item){
                    let PROCESS = item.querySelector<HTMLElement>('.PROCESS');
                    let GRADE = item.querySelector<HTMLElement>('.GRADE');
                    let SEQ2 = item.querySelector<HTMLElement>('.SEQ2');
                    let BODY = item.querySelector<HTMLElement>('.BODY');
                    let SEQ = item.querySelector<HTMLElement>('.SEQ');
                    let PROCESS2 = item.querySelector<HTMLElement>('.PROCESS2');

                    if(PROCESS){

                        PROCESS.textContent = element.process
                        GRADE.innerHTML = _grade + "<br />" + ((element.lotNo != ' ') ? element.lotNo.split("-")[1] : '')
                        SEQ2.textContent = element.color
                        BODY.textContent = element.body
                        SEQ.textContent = _seqno
                        PROCESS2.textContent = jdata[element.rowNo].process

                        var _color = this.getcolor(element.modelName)
                        if(element.isTrd != 'Y'){
                            GRADE.style.backgroundColor = _color;
                            SEQ2.style.backgroundColor = _color;
                            BODY.style.backgroundColor = _color;
                            SEQ.style.backgroundColor = _color;
                        }else{
                            GRADE.style.backgroundColor = 'red';
                            SEQ2.style.backgroundColor = 'red';
                            BODY.style.backgroundColor = 'red';
                            SEQ.style.backgroundColor = 'red';
                        }

                        if(element.isProject == 'Y'){
                            GRADE.style.backgroundColor = '#FFFF00';
                            SEQ2.style.backgroundColor = '#FFFF00';
                            BODY.style.backgroundColor = '#FFFF00';
                            SEQ.style.backgroundColor = '#FFFF00';
                        }

                    }
                }


            }
            else {
                let item2 = document.querySelector<HTMLElement>('.BODY_CONTENT2 .col_process.loc_col_' + element.rowNo);
                if(item2){
                    let PROCESS_2 = item2.querySelector<HTMLElement>('.PROCESS');
                    let GRADE_2 = item2.querySelector<HTMLElement>('.GRADE');
                    let SEQ2_2 = item2.querySelector<HTMLElement>('.SEQ2');
                    let BODY_2 = item2.querySelector<HTMLElement>('.BODY');
                    let SEQ_2 = item2.querySelector<HTMLElement>('.SEQ');
                    let PROCESS2_2 = item2.querySelector<HTMLElement>('.PROCESS2');

                    if(PROCESS_2){

                        PROCESS_2.textContent = element.process
                        GRADE_2.innerHTML = _grade + "<br />" +  ((element.lotNo != ' ') ? element.lotNo.split("-")[1] : '')
                        SEQ2_2.textContent = element.color
                        BODY_2.textContent = element.body
                        SEQ_2.textContent = _seqno
                        PROCESS2_2.textContent = (Number(element.rowNo) < jdata.length) ? jdata[(element.rowNo)].process :'';


                        var _color2 = this.getcolor(element.modelName)
                        if(element.isTrd != 'Y'){
                            GRADE_2.style.backgroundColor = _color2;
                            SEQ2_2.style.backgroundColor = _color2;
                            BODY_2.style.backgroundColor = _color2;
                            SEQ_2.style.backgroundColor = _color2;
                        }else{
                            GRADE_2.style.backgroundColor = 'red';
                            SEQ2_2.style.backgroundColor = 'red';
                            BODY_2.style.backgroundColor = 'red';
                            SEQ_2.style.backgroundColor = 'red';
                        }

                        if(element.isProject == 'Y'){
                            GRADE_2.style.backgroundColor = '#FFFF00';
                            SEQ2_2.style.backgroundColor = '#FFFF00';
                            BODY_2.style.backgroundColor = '#FFFF00';
                            SEQ_2.style.backgroundColor = '#FFFF00';
                        }
                    }
                }


            }
        })



    }
     getcolor(m) {
        //alert(m);
        if (m.toLowerCase() == "camry") {
            return "#FF99CC";
        } else if (m.toLowerCase() == "vios") {
            return "#FABF8F";
        } else if (m.toLowerCase() == "avanza") {
            return "#8DB4E2";
        } else if (m.toLowerCase() == "fortuner") {
            return "#FFFF00";
        } else if (m.toLowerCase() == "innova") {
            return "#C8C09D";
        }

        return "";
    }
}


