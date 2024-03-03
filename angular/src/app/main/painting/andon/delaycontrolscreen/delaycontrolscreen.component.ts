
import { DatePipe } from '@angular/common';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import {
    GetDataOutput,
    PtsAdoDelayControlScreenServiceProxy,
} from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-delaycontrolscreen',
    templateUrl: './delaycontrolscreen.component.html',
    styleUrls: ['./delaycontrolscreen.component.less'],
})
export class DelaycontrolscreenComponent implements OnInit {
    rowStorage: GetDataOutput = new GetDataOutput();
    dataDelayScreen: any[] = [];
    aray: any[] = [];
    today: Date = new Date();
    pipe = new DatePipe('en-US');
    Datedlc: any;
    colTs: number;
    getDayLeaTimePlus: any[] = [];
    getDayRepairIn: any[] = [];
    getTimetxt_ETD: any[] = [];
    getTimeETD: any[] = [];
    lookupDelayTaget: any[] = [];
    lookupDelayTaget2: any[] = [];
    soundstart = 'false';
    soundphaytime = 0;
    soundtype = '';
    audioElement;
    timer;
    h = "";
    m = "";
    h2 = "";
    m2 = "";
    delay_target = "";
    delay_target2 = "";
    v_RemainTime: any[] = [];
    getTimeATD: any[] = [];
    getDateATD: any[] = [];
    w_6hour = 0;
    h_item = 0;
    h_horize = 0;


    constructor(private _service: PtsAdoDelayControlScreenServiceProxy) {}

    ngOnInit() {
        this._service.getData().pipe(finalize(() => {
                    setTimeout(() => {
                        this.onWindowResize();
                    }, 1000);
                })
            )
            .subscribe((result) => {
                if(result.items.length > 0)
                {
                this.dataDelayScreen = result.items ?? [];
                this.binDataToForm(this.dataDelayScreen);
                this.Navigator_Data(this.dataDelayScreen);
            }
            });

        this._service.getLookupDelayTaget("DELAY_TARGET").pipe(finalize(() => {}))
        .subscribe((result) => {
            this.lookupDelayTaget = result.items ?? [];
            });


        this._service.getLookupDelayTaget("DELAY_TARGET2").pipe(finalize(() => {}))
        .subscribe((result) => {
            this.lookupDelayTaget2 = result.items ?? [];
            });
        }



    Navigator_Data(dataDelayScreen){
        // var endtime;
        // var starttime;
        // var s;
        // var ms;
        // starttime = new Date();
        // if (this.soundstart != "true" ) { //lấy trạng thái nhạc
        //     this.soundstart = $(".soundAttribute").attr("soundstart");
        //     this.soundtype = $(".soundAttribute").attr("soundtype");
        //     // caseno_delay = $(".soundAttribute").attr("caseno");
        // }

      }


      /***********************************SOUND*************************************************/


 audioplay() {
    this.audioElement.play();
}

 audiopause() {
    this.audioElement.pause();
}

 audiostop() {

    this.audioElement.pause();
    //audioElement.currentTime = 0;
    this.audioElement.setAttribute("src", "");
    // $(".soundAttribute").attr({
    //     "soundstart": "false",
    //     "soundtype": "",
    //     "src": "",
    //     "caseno": ""
    // });
    this.stopTimeOut();
}

 stopTimeOut() {
    if (this.timer) {
        clearTimeout(this.timer);
        this.timer = 0;
    }
}

 loadAudio() {
    this.audioElement = document.createElement('audio');
}

