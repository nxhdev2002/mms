import { style } from '@angular/animations';
import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

//add Service
import { LgaBp2ProgressScreenServiceProxy, LgaBp2ProgressScreenConfigOutputDto } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//form popup


@Component({
    templateUrl: './LgaBp2ProgressScreen.component.html',
    styleUrls: [
        './LgaBp2ProgressScreen.component.less',
    ],
})

export class LgaBp2ProgressScreenComponent extends AppComponentBase {

    datePipe = new DatePipe('en-US');

    rowData: LgaBp2ProgressScreenConfigOutputDto[] = []
    configData: LgaBp2ProgressScreenConfigOutputDto[] = []

    ecar_tmp: LgaBp2ProgressScreenConfigOutputDto[] = []
    ecars: LgaBp2ProgressScreenConfigOutputDto[] = []
    prodLine: string = '';
    HearderTitle: string = '';

    constructor(
        injector: Injector,
        private _serviceProxy: LgaBp2ProgressScreenServiceProxy,
    ) {
        super(injector);
        console.log('1: constructor');

        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');

        this._serviceProxy.getScreenConfig(this.prodLine)
        .subscribe((result) => {
            if (result) {
                this.configData = result;
                this.HearderTitle = result[0].title;

                let tmpECarId = 0;
                for(let i = 0; this.configData[i]; i++){
                    if(tmpECarId != this.configData[i].ecarId){
                        tmpECarId  = this.configData[i].ecarId;
                        this.ecar_tmp.push(this.configData[i]);
                    }
                }
                this.ecars = this.ecar_tmp;

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


    _processFullwidth: number = 0;
    drawScreen() {

        ///////////////////////////design width
        let w = window.innerWidth - 1;
        let w_col = Math.floor((w / 4) * 100) / 100;
        let w_number = Math.floor((w_col / 3) * 100) / 100;
        let w_ecar = Math.floor((w_col * 3) * 100) / 100;
        // let  w_car_del = Math.floor((w_ecar /2) * 100) / 100;



        let _header_val = document.querySelector<HTMLElement>('.PTA .HEADER .val');
        if(_header_val) _header_val.style.width = (w - 4) + 'px';



        let _process_panel_box = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_panel_box, .PTA .ECAR_CONTENT .ecar_item .ecar_panel_box');
        for(let i=0; _process_panel_box[i]; i++){
            _process_panel_box[i].style.width = (w_col - 4) + 'px';
        }


        //
        let _ecar_name = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_NAME, .PTA  .PROGRESS_BAR .process_name');
        for(let i=0; _ecar_name[i]; i++){
            _ecar_name[i].style.width = (w_number*2 - 1) + 'px';
        }

        //
        let _ecar_number = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_NUMBER,  .PTA  .PROGRESS_BAR .process_number');
        for(let i=0; _ecar_number[i]; i++){
            _ecar_number[i].style.width = (w_number - 1) + 'px';
        }

        //
        this._processFullwidth = (w_col*3 - 0) ;
        let _ecar_delivery = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_DELIVERY');
        for(let i=0; _ecar_delivery[i]; i++){
            _ecar_delivery[i].style.width = (w_col*3 - 0) + 'px';

            let _ecar_delivery_process = _ecar_delivery[i].querySelectorAll<HTMLElement>('.ecar_process');
            let _ecar_delivery_process_count = _ecar_delivery_process.length;
            let  w_car_del = Math.floor((w_ecar /_ecar_delivery_process_count) * 100) / 100;

            let _ecar_del = _ecar_delivery[i].querySelectorAll<HTMLElement>('.ecar_process, .ecar_process .ecar_process_bg');
            for(let i=0; _ecar_del[i]; i++){
                _ecar_del[i].style.width = (w_car_del - 4) + 'px';
            }
        }

        let _process_content = document.querySelector<HTMLElement>('.PTA  .PROGRESS_BAR .process_content');
        if(_process_content) _process_content.style.width = (w_col*3 ) + 'px';

        //
        // let _ecar_del = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process, .PTA  .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process .ecar_process_bg');
        // for(let i=0; _ecar_del[i]; i++){
        //     _ecar_del[i].style.width = (w_car_del - 4) + 'px';
        // }

        //
        this._processFullwidth = this._processFullwidth -4;
        let _process_warp = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_content .process_warp, .PTA  .PROGRESS_BAR .process_content .process_val');
         for(let i=0; _process_warp[i]; i++){
            _process_warp[i].style.width =this._processFullwidth + 'px';
        }

        ///////////////////////////design height
        let h = window.innerHeight - 5;
        let c_car = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_for .ecar_item');

        let c_car_count = (c_car.length * 2) + 3;
        let h_car1 = Math.floor((h / c_car_count) * 100) / 100;
        let h_car2 = Math.floor((h_car1 * 2) * 100) / 100;

        //
        let _car = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item, .PTA .ECAR_CONTENT .ecar_item .ecar_panel, ' +
                                                                                          '.PTA .PROGRESS_BAR, .PTA .PROGRESS_BAR .process_panel');
        for(let i=0; _car[i]; i++){
            _car[i].style.height = (h_car2 - 0) + 'px';
        }

        let _car_del = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ecar_process, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ecar_process .ecar_process_bg, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ECAR_NAME, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ECAR_NUMBER, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel_box, ' +
                                                                                                '.PTA .PROGRESS_BAR .process_panel_box,  '+
                                                                                                '.PTA .PROGRESS_BAR .process_panel_box .process_name,  '+
                                                                                                '.PTA .PROGRESS_BAR .process_panel_box .process_number,  '+
                                                                                                '.PTA .PROGRESS_BAR .process_panel .process_warp, '+
                                                                                                '.PTA .PROGRESS_BAR .process_panel .process_run');
        for(let i=0; _car_del[i]; i++){
            _car_del[i].style.height = (h_car2 - 3) + 'px';
        }

        //
        let _hearder = document.querySelector<HTMLElement>('.PTA .HEADER');
        if(_hearder) _hearder.style.height = (h_car1 - 0) + 'px';
        let _hearder_val = document.querySelector<HTMLElement>('.PTA .HEADER .val');
        if(_hearder_val) _hearder_val.style.height = (h_car1 - 3) + 'px';

        let _sub_number = document.querySelectorAll<HTMLElement>('.PTA .PROGRESS_BAR .process_panel_box .process_number .sub_number');
        let h_car3 = (h_car2 - 3)/2;
        for(let i=0; _sub_number[i]; i++){
            _sub_number[i].style.height = h_car3 + 'px';
        }

        //
        let _img = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ecar_img_warning');
        let h_car4 = (h_car2 - 3)/3;
        for(let i=0; _img[i]; i++){
            _img[i].style.height = h_car4 + 'px';
            _img[i].style.left = '-' + (h_car4/2 + 5) + 'px'
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

    getData(){
        this._serviceProxy.getScreenData(this.prodLine)
        .subscribe((result) => {
            if (result) {
                this.rowData = result;
                this.bindData(this.rowData);
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

    bindData(_data: LgaBp2ProgressScreenConfigOutputDto[]){

        if(_data.length > 0){
            let _row = _data[0];
            //actual vol count
            let _act_vol_count = document.querySelector<HTMLElement>(".PTA .PROGRESS_BAR .process_number .sub_number.ACT");
            if(_act_vol_count) _act_vol_count.textContent = _row.actualVolCount.toString();
            //plan vol count
            let _plan_vol_count = document.querySelector<HTMLElement>(".PTA .PROGRESS_BAR .process_number .sub_number.PLAN");
            if(_plan_vol_count) _plan_vol_count.textContent = _row.planVolCount.toString();

            //process number = plantrim - actualtrim
            let _process_countdown = _row.planTrim - _row.actualTrim;
            let _process_countdown_number = document.querySelector<HTMLElement>(".PTA .PROGRESS_BAR .process_panel .process_val");
            if(_process_countdown_number) _process_countdown_number.textContent = _process_countdown.toString();

            //process countdown with
            //this._processFullwidth
            let _per_number = this.fn.getPercentByQty(_row.planTrim, _row.actualTrim);
            let _width_by_per_number = this.fn.getQtyByPercent2(this._processFullwidth, _per_number);

            let _process_run = document.querySelector<HTMLElement>(".PTA .PROGRESS_BAR .process_panel .process_run");
             if(_process_run)  _process_run.style.width = _width_by_per_number + "px";

             if(_row.screenStatus == "PAUSED") {
                document.querySelector<HTMLElement>(".POPUP_PAUSE").style.display = "flex";
             }
             else {
                document.querySelector<HTMLElement>(".POPUP_PAUSE").style.display = "none";
             }

        }

        let _LINE_ECAR_TMP = '';
        let _FINISIH_TMP = "";
        for(let i =0; _data[i]; i++){

            let _row = _data[i];
            let _LINE_ECAR = _row.prodLine + "_" + _row.ecarName.replace(' ','_');

            if (_LINE_ECAR_TMP == _LINE_ECAR && (_FINISIH_TMP != "FINISIHED" && _FINISIH_TMP != "DELAYED") ) continue;  //pk đã finished,DELAYED thì mới chạy tiếp đến del
            if (_LINE_ECAR_TMP != _LINE_ECAR ) _LINE_ECAR_TMP = _LINE_ECAR;                // sang ecar mới

            let _ecar = document.querySelector<HTMLElement>(".ECAR_CONTENT .ECAR_" + _row.ecarId);

            if(_ecar){
                //ecar finish count
                let _ecar_count = _ecar.querySelector<HTMLElement>(".ECAR_NUMBER .val");
                if(_ecar_count) _ecar_count.textContent = _row.ecarCount.toString();

                //ecar status
                let _ecar_status = _ecar.querySelector<HTMLElement>(".ECAR_DELIVERY .ecar_process." + _row.processName);
                if(_ecar_status)  {
                    //clear -phuongdv
                    _ecar_status.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
                    let _status = this.fn.isStatus(_row.status,"");

                    if(_status != "") _ecar_status.classList.add(_status);


                    //is delay - clear
                    let _isdelay = _ecar_status.querySelectorAll<HTMLElement>(".ecar_img_warning");
                    for(let i=0; _isdelay[i]; i++){
                        _isdelay[i].classList.remove("ACTIVE");
                    }

                    let isdelay = this.fn.isStatus(_row.isDelayStart,"");
                    if(isdelay == "Y") { _ecar_status.querySelector<HTMLElement>(".ecar_img_warning.RED_Y").classList.add("ACTIVE"); }
                    if(isdelay == "N") { _ecar_status.querySelector<HTMLElement>(".ecar_img_warning.YELLOW_N").classList.add("ACTIVE"); }

                    _FINISIH_TMP = _status;
                }
            }
        };
    }


    ecarProcess(ecarId:number){
        return this.configData.filter(a => a.ecarId === ecarId);
    }

}
