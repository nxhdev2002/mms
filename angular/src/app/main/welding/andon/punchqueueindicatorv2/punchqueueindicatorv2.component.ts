import { DatePipe } from '@angular/common';
import { Component, HostListener, Injector, } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
/* tslint:disable */
/* eslint-disable */
//add Service

import {  WldAdoPunchQueueIndicatorDto, WldAdoPunchQueueIndicatorServiceProxy } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//form popup
@Component({
    templateUrl: './punchqueueindicatorv2.component.html',
    styleUrls: ['./punchqueueindicatorv2.component.less'],
})
export class Punchqueueindicatorv2Component extends AppComponentBase {

    isLoading: boolean = false;
    rowData: WldAdoPunchQueueIndicatorDto[];
    datePipe = new DatePipe('en-US');

    constructor(
        injector: Injector,
        private _serviceProxy: WldAdoPunchQueueIndicatorServiceProxy
    ) {
        super(injector);
        console.log('1: constructor');

        this.getParameter();

    }

    ngOnInit(): void {
        console.log('2: ngOnInit');
        this.drawScreen();
    }

    ngAfterViewInit() {
        console.log('3: ngAfterViewInit');
        this.repeatBindData();
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        //alert('4: onWindowResize')
        this.drawScreen();
        // this.notify.success('Height: ' + hButton);
    }
    ////end set width height


    ngOnDestroy(): void {

        // this.timerstorage = null;
        clearTimeout(this.timer_repeat);
    }


    getParameter() {
        let urlParams = new URLSearchParams(window.location.search);
        // this.SCREEN_CODE = urlParams.get('screen_code');
    }



    drawScreen() {

        ///////////////////////////design width
        let w = window.innerWidth - 0;
        let wItem = Math.floor((w / 6) * 100) / 100;

        let _bTopTitle = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel .pTopItem.Title');
        if(_bTopTitle) _bTopTitle.style.width = wItem*4 + 'px';

        let _bTopBodyCount = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel .pTopItem.BodyCount');
        if(_bTopBodyCount) _bTopBodyCount.style.width = wItem*2 + 'px';

        let _pBotItem = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBot .pBotItem');
        for(let i=0; _pBotItem[i]; i++){
            _pBotItem[i].style.width = wItem + 'px';
        }

        let _pBodyCol = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem, .PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item');
        for(let i=0; _pBodyCol[i]; i++){
            _pBodyCol[i].style.width = wItem*2 + 'px';
        }


        let _pBTitleNext = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyTitle .pBTitle.Next');
        if(_pBTitleNext) _pBTitleNext.style.width = wItem*2 + 'px';
        let _pBTitleCurrent = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyTitle .pBTitle.Current');
        if(_pBTitleCurrent) _pBTitleCurrent.style.width = wItem*4 + 'px';

        let _pTopItemItem = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pTop .pTopItem .pTBItem ');
        for(let i=0; _pTopItemItem[i]; i++){
            _pTopItemItem[i].style.width = (wItem) + 'px';
        }


        ///////////////////////////design height
        let h = window.innerHeight - 0;
        let hItem = Math.floor((h / 13) * 100) / 100;

        let _pTopItem = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pTop .pTopItem, .PTA .PTA_Content .bPanel.pBot .pBotItem');
        for(let i=0; _pTopItem[i]; i++){
            _pTopItem[i].style.height = (hItem*2) + 'px';
        }

        let _pBTitle = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyTitle .pBTitle');
        for(let i=0; _pBTitle[i]; i++){
            _pBTitle[i].style.height = (hItem) + 'px';
            _pBTitle[i].style.lineHeight = (hItem) + 'px';
        }

        // let _pBodyCol = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem');
        for(let i=0; _pBodyCol[i]; i++){
            _pBodyCol[i].style.height = (hItem*4) + 'px';
            _pBodyCol[i].style.lineHeight = (hItem*4) + 'px';
        }

        //let _pTopItemItem = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pTop .pTopItem');
        for(let i=0; _pTopItemItem[i]; i++){
            _pTopItemItem[i].style.height = (hItem*2) + 'px';
            _pTopItemItem[i].style.lineHeight = (hItem*2) + 'px';
        }

        let _pBodyColItemTit = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.title');
        for(let i=0; _pBodyColItemTit[i]; i++){
            _pBodyColItemTit[i].style.height = (hItem) + 'px';
            _pBodyColItemTit[i].style.lineHeight = (hItem) + 'px';
        }
        let _pBodyColItemVal = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.value');
        for(let i=0; _pBodyColItemVal[i]; i++){
            _pBodyColItemVal[i].style.height = (hItem*3) + 'px';
            _pBodyColItemVal[i].style.lineHeight = (hItem*3) + 'px';
        }

    }