 StartSound(urlsound, tacktime, stype, caseno) {

    // this.soundstart = $(".soundAttribute").attr("soundstart");
    // this.soundtype = $(".soundAttribute").attr("soundtype");

    // if (this.soundstart == "false") {
    //     this.soundphaytime = 0;
    //     this.soundtype = stype;
    //     //Chạy Sound

    //     this.audioElement.setAttribute("src", urlsound);
    //     this.audioElement.setAttribute("currentTime", tacktime);
    //     this.audioElement.setAttribute("LOOP", "true");
    //     this.audioplay();


    //     // $(".soundAttribute").attr({
    //     //     "soundstart": "true",
    //     //     "soundtype": stype,
    //     //     "src": urlsound,
    //     //     "caseno": caseno
    //     // });
    //     this.soundstart = "true";

    //     this.timer = setTimeout("StartSound('" + urlsound + "'," + tacktime + ",'" + this.soundtype + "','" + caseno + "')", 1000);

    // } else if (this.soundtype == stype && this.soundstart == "true" && this.soundphaytime <= tacktime) {

    //     this.soundphaytime++;
    //     //$(".message4").html(soundphaytime + " - " + soundstart + " - " + urlsound);
    //     this.timer = setTimeout("StartSound('" + urlsound + "','" + tacktime + "','" + this.soundtype + "','" + caseno + "')", 1000);
    // } else {
    //     this.audiostop();
    // }
}
/******************************END SOUND**************************************************/




    getDate() {
        this.Datedlc = this.pipe.transform(this.today, 'd - MMM');
    }
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        var w = window.innerWidth;

        var h = window.innerHeight;
        var hCol = (h * 10) / 100;
        var w6 = (w * 65) / 100;
        var w4 = w - w6;
        var wheader = (w * 4) / 100;

        //chia width 6
        var w6_5 = (w6 * 7) / 100; // 5%
        var w6_15 = (w6 * 20) / 100; // 15%
        var w6_8 = (w6 * 10) / 100; // 8%
        var w6_10 = (w6 * 15) / 100; // 8%
        var w6_7 = (w6 * 9) / 100; // 7%
        //chia width 4
        var w4_6 = w4 / 6;
        var w4_6_2 = (w4_6 * 64) / 100;
        var w4_6_5_6 = w4_6 - (w4_6 * 46) / 100;

        //loadScreenView
        var w_6hour = (Math.floor(((w - 4) / 3) * 100) / 100) - 2;
        var hItem = Math.floor((h / 12) * 100) / 100;
        var h_horize_7 = Math.floor(((hItem - 2) / 7) * 100) / 100;
        var h_full_horize = (hItem - 2) - (h_horize_7 * 2);
        this.h_horize = h_full_horize;
        this.h_item = hItem - 2;

        //set width cho header
        var header = document.querySelectorAll<HTMLElement>('.boxScreen-header');
        for (let i = 0; header[i]; i++) {
            (header[i] as HTMLElement).style.width = w + 'px';
            (header[i] as HTMLElement).style.height = hCol + 'px';
            (header[i] as HTMLElement).style.paddingLeft = wheader + 'px';
            (header[i] as HTMLElement).style.paddingRight = wheader + 'px';
        }

        //get class
        var col4 = document.querySelectorAll<HTMLElement>('.col-body__6');
        var Col = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col');
        var Col1 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col1');
        var Col2 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col2');
        var Col3 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col3');
        var Col4 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col4');
        var Col5 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col5');
        var Col6 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col6');
        var Col7 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col7');
        var Col8 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col8');
        var Col9 = document.querySelectorAll<HTMLElement>('.col-body__6 .boxScreen--body__col9');

        //set px class cha
        for (let i = 0; col4[i]; i++) {
            (col4[i] as HTMLElement).style.width = w6 + 'px';
            (col4[i] as HTMLElement).style.height = hCol + 'px';
        }
        //set px class con
        for (let i = 0; Col[i]; i++) {
            (Col[i] as HTMLElement).style.width = w6_5 + 'px';
        }

        for (let i = 0; Col1[i]; i++) {
            (Col1[i] as HTMLElement).style.width = w6_15 + 'px';
        }

        for (let i = 0; Col2[i]; i++) {
            (Col2[i] as HTMLElement).style.width = w6_7 + 'px';
        }

