import { style, transition } from '@angular/animations';
import { Component, Directive, ElementRef, HostListener, Injector, Input, OnInit, PipeTransform } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    LgaBp2PxPUpPlanDto,
    LgaBp2PxPUpPlanServiceProxy,
    MstCmmLookupDto,
    MstLgwScreenConfigDto,
} from '@shared/service-proxies/service-proxies';
import { finalize, filter } from 'rxjs/operators';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    templateUrl: './bigpartpxpup.component.html',
    styleUrls: ['./bigpartpxpup.component.less'],
})
export class BigPartPxpUpComponent extends AppComponentBase implements OnInit {
    changeText: boolean;
    dataScreenConfig: MstLgwScreenConfigDto[] = [];
    dataLookUpByDomain: MstCmmLookupDto[] = [];
    dataModuleUpTable: any[] = [];
    dataModuleUpTableFor: any[] = [];
    dataUpPlanBaseToday: any[] = [];
    dataUpPlanUpCase: any[] = [];
    dataLastScan: any[] = [];
    dataByWorking: LgaBp2PxPUpPlanDto[] = [];
    dataUpPlanCaseDelay: any[] = [];
    dataBigPart2x2: any[] = [];
    END_NO_IN_SHIFT: any;
    TOTAL_COLUMN_SEQ_A1: any;
    databoderA1: any;
    jdata: any;
    LINE_OFF_DELAY: any;
    line_off: number = 14;
    v_iitemA1minwidth: number = 0;
    soundstart: any = 'false';
    soundphaytime: number = 0;
    soundtype: any = '';
    audioElement;
    timer;
    time_refesh: number = 200; //5 phút re-load 1 lần.
    current_refesh: number = 0;
    Requestcount = 1;
    caseno_delay;
    clearTimeLoadData;
    clearTimeLoadForm;
    fn:CommonFunction = new CommonFunction();
    changeStt: number = 0;;


    constructor(injector: Injector, private _service: LgaBp2PxPUpPlanServiceProxy) {
        super(injector);
        this.changeText = false;
    }

