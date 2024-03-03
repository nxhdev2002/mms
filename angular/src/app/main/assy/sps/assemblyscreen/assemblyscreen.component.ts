import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
/* tslint:disable */
/* eslint-disable */
//add Service
import {
    AsyAdoAssemblyScreenServiceProxy,
    AsyAdoAssemblyScreenDataOutputDto,
    AsyAdoAssemblyScreenConfigOutputDto
} from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//form popup


@Component({
    templateUrl: './assemblyscreen.component.html',
    styleUrls: [
        './assemblyscreen.component.less',
    ],
})

export class AssemblyScreenComponent extends AppComponentBase {

    datePipe = new DatePipe('en-US');

    rowData: AsyAdoAssemblyScreenDataOutputDto[] = []
    configData: AsyAdoAssemblyScreenConfigOutputDto


    SCREEN_CODE: string;


    constructor(
        injector: Injector,
        private _serviceProxy: AsyAdoAssemblyScreenServiceProxy,
    ) {
        super(injector);
        console.log('1: constructor');

        this.getParameter();

        this._serviceProxy.getScreenConfig(this.SCREEN_CODE)
            .subscribe((result) => {
                if (result) {

                    this.configData = result[0];

                    console.log(this.configData)
                        setTimeout(() => {
                            this.drawScreen();
                        }, 500);
                }
            });
    }



    ngOnInit(): void {
        console.log('2: ngOnInit');
    }

    ngAfterViewInit() {
        console.log('3: ngAfterViewInit');
        this.repeatBindData();
    }

    @HostListener('load', ['$event'])
    onLoad() {
        console.log('4: ngAfterViewInit');
        // this.drawScreen();
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
        this.SCREEN_CODE = urlParams.get('screen_code');
    }

    drawScreen() {

        ///////////////////////////design width
        let w = window.innerWidth;

        let col_process = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .prosses_item');
        if(col_process.length > 0) {

            let w_col_process = Math.floor((w / col_process.length) * 100) / 100;

            for(let i=0; i < col_process.length  ; i++){
                col_process[i].style.width = w_col_process + 'px';
            }

            let _img = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .prosses_item .img_line ');
            for(let i=0; i < _img.length; i++){
                _img[i].style.width = (w_col_process -1) + 'px';
            }
        }


        ///////////////////////////design height
        let h = window.innerHeight - 1;


        let row_item = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .prosses_item.index_0 ._itemFor ._item');
        if(row_item.length > 0){

            let h_row_item = Math.floor((h / row_item.length) * 100) / 100;

            let obj_row_item = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .prosses_item ._item, ' +
                                                                                                             '.PTA .PTA_Content .prosses_item ._item .header_title, ' +
                                                                                                             '.PTA .PTA_Content .prosses_item ._item .header_title .img_line');
            for(let i=0;i <  obj_row_item.length; i++){
                obj_row_item[i].style.height = (h_row_item - 1) + 'px';
                obj_row_item[i].style.lineHeight = (h_row_item - 1) + 'px';
            }

        }

    }


    getData() {

        if(this.configData){
            this._serviceProxy.getScreenData(this.configData.prodLine)
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
        else{
            this.timer_repeat = setTimeout(() => {
                this.repeatBindData();
            }, this.secondDelay);
        }
    }

    timecount:number = 0;
    refeshPage: number = 600; // 10phut
    timer_repeat;
    secondDelay = 1000;
    fn: CommonFunction = new CommonFunction();
    repeatBindData(){

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
    BindData(_data: AsyAdoAssemblyScreenDataOutputDto[]){

        if(_data){
            _data.forEach(_row => {
                let _item = document.querySelector<HTMLElement>('.PTA .PTA_Content .prosses_item.' + _row.line + '.' + _row.process);
                if(_item){

                    //clear
                    let _val = _item.querySelectorAll<HTMLElement>("._item .val");
                    for(let i=0;i <  _val.length; i++){  _val[i].textContent = ""; }
                    _item.style.backgroundColor = 'transparent';


                    //background

                    _item.style.backgroundColor = this.getcolor(_row.modelName.toLowerCase());
                    //
                    let _grade = _item.querySelector<HTMLElement>("._item._GRADE .val");
                    if(_grade) _grade.textContent = _row.grade;

                    let _model = _item.querySelector<HTMLElement>("._item._MODEL .val");
                    if(_model) _model.textContent = _row.modelName;

                    let _seq = _item.querySelector<HTMLElement>("._item._SEQUENCE .val");
                    if(_seq) _seq.textContent = _row.seqNo;

                    let _body = _item.querySelector<HTMLElement>("._item._BODY .val");
                    if(_body) _body.textContent = _row.body;

                    let _lotno = _item.querySelector<HTMLElement>("._item._LOTNO .val");
                    if(_lotno) _lotno.textContent = _row.lotNo;

                    let _color = _item.querySelector<HTMLElement>("._item._COLOR .val");
                    if(_color) _color.textContent = _row.color;
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


    _IsShowLeftTitle(){
        if(this.configData){
            return this.configData.isShowLeftTitle == 'Y'
        }
        return false;
    }

    _detect_headerText(i:number){
        let _headernames = this.configData.headerList .split(',')[i].split('/');
        return _headernames.length;
    }

}
