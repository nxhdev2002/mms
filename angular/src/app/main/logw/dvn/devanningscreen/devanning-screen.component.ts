import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwDvnContListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    templateUrl: './devanning-screen.component.html',
    styleUrls: ['./devanning-screen.component.less'],
})
export class DevanningScreenComponent extends AppComponentBase implements OnInit {

    //screen
    strdate: string = "";
    // dataPlanDev: any[] = [];
    // dataDaily: any[] = [];
    countPlanActual: number = 0;
    countPlanBase: number = 0;
    numPlanActual: any[] = [];
    numPlanBase: any[] = [];
    numFull: any[] = [];
    clearTimeLoadData;
    fn:CommonFunction = new CommonFunction();

    constructor(
        injector: Injector,
        private _service: LgwDvnContListServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.bindTitleHeader();
    }

    ngAfterViewInit() {
       this.timeoutData()
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

            this.bindTitleHeader();
            this.getDataPlanDev();
            this.getDataDaily();
            this.fn.showtime('time_now_log');

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

    }



    getDataPlanDev() {
        try{
            this._service.getPlanDev()
            .subscribe((result) => {

                this.bindDataPlanDev(result.items ?? []);

            },(error) => {
                console.log(error);
            })
        } catch(ex){
            console.log(ex);
        }
    }