    ngOnInit(): void {
        // this.getData();
         this.getDataForm();
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
            }, 2000);
        }
    }


    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
        // this.getData();
    }


    getDataForm(){
        this._service.getDataModuleUpTable('A2 BIG')
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.dataModuleUpTable = result.items ?? [];
            });
    }

    getData() {
        this._service
            .getDataBigPart2X2Screen('1','LGA_BP2_BIG_PART_PXP_UP_SCREEN','A2','A2 BIG')
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                try{
                    console.log('getData');
                    this.dataBigPart2x2 = result ?? [];
                    this.dataScreenConfig = this.dataBigPart2x2[0];
                    this.dataLookUpByDomain = this.dataBigPart2x2[1];
                    // this.dataModuleUpTable = this.dataBigPart2x2[2];
                    this.dataUpPlanBaseToday = this.dataBigPart2x2[2];
                    if(this.dataUpPlanUpCase.length <= 0)  this.dataUpPlanUpCase = this.dataBigPart2x2[3];
                    this.dataLastScan = this.dataBigPart2x2[4];
                    this.dataByWorking = this.dataBigPart2x2[5];
                    this.dataUpPlanCaseDelay = this.dataBigPart2x2[6];


                    setTimeout(() => {
                        this.loadForm();
                    }, 100);

                    setTimeout(() => {
                        this.bindScan(this.dataLastScan, this.dataScreenConfig[0]);
                        this.bindDelayCase(this.dataUpPlanCaseDelay);
                        this.bindData(this.dataLastScan);
                    }, 200);

                    setTimeout(() => {
                        this.bindUnpackCase(this.dataByWorking, this.dataScreenConfig[0]);
                    }, 300);

                    // setTimeout(() => {
                            // this.bindCALL_LEADER(this.dataScreenConfig[0], this.dataLookUpByDomain);
                    //     this.bindData(this.dataLastScan);
                    // }, 500);

                    this.bindSTOP_PAUSE(this.dataByWorking);

                    this.TOTAL_COLUMN_SEQ_A1 = this.dataScreenConfig[0].totalColumnSeqA2;
                    this.END_NO_IN_SHIFT = this.dataUpPlanBaseToday[0].endNoInShift;
                    this.LINE_OFF_DELAY = this.dataLookUpByDomain.filter((f) => f.itemCode === 'LINE_OFF_DELAY')[0].itemValue;


                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 2000);

                } catch(ex){
                    console.log('2: ' + ex);
                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 2000);
                }

            },(error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 2000);
            });
    }

    iCaseDataByUP(up_tb, v_i) {
        var a = this.dataUpPlanUpCase.filter((a) => a.upTable === up_tb);
        var b = a.filter((b) => b.noOfALineIn === v_i);
        return b;
    }

    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }


    onMouseOver(mousehover: HTMLElement, i) {
        var datadetails = mousehover.getAttribute('data-details');
        document.querySelector<HTMLElement>('.popDetail_' + i).innerHTML = datadetails;
    }

    onMouseOut(i) {
        document.querySelector<HTMLElement>('.popDetail_' + i).innerHTML = '';
    }

    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight - 4;
        var wHeader = Math.floor((w / 12) * 10) / 10;
        var wContent = w - wHeader;
        var countUP = document.querySelectorAll<HTMLElement>('.HeadA1').length;
        var hUP = Math.floor((h / countUP) * 10) / 10;

        var _screenHeadCss = document.querySelectorAll<HTMLElement>('.contentScreen .screenHead');
        for (let i = 0; _screenHeadCss[i]; i++) {
            _screenHeadCss[i].style.width = wHeader + 'px';
        }

        var _headA1_subCss = document.querySelectorAll<HTMLElement>('.HeadA1 .HeadA1_sub');
        for (let i = 0; _headA1_subCss[i]; i++) {
            _headA1_subCss[i].style.height = hUP / 2 - 1 + 'px';
            _headA1_subCss[i].style.lineHeight = hUP / 2 - 1 + 'px';
        }

        var _iitem1Css = document.querySelectorAll<HTMLElement>(
            '.HeadA1'
        );
        for (let i = 0; _iitem1Css[i]; i++) {
            _iitem1Css[i].style.height = hUP - 1.8 + 'px';
            _iitem1Css[i].style.lineHeight = hUP - 1.8 + 'px';
        }

        var _iitem1Css = document.querySelectorAll<HTMLElement>(
            '.ContentA1,' + '.ContentA1 .iitemA1,' + '.ContentA1 .iitemA1UP'
        );
        for (let i = 0; _iitem1Css[i]; i++) {
            _iitem1Css[i].style.height = hUP - 1.5 + 'px';
            _iitem1Css[i].style.lineHeight = hUP - 1.5 + 'px';
        }

        var _headA1DELAYCss = document.querySelectorAll<HTMLElement>('.HeadA1.DELAY');
        for (let i = 0; _headA1DELAYCss[i]; i++) {
            _headA1DELAYCss[i].style.lineHeight = 'normal';
        }

        var _headA1DELAYCss = document.querySelectorAll<HTMLElement>('.HeadA1.DELAY');
        for (let i = 0; _headA1DELAYCss[i]; i++) {
            _headA1DELAYCss[i].style.lineHeight = 'normal';
        }

        var _iitemA1UPLineUPA1TextCss = document.querySelectorAll<HTMLElement>('.ContentA1 .iitemA1UP .LineUPA1Text');
        for (let i = 0; _iitemA1UPLineUPA1TextCss[i]; i++) {
            _iitemA1UPLineUPA1TextCss[i].style.height = hUP - 1 + 'px';
            _iitemA1UPLineUPA1TextCss[i].style.lineHeight = hUP / 4 - 1 + 'px';
        }

        var _lineUPA1TextspanCss = document.querySelectorAll<HTMLElement>('.ContentA1 .iitemA1UP .LineUPA1Text span');
        for (let i = 0; _lineUPA1TextspanCss[i]; i++) {
            _lineUPA1TextspanCss[i].style.lineHeight = hUP + 'px';
        }

        var _iitemA1UPIS_NEW_PARTCss = document.querySelectorAll<HTMLElement>('.ContentA1 .iitemA1UP .IS_NEW_PART');
        for (let i = 0; _iitemA1UPIS_NEW_PARTCss[i]; i++) {
            _iitemA1UPIS_NEW_PARTCss[i].style.height = hUP / 4 + 'px';
            _iitemA1UPIS_NEW_PARTCss[i].style.lineHeight = hUP / 4 + 'px';
        }

        var _iitemA1LineUPA1Text_DELAYnotline2spanCss = document.querySelectorAll<HTMLElement>(
            '.screenContent.delay .iitemA1UP .LineUPA1Text_DELAY:not(.line2) span'
        );
        for (let i = 0; _iitemA1LineUPA1Text_DELAYnotline2spanCss[i]; i++) {
            _iitemA1LineUPA1Text_DELAYnotline2spanCss[i].style.height = hUP / 5 + 'px';
            _iitemA1LineUPA1Text_DELAYnotline2spanCss[i].style.lineHeight = hUP / 2 + 'px';
        }

        var _iitemA1UPbg_CaseDelayCss = document.querySelectorAll<HTMLElement>('.ContentA1 .iitemA1UP .bg_CaseDelay');
        for (let i = 0; _iitemA1UPbg_CaseDelayCss[i]; i++) {
            _iitemA1UPbg_CaseDelayCss[i].style.height = hUP + 1 + 'px';
        }

        //

        var countiitemA1 = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .iitemA1').length;
        var iitemA1width = wContent / this.line_off;
        var wScroll_warp = iitemA1width * countiitemA1;
        var iitemA1minwidth = Math.floor(iitemA1width * 10) / 10;
        this.v_iitemA1minwidth = Math.floor(iitemA1width * 10) / 10;
        wScroll_warp = Math.floor(wScroll_warp * 10) / 10;

        var _screenContentCss = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent');
        for (let i = 0; _screenContentCss[i]; i++) {
            _screenContentCss[i].style.width = wContent + 10 + 'px';
        }

        var _WARP_SCROLLCss = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .WARP_SCROLL');
        for (let i = 0; _WARP_SCROLLCss[i]; i++) {
            _WARP_SCROLLCss[i].style.width = wScroll_warp + 100 + 'px';
        }

        var _ContentA1Css = document.querySelectorAll<HTMLElement>('.ContentA1');
        for (let i = 0; _ContentA1Css[i]; i++) {
            _ContentA1Css[i].style.width = wScroll_warp + 'px';
        }

        var _iitemA1Css = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .iitemA1');
        for (let i = 0; _iitemA1Css[i]; i++) {
            _iitemA1Css[i].style.width = iitemA1minwidth + 'px';
            _iitemA1Css[i].style.minWidth = iitemA1minwidth + 'px';
        }

        var _iitemA1UPCss = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .iitemA1UP');
        for (let i = 0; _iitemA1UPCss[i]; i++) {
            _iitemA1UPCss[i].style.width = iitemA1minwidth + 'px';
        }

        //line of delay
        var counDelay = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent.delay .iitemA1UP').length;
        var delaywidth = Math.floor((wContent / counDelay) * 10) / 10;

        var _delayiitemA1UPCss = document.querySelectorAll<HTMLElement>(
            '.contentScreen .screenContent.delay .iitemA1UP'
        );
        for (let i = 0; _delayiitemA1UPCss[i]; i++) {
            _delayiitemA1UPCss[i].style.width = delaywidth - 1 + 'px';
        }

        var _bg_CaseDelayCss = document.querySelectorAll<HTMLElement>(
            '.contentScreen .screenContent.delay .iitemA1UP .bg_CaseDelay'
        );
        for (let i = 0; _bg_CaseDelayCss[i]; i++) {
            _bg_CaseDelayCss[i].style.width = delaywidth - 1 + 'px';
        }

        //Resize Case_no
        var caseA1 = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .iitemA1UP .sttInLineUPA1');
        var caseA1LineUPA1Text = document.querySelectorAll<HTMLElement>(
            '.contentScreen .screenContent .iitemA1UP  .LineUPA1Text'
        );
        var caseA1IS_NEW_PART = document.querySelectorAll<HTMLElement>(
            '.contentScreen .screenContent .iitemA1UP  .IS_NEW_PART'
        );
        var wcolCase = 0;

        for (var i = 0; i < caseA1.length; i++) {
            wcolCase = Number(caseA1[i].getAttribute('tacktime')) * iitemA1minwidth;
            caseA1[i].style.width = wcolCase - 1 + 'px';
            caseA1LineUPA1Text[i].style.width = wcolCase - 1 + 'px';
            caseA1IS_NEW_PART[i].style.width = wcolCase - 1 + 'px';
            //  document.querySelector(caseA1[i]).parent().find(".LineUPA1Text").css({ "width": wcolCase - 1 });
            //  document.querySelector(caseA1[i]).parent().find(".IS_NEW_PART").css({ "width": wcolCase - 1});
        }

    }

    bindData(jdata) {
        var endtime;
        var starttime;
        var s;
        var ms;
        //this.clearStyle();
        starttime = new Date();
        endtime = new Date();
        s = endtime.getSeconds() - starttime.getSeconds();
        ms = endtime.getMilliseconds() - starttime.getMilliseconds();
        (document.querySelector<HTMLElement>('.message1')).innerHTML =  'Line Off : ' + s + 's.' + ms + 'ms'

        var starttime2 = new Date();

        this.bindDataTimeRun(jdata); //DELAY: bind thời gian delay và total delay

        endtime = new Date();
        s = endtime.getSeconds() - starttime2.getSeconds();
        ms = endtime.getMilliseconds() - starttime2.getMilliseconds();
        (document.querySelector<HTMLElement>('.message2')).innerHTML =  'U/P Progress: : ' + s + 's.' + ms + 'ms'

        s = endtime.getSeconds() - starttime.getSeconds();
        ms = endtime.getMilliseconds() - starttime.getMilliseconds();
        (document.querySelector<HTMLElement>('.message2')).innerHTML = 'ALL Progress: ' + s + 's.' + ms + 'ms - Request count: ' + this.Requestcount++ ;
    }

    bindDataTimeRun(jdata) {
       if(jdata.length > 0 ){
        //HIỂN THỊ CÁC CASE DELAY SECOND
        for (var i = 0; i < jdata.length; i++) {
            var casedelaycss = jdata[i].prodLine + jdata[i].noInDate + 'Delay';
            var delayconfirmcss = jdata[i].prodLine + jdata[i].noInDate + 'DelayUnpack';
            var casedelay = document.querySelector<HTMLElement>('.' + casedelaycss);
            var delayconfirm = document.querySelector<HTMLElement>('.' + delayconfirmcss);
            if (casedelay != null) { casedelay.innerHTML = this.toMMSS(jdata[i].delaySecond);}
            if (delayconfirm != null) { delayconfirm.innerHTML = this.toMMSS(jdata[i].delayConfirmSecond); }
        }

        var resetw1 = document.querySelectorAll<HTMLElement>('.sttInLineUPA1:not(.A1UnPacking) .A1TimeLine');
        if (resetw1.length > 0) {
            for (var i = 0; resetw1[i]; i++) {
                resetw1[i].style.width = 0 + 'px';
            }
        }
        document.querySelector<HTMLElement>('.dateWorking').innerHTML = jdata[0].workingDate + ' - Ca: ' + jdata[0].shift;
       }

    }

    bindScan(jdata, jdataScreen) {
        this.clearStyle();
        var v_bgcolor = jdataScreen.unpackDoneColor;
        this.line_off = Number( this.dataLookUpByDomain.filter((f) => f.itemCode.toUpperCase() == 'LINE_OFF')[0].itemValue);
        var countiitemA1 = document.querySelectorAll<HTMLElement>('.contentScreen .screenContent .iitemA1').length;
        document.querySelector('.contentScreen .HeadA1 .HeadA1Max span').innerHTML = this.dataUpPlanBaseToday[0].endNoInShift.toString();
        var maxTact = 0;

        for (var i = 0; i < jdata.length; i++) {
            if (jdata[i].prodLine == 'A2') {
                //
                var scanthis = document.querySelectorAll<HTMLElement>('.sttInLineA1_' + jdata[i].noInDate);
                var scops = document.querySelectorAll<HTMLElement>('.sttInLineA1');

                document.querySelector('.contentScreen .HeadA1 .HeadA1Text span').innerHTML = jdata[i].noInDate;
                var isExists = (scops.length > 0 ? true : false) && (scanthis.length > 0 ? true : false);

                if (isExists) {
                    for (var ind = 0; ind < scops.length; ind++) {
                        scops[ind].classList.add('sttScanInLineA1');
                        if (scops[ind].getAttribute('stt-index') == jdata[i].noInDate) {
                            break;
                        }
                    }
                }
                maxTact = jdata[i].noInDate;
                var _bgc = document.querySelectorAll<HTMLElement>('.sttScanInLineA1');
                for (let i = 0; _bgc[i]; i++) {
                    _bgc[i].style.backgroundColor = v_bgcolor;
                }
            }
        }

        //scroll
        var scroll_Tact = maxTact - Math.floor(this.line_off / 2) > 0 ? maxTact - Math.floor(this.line_off / 2) : 0;
        scroll_Tact = scroll_Tact + this.line_off <= countiitemA1 ? scroll_Tact : countiitemA1 - this.line_off;
        document
            .querySelector('.contentScreen .screenContent.scroll')
            .scrollTo(scroll_Tact * this.v_iitemA1minwidth, 0);
    }

    bindDelayCase(jdata) {
        var objCaseDelay = document.querySelectorAll<HTMLElement>('.screenContent.delay .iitemA1UP');
        for (let i = 0; objCaseDelay[i]; i++) {
            objCaseDelay[i].classList.remove('exists_data');
        }

        document.querySelector('.screenContent.delay .iitemA1UP .LineUPA1Text_DELAY span').innerHTML = '';
        var objDelayAttrNot = document.querySelectorAll<HTMLElement>('.screenContent.delay .iitemA1UP .LineUPA1Text_DELAY:not(.line2)');
        var objCaseNo = document.querySelectorAll<HTMLElement>('.screenContent.delay .iitemA1UP .LineUPA1Text_DELAY:not(.line2) span');
        var objDelay = document.querySelectorAll<HTMLElement>('.screenContent.delay .iitemA1UP .LineUPA1Text_DELAY.line2 span');

        if(jdata.length > 0)
        {
            for (var i = 0; i < jdata.length; i++) {
                if (i < objCaseDelay.length) {
                    var _caseno = jdata[i].caseNo.substring(0, 2) + jdata[i].caseNo.substring(3, 6);

                    var delayTT;
                    if (jdata[i].isCurrentDate == 1) {
                        delayTT = jdata[i].maxNoInDate - Math.ceil(jdata[i].maxNoOfALineIn);
                    } else {
                        var d = this.parseJsonDate(jdata[i].workingDate);
                        delayTT = d.toDateString().substring(4, 7) + ' ' + d.getDate();
                    }
                    objDelayAttrNot[i].getAttribute('data-details');
                    objCaseNo[i].innerHTML = _caseno;
                    objDelay[i].innerHTML = '(' + delayTT + ')';
                    objCaseDelay[i].classList.add('exists_data');
                }
            }

            for (var i = Number(jdata.length); i < objCaseNo.length - jdata.length; i++) {
                objCaseNo[i].innerHTML = '';
                objDelay[i].innerHTML = '';
            }
        }
        else{
            for (var i = 0; i < objCaseNo.length; i++) {
                    objCaseNo[i].innerHTML = '';
                    objDelay[i].innerHTML = '';
            }
        }
    }

    bindUnpackCase(jdata, jdataScreen) {

        var scops = document.querySelectorAll<HTMLElement>('.sttInLineUPA1');
        var isExists = scops.length > 0 ? true : false;
        if (!isExists) {
            return;
        }

        for (var ii = 0; ii < jdata.length; ii++) {
            if (jdata[ii].prodLine == 'A2') {
                continue;
            }
            var v_caseNo = jdata[ii].caseNo.replace('-','').replace('/','')
            if (jdata[ii].status == 70) {
                // Đã dỡ xong
                var css70 = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
                 //clean
                var clearFinish = document.querySelectorAll<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1.finish');
                if(clearFinish){
                    for (let i = 0; clearFinish[i]; i++) {
                        css70[i].classList.remove('finish')
                    }
                }
                //add color
                if(css70){
                     css70.classList.add('finish')
                     css70.style.backgroundColor = jdataScreen.unpackDoneColor;
                    }
                if ( this.soundstart == 'true' && this.soundtype == 'DELAY_UNPACKING_A1' && this.caseno_delay == jdata[ii].caseNo ) { this.stopAudio(); }

            } else if (jdata[ii].status == 65) {
                // Delay finish Unpacking
                var ttw = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
                 //clean
                var clearDelayFinish = document.querySelectorAll<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1.delayFinishUnpacking');
                if(clearDelayFinish){
                    for (let i = 0; clearDelayFinish[i]; i++) {
                        ttw[i].classList.remove('delayFinishUnpacking')
                    }
                }
                //add color
                if(ttw){
                    ttw.classList.add('delayFinishUnpacking')
                    ttw.style.backgroundColor = jdataScreen.delayUnpackColor;
                    }
                if (jdataScreen.delayUnpackSound != '' && this.soundstart == "false") {
                    this.playAudio(jdataScreen.delayUnpackSound, jdataScreen.delayUnpackPlaytime, "DELAY_UNPACKING_A1", jdata[ii].caseNo);
                }

            } else if (jdata[ii].unpackingStartDatetime != null && jdata[ii].status == 60) {
                // Đã bắt đầu Start Unpacking
                var css60 = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
                 //clean
                var clearA1UP = document.querySelectorAll<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1.A1UnPacking');
                if(clearA1UP){
                    for (let i = 0; clearA1UP[i]; i++) {
                        css60[i].classList.remove('A1UnPacking')
                    }
                }
                //add color
                if(css60){
                    css60.classList.add('A1UnPacking');
                    css60.style.backgroundColor = '#99CCFF';
                }

                var css60a = document.querySelector<HTMLElement>('.A1TimeLine.' + v_caseNo);
                if(css60a){
                    css60a.classList.add('A1UnPacking');
                    css60a.style.backgroundColor = jdataScreen.delayUnpackColor;
                }

                if (this.soundstart == "true" && this.soundtype == "CONFIRM_UNPACKING_A1" && this.caseno_delay == jdata[ii].caseNo) { this.stopAudio(); }
                if (this.soundstart == "true" && this.soundtype == "DELAY_START_A1" && this.caseno_delay == jdata[ii].caseNo) { this.stopAudio(); }

            } else if (jdata[ii].status == 50) {
                // Đã xác nhận đưa lên bàn xoay (confirm cờ)
                var css50 = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
                //clean
                var clearConfirmUP = document.querySelectorAll<HTMLElement>('.sttInLineUPA1.A1ConfirmUnPacking');
                if(clearConfirmUP){
                    for (let i = 0; clearConfirmUP[i]; i++) {
                        css50[i].classList.remove('A1ConfirmUnPacking')
                    }
                }
               //add color
                if(css50) {
                    css50.classList.add('A1ConfirmUnPacking');
                    css50.style.backgroundColor = '#99CCFF';
                }

                if (this.soundstart == "true" && this.soundtype == "CONFIRM_UNPACKING_A1" && this.caseno_delay == jdata[ii].caseNo) { this.stopAudio(); }
                if (this.soundstart == "true" && this.soundtype == "BEFORE_TACKTIME_A1" && this.caseno_delay == jdata[ii].caseNo) { this.stopAudio(); }

                if (jdata[ii].delayConfirmFlag != null && jdataScreen.confirmFlagSound != '' && this.soundstart == "false") { //Đến thời điểm cần Start Unpacking
                        this.playAudio(jdataScreen.confirmFlagSound, jdataScreen.confirmFlagPlaytime, "DELAY_START_A1", jdata[ii].caseNo);
                }

            } else if (jdata[ii].status == 40) {
                // Đã bắt đầu tính Delay Confirm Unpacking
                let css40 = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
                 //clean
                var clearWaitCfUP = document.querySelectorAll<HTMLElement>('.sttInLineUPA1.A1WaitConfirmUnPacking');
                if(clearWaitCfUP){
                    for (let i = 0; clearWaitCfUP[i]; i++) {
                        css40[i].classList.remove('A1WaitConfirmUnPacking')
                    }
                }
                //add color
                if(css40) {
                    css40.classList.add('A1WaitConfirmUnPacking');
                    css40.style.backgroundColor = jdataScreen.confirmFlagColor;
                }
                if (this.soundstart == "true" && this.soundtype == "BEFORE_TACKTIME_A1" && this.caseno_delay == jdata[ii].caseNo) { this.stopAudio(); }
                if (jdataScreen.confirmFlagSound != '' && this.soundstart == "false") {

                        this.playAudio(jdataScreen.confirmFlagSound, jdataScreen.confirmFlagPlaytime, "CONFIRM_UNPACKING_A1", jdata[ii].caseNo);
                }

            } else if (jdata[ii].unpackingStartDatetime == null && jdata[ii].status == 30) {
                // Trạng thái cảnh báo hồng chuẩn bị dỡ tiếp theo
                var css30 = document.querySelector<HTMLElement>('.' + v_caseNo + '.sttInLineUPA1');
               //clean
                var clearNextUP = document.querySelectorAll<HTMLElement>('.sttInLineUPA1.A1NextUnPacking');
                if(clearNextUP){
                    for (let i = 0; clearNextUP[i]; i++) {
                        css30[i].classList.remove('A1NextUnPacking')
                    }
                }
                //add color
                if(css30) {
                     css30.classList.add('A1NextUnPacking');
                     css30.style.backgroundColor = jdataScreen.beforeTacktimeColor;
                    }
                if (jdataScreen.beforeTacktimeSound != '' && this.soundstart == "false") {
                    this.playAudio(jdataScreen.beforeTacktimeSound, jdataScreen.beforeTacktimePlaytime, "BEFORE_TACKTIME_A1", jdata[ii].caseNo);
                }

            } else if (jdata[ii].unpackingStartDatetime == null && jdata[ii].status == 2) {
                // Trạng thái mặc định không làm gì
            }
        }
    }

    bindCALL_LEADER(jdataScreen, jdata) {
        var is_call_leader = 1;
        for (var i = 0; i < jdata.length; i++) {
            if (jdata[i].itemCode == 'SCREEN_CALL_LEADER_V2') {
                is_call_leader = jdata[i].itemValue;
                break;
            }
        }

        if (is_call_leader == 0) {   //trạng thái gọi leader
            if (jdataScreen.callLeaderColor != '' && this.soundstart == "false") {
                this.playAudio(jdataScreen.callLeaderSound, jdataScreen.callLeaderPlaytime, "CALL_LEADER", "");
            }
        } else {
            if (this.soundstart == "true" && this.soundtype == "CALL_LEADER") { this.stopAudio(); }

            //Stop Sound
        }
    }

    bindSTOP_PAUSE(jdata) {
        //Status Screen
        var screenStatus = document.getElementById('screen-status');

        if (jdata.length > 0) {
            if (jdata[0].screenStatus == 'STOPED') {
                screenStatus.innerHTML = jdata[0].screenStatus.replace('ED', '');
                screenStatus.style.display = 'block';
            } else if (jdata[0].screenStatus == 'PAUSED') {
                screenStatus.innerHTML = jdata[0].screenStatus.replace('D', '');
                screenStatus.style.display = 'block';
            } else {
                screenStatus.style.display = 'none';
            }
        }

    }

    clearStyle() {
        var clear1 = document.querySelectorAll<HTMLElement>('.iitemA1UP:not(.A1UnPacking) .A1TimeLine');
        if(clear2){
            for (let i = 0; clear1[i]; i++) {
                clear1[i].style.width = 0 + 'px';
                clear1[i].style.backgroundColor = '';
            }
        }

        var clear2 = document.querySelectorAll<HTMLElement>('.sttInLineA1');
        if(clear2){
            for (let i = 0; clear2[i]; i++) {
                clear2[i].style.backgroundColor = '';
                clear2[i].classList.remove('sttScanInLineA1');
            }
        }
    }

    parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

    toMMSS(ttseconds) {
        var sec_num = parseInt(ttseconds, 0); // don't forget the second param
        var minutes = Math.floor(sec_num / 60);
        var seconds = sec_num - minutes * 60;

        if (minutes < 10) {
            minutes = Number('0' + minutes);
        }
        if (seconds < 10) {
            seconds = Number('0' + seconds);
        }
        var time = minutes + ':' + seconds;
        return time;
    }

    //#region Sound
    v_urlsourd;
    playAudio(urlsound, tacktime, stype, caseno) {
        // if (this.soundstart == 'false') {
        //     this.soundphaytime = 0;
        //     this.soundtype = stype;
        //     //Chạy Sound

        //     let audio = new Audio(urlsound);
        //     audio.currentTime = tacktime;
        //     audio.loop = true;
        //     audio.muted = false;
        //     audio.load();
        //    // audio.play();
        //     this.v_urlsourd = urlsound;

        //     this.soundstart = 'true';

        // } else if (this.soundtype == stype && this.soundstart == 'true' && this.soundphaytime <= tacktime) {
        //     this.soundphaytime++;
        //     this.playAudio(urlsound, tacktime, this.soundtype, caseno);

        // } else {
        //     this.stopAudio();
        // }
    }

    stopAudio(){
        // let audio = new Audio(this.v_urlsourd);
        // audio.pause();
    }
    //#endregion
}