        for (let i = 0; Col3[i]; i++) {
            (Col3[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col4[i]; i++) {
            (Col4[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col5[i]; i++) {
            (Col5[i] as HTMLElement).style.width = w6_15 + 'px';
        }

        for (let i = 0; Col6[i]; i++) {
            (Col6[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col7[i]; i++) {
            (Col7[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col8[i]; i++) {
            (Col8[i] as HTMLElement).style.width = w6_8 + 'px';
        }

        for (let i = 0; Col9[i]; i++) {
            (Col9[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        // set width
        var col4 = document.querySelectorAll<HTMLElement>('.col-body__4');
        var col4_1 = document.querySelectorAll<HTMLElement>('.col4-1');
        var col4_2 = document.querySelectorAll<HTMLElement>('.col4-2');
        var col4_3 = document.querySelectorAll<HTMLElement>('.col4-3');
        var col4_4 = document.querySelectorAll<HTMLElement>('.col4-4');
        var col4_5 = document.querySelectorAll<HTMLElement>('.col4-5');
        var col4_6 = document.querySelectorAll<HTMLElement>('.col4-6');

        //get class cha
        for (let i = 0; col4[i]; i++) {
            (col4[i] as HTMLElement).style.width = w4 + 'px';
        }

        //get class con
        for (let i = 0; col4_1[i]; i++) {
            (col4_1[i] as HTMLElement).style.width = w4_6 + 'px';
        }

        for (let i = 0; col4_2[i]; i++) {
            (col4_2[i] as HTMLElement).style.width = w4_6 + 'px';
        }

        for (let i = 0; col4_3[i]; i++) {
            (col4_3[i] as HTMLElement).style.width = w4_6 + 'px';
        }

        for (let i = 0; col4_4[i]; i++) {
            (col4_4[i] as HTMLElement).style.width = w4_6_2 + 'px';
        }

        for (let i = 0; col4_5[i]; i++) {
            (col4_5[i] as HTMLElement).style.width = w4_6_5_6 + 'px';
        }

        for (let i = 0; col4_6[i]; i++) {
            (col4_6[i] as HTMLElement).style.width = w4_6 + 'px';
        }

        //get class footer
        var footter = document.querySelectorAll<HTMLElement>('.boxScreen-footter');
        var footterCol = document.querySelectorAll<HTMLElement>('.footter-col_6');
        var Col = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col');
        var Col1 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col1');
        var Col2 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col2');
        var Col3 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col3');
        var Col4 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col4');
        var Col5 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col5');
        var Col6 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col6');
        var Col7 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col7');
        var Col8 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col8');
        var Col9 = document.querySelectorAll<HTMLElement>('.boxScreen--footter__col9');

        //get class cha
        for (let i = 0; footter[i]; i++) {
            (footter[i] as HTMLElement).style.width = w + 'px';
            (footter[i] as HTMLElement).style.height = hCol + 'px';
        }
        for (let i = 0; footterCol[i]; i++) {
            (footterCol[i] as HTMLElement).style.width = w6 + 'px';
        }
        for (let i = 0; Col[i]; i++) {
            (Col[i] as HTMLElement).style.width = w6_5 + 'px';
        }

        for (let i = 0; Col1[i]; i++) {
            (Col1[i] as HTMLElement).style.width = w6_15 + 'px';
        }

        for (let i = 0; Col2[i]; i++) {
            (Col2[i] as HTMLElement).style.width = w6_7 + 'px';
        }

        for (let i = 0; Col3[i]; i++) {
            (Col3[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col4[i]; i++) {
            (Col4[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col5[i]; i++) {
            (Col5[i] as HTMLElement).style.width = w6_15 + 'px';
        }

        for (let i = 0; Col6[i]; i++) {
            (Col6[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col7[i]; i++) {
            (Col7[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        for (let i = 0; Col8[i]; i++) {
            (Col8[i] as HTMLElement).style.width = w6_8 + 'px';
        }

        for (let i = 0; Col9[i]; i++) {
            (Col9[i] as HTMLElement).style.width = w6_10 + 'px';
        }

        var footter10 = document.querySelectorAll<HTMLElement>('.boxScreen-footter .footter-col_4');
        for (let i = 0; footter10[i]; i++) {
            (footter10[i] as HTMLElement).style.width = w4 + 'px';
        }
    }

    binDataToForm(dataDelayScreen){
        dataDelayScreen.forEach((element) => {
            //LeaTimePlus
            const dateValueLeaTimePlus = element.leadtimePlus
                ? new Date(element.leadtimePlus?.toString())
                : new Date();
            var setHLeaTimePlus = moment(dateValueLeaTimePlus.toString()).format('hh:mm');
            var setD = moment(dateValueLeaTimePlus.toString()).format('DD');
            element.leadtimePlus = setHLeaTimePlus;
            element.getDayLeaTimePlus = setD;

            //RepairIn
            const dateValuestartRepairIn = element.repairIn
                ? new Date(element.repairIn?.toString())
                : new Date();
            var setRepairIn = moment(dateValuestartRepairIn.toString()).format('hh:mm');
            var setD = moment(dateValuestartRepairIn.toString()).format('DD');
            element.repairIn = setRepairIn;
            element.getDayRepairIn = setD;

            //delay
            var delay = this.toHHh_MMm(element.totalDelayAct < 0 ? element.totalDelayAct * -1 : element.totalDelayAct);
            element.delayValue = delay;

            //etd
            const dateValuestartEtd = element.etd ? new Date(element.etd?.toString()) : new Date();
            var setH = moment(dateValuestartEtd.toString()).format('hh:mm');
            element.getTimeETD = setH;
            var txt_ETD = this.toHHh_MMm((element.etd - element.startRepair) / 1000);
            element.getTimetxt_ETD = txt_ETD;

            const dateValueaInPlanDate = element.aInPlanDate ? new Date(element.aInPlanDate?.toString()) : new Date();
            var setaInPlanDate = this.getTime(dateValueaInPlanDate)
            element.getTimeATD = setaInPlanDate;
            var txt_ATD = moment(dateValueaInPlanDate.toString()).format('DD');
            element.getDateATD = txt_ATD;


            var location1 = document.getElementById("_LOCATION");
            var v_location = "<select id='_location_" + element.id + "' onchange='LocationChoose(this);' class='_LOCATION' delay_id='" + element.id + "'>" + location1 + "</select>";
            element.v_location = v_location;


            var remainTime = this.toHHh_MMm(element.remainingTime);
            element.v_RemainTime = remainTime

            var objScreen = document.querySelectorAll<HTMLElement>('.boxScreen .boxScreen-footter .footter-col_4 .horize');
            this.buildScreen(element,objScreen);

        });

        //bindTime

        var itemValue = this.lookupDelayTaget[0].itemValue
        if(this.lookupDelayTaget.length > 0){
            this.h = itemValue.substr(0,2);
            this.m = itemValue.substr(3,4);
            this.delay_target = itemValue
        }
        else{
            this.h = "2"
            this.m = "20"
            this.delay_target = "2:20"
        }

        var itemValue2 = this.lookupDelayTaget2[0].itemValue
        if(this.lookupDelayTaget2.length > 0){
            this.h2 = itemValue2.substr(0,2);
            this.m2 = itemValue2.substr(3,4);
            this.delay_target2 = itemValue2
        }
        else{
            this.h2 = "1"
            this.m2 = "10"
            this.delay_target2 = "1:10"
        }








    }

    buildScreen(data, objscreen){
        console.log(this.h);
        var delay_target = document.querySelectorAll<HTMLElement>('.boxScreen .boxScreen-header .boxScreen-header_Conten1 .boxScreen-header_txt1 ._DELAY_TARGET');
     //  console.log(delay_target);
         var int_delay_hour = this.h;
         var int_delay_minute = this.m;


        var w_hour1h = Math.floor((this.w_6hour / 6) * 100) / 100;
        var w_hour20m = Math.floor((((w_hour1h - 2) * parseInt(int_delay_minute,0)) / 60) * 100) / 100;
        var h_hour1h = Math.floor((this.h_item / 4) * 100) / 100;
        var h_hourCol = Math.floor((this.h_item / 2) * 100) / 100;

        // console.log(w_hour1h);
        // console.log(w_hour20m);
        // console.log(h_hour1h);
        // console.log(h_hourCol);


    }

    toHHh_MMm(ttseconds) {
        var sec_num = ttseconds; // don't forget the second param
        var hours = Math.floor(sec_num / 3600);
        var minutes = Math.floor((sec_num - hours * 3600) / 60);
        var namehours = hours == 0 ? '' : hours + 'h ';
        var time = namehours + minutes + 'm '; //+ ':' + seconds;
        return time;
    }


    LocationChoose(){
        // this.getDataForSCreen().pipe(finalize(()=> {})).subscribe((result) =>
        // {
        //     this.dataLineRealTime = result.items ?? [];
        // })

    }

    getTime(dt) {
        var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " + (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
        return strtime;
    }
}