    getDataDaily() {
        this._service.getsDailly()
        .subscribe((result) => {
            try{
                let data = result.items ?? [];

                this.bindDataDaily(data);
                this.bydataLineOff(data);
                this.byDataDev(data);

                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            }catch(ex){
                console.log('2: '+ ex);
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

    bydataLineOff(data:any[]) {
        let htmLLineOff = document.querySelector('.content_dev_v2 .row4_ct .line_off_right');
        let w = window.innerWidth;
        var leftCt6 = ((w / 100) * 5 - 10);
        var rightCT6 = w - leftCt6 - 1;
        var wLineOff = (rightCT6 / 120) //120:giá trị line_off

        // var response = this.dataDaily;

        if (data.length > 0) {
            var pa = data[0].totalActual;
            var pb = ((data.length) * 10);

            var htm = "";
            for (var i = 0; i < 120; i++) {
                if (i < pa) {
                    var htm = htm + '<div style="height: 100%;border-right: 1px solid black;background-color: #7cf602;width:' + wLineOff + 'px' + '"></div>';
                }
                if (pa <= i && i < pb) {
                    var htm = htm + '<div style="height: 100%;border-right: 1px solid black;background-color: #fffa01;width:' + wLineOff + 'px' + '"></div>';
                }
                if (i >= pb && i < 120) {
                    var htm = htm + '<div style="height: 100%;border-right: 1px solid black;background-color: white;width:' + wLineOff + 'px' + '"></div>';
                }
            }
            htmLLineOff.innerHTML = htm;
        }
    }

    byDataDev(response:any[]) {
        let w = window.innerWidth;
        var h = window.innerHeight;

        let cssContentdevv2 = document.querySelectorAll<HTMLElement>('.content_dev_v2');
        for (let i = 0; cssContentdevv2[i]; i++) {
            cssContentdevv2[i].style.width = w + "px";
            cssContentdevv2[i].style.height = h + "px";
        }

        var leftCt4 = ((w / 100) * 5 - 10);
        let cssLineoffleft = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row5_ct .reban_right');
        for (let i = 0; cssLineoffleft[i]; i++) {
            cssLineoffleft[i].style.width = (leftCt4 - 1) + "px";
        }

        var rightCT4 = w - leftCt4 - 1;
        let cssLineoffright = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row5_ct .reban_right');
        for (let i = 0; cssLineoffright[i]; i++) {
            cssLineoffright[i].style.width = rightCT4 + "px";
        }
        var wTaktdt = (rightCT4 / 120) //120:giá trị line_off
        let cssRightactual = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row4_ct .line_off_right .right_actual');
        for (let i = 0; cssRightactual[i]; i++) {
            cssRightactual[i].style.width = wTaktdt + "px";
        }

        let cssRightplan = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row4_ct .line_off_right .right_plan');
        for (let i = 0; cssRightplan[i]; i++) {
            cssRightplan[i].style.width = wTaktdt + "px";
        }

        let cssRight = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row4_ct .line_off_right .right');
        for (let i = 0; cssRight[i]; i++) {
            cssRight[i].style.width = wTaktdt + "px";
        }

        var leftCt5 = ((w / 100) * 5 - 10);
        let cssRebanleft = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row5_ct .reban_left');
        for (let i = 0; cssRebanleft[i]; i++) {
            cssRebanleft[i].style.width = (leftCt5 - 1) + "px";
        }

        var rightCT5 = w - leftCt5 - 1;
        let cssRebanright = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row5_ct .reban_right');
        for (let i = 0; cssRebanright[i]; i++) {
            cssRebanright[i].style.width = rightCT5 + "px";
        }

        let htmlRenbanright = document.querySelector('.content_dev_v2 .row5_ct .reban_right');

        //Avg Takt time
        var AVG_TAKT_DEV_LT = 0;
        var TOTAL_ACTUAL = 0;
        //var response = this.dataDaily;

        if (response.length > 0) {

            AVG_TAKT_DEV_LT = response[0].avgTaktDevLT;
            TOTAL_ACTUAL = response[0].totalActual;

            var a = response.length;
            var htm = "";
            for (var i = 0; i < a; i++) {
                var z = i + 1;
                var s1 = response[i].planDevanningLineOff;
                var wLt = response[i].devLeadtime;
                var lt = (wLt * 60) / AVG_TAKT_DEV_LT;
                var start = (wTaktdt * s1 - 1);
                var wLeadtime = (lt * wTaktdt);
                var locationActual = wTaktdt * TOTAL_ACTUAL


                if ((response[i].actDevanningDateFinished == null) && (locationActual > (start + wLeadtime))) {
                    if (response[i].status == "DEVANED") {
                        var htm = htm + '<div class="cssReban_color_v2" style="height: 16.6%;border-left: 1px solid black;border-right: 1px solid black;text-align: center;background-color: red;float: left;position: absolute;animation-name: example;animation-duration: 2s;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else if (response[i].status == "DEVANNING") {
                        var htm = htm + '<div class="cssReban_devaning_v2" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;background-color: red;float: left;animation-name: example;animation-duration: 2s;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else if (response[i].status == "ARRIVED") {
                        var htm = htm + '<div class="cssReban_v2" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;text-align: center;background-color: red;float: left;position: absolute;animation-name: example;animation-duration: 2s;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb1" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else {
                        var htm = htm + '<div class="cssReban_v2" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;text-align: center;background-color: red;float: left;position: absolute;animation-name: example;animation-duration: 2s;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';

                    }
                }
                else {
                    if (response[i].status == "DEVANED") {
                        var htm = htm + '<div class="cssReban_color" style="height: 16.6%;border-left: 1px solid black;border-right: 1px solid black;background-color: #4cd403;text-align: center;float: left;position: absolute;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else if (response[i].status == "DEVANNING") {
                        var htm = htm + '<div class="cssReban_devaning" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;background-color: #36fca1;text-align: center;float: left;position: absolute;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else if (response[i].status == "ARRIVED") {
                        var htm = htm + '<div class="cssReban" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;text-align: center;float: left;position: absolute;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb1" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                    else {
                        var htm = htm + '<div class="cssReban" style="height: 16.6% !important;border-left: 1px solid black;border-right: 1px solid black;height: 100%;text-align: center;float: left;position: absolute;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
                            + '<div class="rb0" ></div>'
                            + '<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 10%;margin-top: 70px;">' + response[i].renban + '</div>'
                            + '</div>';
                    }
                }
            }
            htmlRenbanright.innerHTML = htm;
        }
        else {
            htmlRenbanright.innerHTML = "";
        }
    }

    // bydataDelay(){
    //     let w = window.innerWidth;
    //     var leftCt6 = ((w / 100) * 5 - 10);
    //     let cssLineoffleft = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row4_ct .line_off_left');
    //     for(let i=0; cssLineoffleft[i]; i++){
    //         cssLineoffleft[i].style.width = (leftCt6-1) + "px";
    //     }

    //     var rightCT6 = w - leftCt6 - 1;
    //     let cssLineoffright = document.querySelectorAll<HTMLElement>('.content_dev_v2 .row4_ct .line_off_right');
    //     for(let i=0; cssLineoffright[i]; i++){
    //         cssLineoffright[i].style.width = rightCT6 + "px";
    //     }

    //     let htmlDelayright = document.querySelector('.content_dev_v2 .row6_ct .delay_right');

    //     var wTaktdt = (rightCT6 / 120) //120:giá trị line_off

    //     //Avg Takt time

    //     if (this.dataDaily.length > 0) {

    //         var a = this.dataDaily.length;
    //         var response = this.dataDaily;
    //         var htm = "";
    //         for (var i = 0; i < a; i++) {
    //             var z = i + 1;
    //             var start = (4 * wTaktdt * i);
    //             var wLeadtime = (4 * wTaktdt);

    //             if (response[i].status == "ARRIVED") {
    //                 var htm = htm + '<div class="cssDelay" style="height: 9.8% !important;border-right: 1px solid black;height: 100%;text-align: center;position: absolute;background-color: #ff0000;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
    //                 +'<div class="rb1" ></div>'
    //                 +'<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 15%;margin-top: 40px;color:white;" >' + response[i].renban + '</div>'
    //                 +'</div>';
    //             }
    //             else {
    //                 var htm = htm + '<div class="cssDelay" style="height: 9.8% !important;border-right: 1px solid black;height: 100%;text-align: center;position: absolute;background-color: #ff0000;z-index:' + z + ';margin-left:' + start + 'px' + ';width:' + wLeadtime + 'px' + '">'
    //                 +'<div class="rb0" ></div>'
    //                 +'<div class ="reban_name" style="width: 30%;transform: rotate(270deg);margin-left: 15%;margin-top: 40px;color:white;" >' + response[i].renban + '</div>'
    //                 +'</div>';
    //             }

    //         }
    //         htmlDelayright.innerHTML = htm;

    //     }
    //     else {
    //         htmlDelayright.innerHTML = "";
    //     }

    // }

    bindDataPlanDev(planDev:any[]) {
        let _ct_no = document.querySelector('.content_dev_v2 .row3_ct .r3ct_left .container_no .ct_no');
        let _ct_no_big = document.querySelector('.content_dev_v2 .row3_ct .r3ct_left .container_no .ct_no_big');
        let _rb = document.querySelector('.content_dev_v2 .row3_ct .r3ct_left .rb');
        let _dev_type = document.querySelector('.content_dev_v2 .row3_ct .r3ct_left .dev_type .cssLot');

        let _ct_no_right = document.querySelector('.content_dev_v2 .row3_ct .r3ct_right .container_no .ct_no');
        let _ct_no_big_right = document.querySelector('.content_dev_v2 .row3_ct .r3ct_right .container_no .ct_no_big');
        let _rb_right = document.querySelector('.content_dev_v2 .row3_ct .r3ct_right .rb');
        let _dev_type_right = document.querySelector('.content_dev_v2 .row3_ct .r3ct_right .dev_type .cssLot');

        // var planDev = this.dataPlanDev
        //r3ct_left
        //_ct_no


        if (planDev.length > 0) {
            var dt_containerNo = planDev[0].containerNo
            if (dt_containerNo.length > 4) {
                _ct_no.innerHTML = dt_containerNo.substring(0, (dt_containerNo.length - 4));
            }
            else {
                _ct_no.innerHTML = dt_containerNo;
            }
        }
        else {
            _ct_no.innerHTML = "&nbsp;";
        }

        //ct_no_big
        if (planDev.length > 0) {
            var dt_containerNo = planDev[0].containerNo
            if (dt_containerNo.length > 4) {
                _ct_no_big.innerHTML = dt_containerNo.substring((dt_containerNo.length), (dt_containerNo.length - 4));
            }
            else {
                _ct_no_big.innerHTML = "&nbsp;";
            }
        }
        else {
            _ct_no_big.innerHTML = "&nbsp;";
        }

        //rb
        if (planDev.length > 0) {
            _rb.innerHTML = planDev[0].renban
        }
        else {
            _rb.innerHTML = "&nbsp;"
        }

        //dev_type
        if (planDev.length > 0) {
            _dev_type.innerHTML = planDev[0].devanningType
        }
        else {
            _dev_type.innerHTML = "<br />"
        }


        //r3ct_right
        //_ct_no
        if (planDev.length > 0) {
            var dt_containerNo = planDev[1].containerNo
            if (dt_containerNo.length > 4) {
                _ct_no_right.innerHTML = dt_containerNo.substring(0, (dt_containerNo.length - 4));
            }
            else {
                _ct_no_right.innerHTML = dt_containerNo;
            }
        }
        else {
            _ct_no_right.innerHTML = "&nbsp;";
        }

        //ct_no_big
        if (planDev.length > 0) {
            var dt_containerNo = planDev[1].containerNo
            if (dt_containerNo.length > 4) {
                _ct_no_big_right.innerHTML = dt_containerNo.substring((dt_containerNo.length), (dt_containerNo.length - 4));
            }
            else {
                _ct_no_big_right.innerHTML = "&nbsp;";
            }
        }
        else {
            _ct_no_big_right.innerHTML = "&nbsp;";
        }

        //rb
        if (planDev.length > 0) {
            _rb_right.innerHTML = planDev[1].renban
        }
        else {
            _rb_right.innerHTML = "&nbsp;"
        }

        //dev_type
        if (planDev.length > 0) {
            _dev_type_right.innerHTML = planDev[1].devanningType
        }
        else {
            _dev_type_right.innerHTML = "<br />"
        }



    }

    bindDataDaily(dailys:any[]) {
        let _plan_actual = document.querySelector('.content_dev_v2 .row4_ct .line_off_left .line_off_left_detail .plan_actual');
        let _plan_base = document.querySelector('.content_dev_v2 .row4_ct .line_off_left .line_off_left_detail .plan_base');

        // var dailys = this.dataDaily;

        this.countPlanActual = dailys[0].totalActual;
        this.countPlanBase = ((dailys.length) * 10);

        _plan_actual.innerHTML = this.countPlanActual.toString();
        _plan_base.innerHTML = this.countPlanBase.toString();

    }


    bindTitleHeader() {
        if (this.strdate != strdatetmp) {
            var d = new Date();
            var strdatetmp = this.getTime(d).replace(" : ", ":") + " (" + d.getDate() + "-" + this.getMonthEN(d.getMonth()).substring(0, 3) + ")";
            let _timetitle = document.querySelector('.content_dev_v2 .row1_ct .timetitle');
            _timetitle.innerHTML = strdatetmp.toString();
            this.strdate = strdatetmp;

        }
    }

    getTime(dt) {
        var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " + (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
        return strtime;
    }

    getMonthEN(m) {
        switch (m) {
            case 0: return "January";
            case 1: return "February";
            case 2: return "March";
            case 3: return "April";
            case 4: return "May";
            case 5: return "June";
            case 6: return "July";
            case 7: return "August";
            case 8: return "September";
            case 9: return "October";
            case 10: return "November";
            case 11: return "December";
            default: return m;
        }
    }

    fornumbersPlan(start: number, stop: number) {
        let list: any[] = [];
        for (let i = start; i <= stop; i++) {
            list.push({ a: i.toString() });
        }
        return list;
    }
}