    getData() {

            this._serviceProxy.getWldAdoPunchQueueIndicatorV2()
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                if (result) {
                    this.rowData = result;
                    this.BindData(this.rowData);
                }


                this.timer_repeat = setTimeout(() => {
                    this.repeatBindData();
                }, this.secondDelay);
            },(error) => {
                this.timer_repeat = setTimeout(() => {
                    this.repeatBindData();
                }, this.secondDelay);
        });
    }

    timecount:number = 0;
    refeshPage: number = 600; // 10phut
    timer_repeat;
    secondDelay = 1000;
    fn: CommonFunction = new CommonFunction();
    repeatBindData(){
        //phuongdv
        if (this.timecount > this.refeshPage) window.location.reload();
        this.timecount = this.timecount + 1;
        try{

            this.getData();
            this.fn.showtime('time_now_log');

        }catch(ex){
            console.log(ex);

            this.timer_repeat = setTimeout(() => {
                this.repeatBindData();
            }, this.secondDelay);
        }


    }

/*
    camry : #FF99CC
    vios : #FABF8F
    avanza : #8DB4E2
    fortuner : #FFFF00
    innova : #C8C09D
*/
    BindData(_data: WldAdoPunchQueueIndicatorDto[]){

        //clear
        let _itemclear = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem.LOT_NO, ' +
                                                                '.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem.BODY_NO, ' +
                                                                '.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.punchIndicator, ' +
                                                                '.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.wTalkTime ');
        for(let i=0;_itemclear[i]; i++){  _itemclear[i].textContent = ""; }
        let _currentColor = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent.Current');
        _currentColor.classList.remove("YELLOW","GREEN");


        if(_data){
            _data.forEach(_row => {
                let _item = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent.Seq'+_row.seq+'.Filler'+_row.filler);
                if(_item){

                    let _lot_no = _item.querySelector<HTMLElement>('.pBCItem.LOT_NO');
                    _lot_no.textContent = _row.lotNo;
                    let _body_no = _item.querySelector<HTMLElement>('.pBCItem.BODY_NO');
                    _body_no.textContent = _row.bodyNo;
                }
                if (_row.seq == 3 && _row.filler == '3') {
                    let _punchIndicator = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.punchIndicator');
                    _punchIndicator.textContent = _row.punchIndicator;

                    let _color = '';
                    if(_row.weldSignal == "CALL") _color = 'YELLOW';
                    if(_row.weldSignal == "CF") _color = 'GREEN';

                }
                if (_row.seq == 4 && _row.filler == '4') {
                    let _wTalkTime = document.querySelector<HTMLElement>('.PTA .PTA_Content .bPanel.pBody .pBodyline .pBContent .pBCItem .TT1Item.wTalkTime');
                    _wTalkTime.textContent = this.fn.toMMSS(_row.wTalkTime);
                }

            });
        }
    }

    getcolor(m) {
        //alert(m);     - phuongdv
        if (m == "camry") {
            return "#FF99CC";
        } else if (m == "vios") {
            return "#FABF8F";
        } else if (m == "avanza") {
            return "#8DB4E2";
        } else if (m == "fortuner") {
            return "#ff0";
        } else if (m == "innova") {
            return "#c8c09d";
        }

        return "";
    }

}
