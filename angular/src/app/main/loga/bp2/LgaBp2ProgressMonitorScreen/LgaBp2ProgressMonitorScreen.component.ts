import { style } from '@angular/animations';
import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

//add Service
import { LgaBp2ProgressScreenServiceProxy, LgaBp2ProgressScreenConfigOutputDto, LgaBp2ProgressMonitorScreenDto } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//form popup


@Component({
    templateUrl: './LgaBp2ProgressMonitorScreen.component.html',
    styleUrls: [
        './LgaBp2ProgressMonitorScreen.component.less',
    ],
})

export class LgaBp2ProgressMonitorScreenComponent extends AppComponentBase {

    datePipe = new DatePipe('en-US');

    configData: LgaBp2ProgressScreenConfigOutputDto[] = []
    ecar_tmp: LgaBp2ProgressScreenConfigOutputDto[] = []
    ecars: LgaBp2ProgressScreenConfigOutputDto[] = []

    prodLine: string = '';
    HearderTitle: string = '';
    TotalCycle:number = 0;

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
                this.TotalCycle = result[0].totalCycle;
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

        let w_number = Math.floor((w_col / 4) * 100) / 100;
        let w_ecar = Math.floor((w_col * 3) * 100) / 100;
        // let  w_car_del = Math.floor((w_ecar /2) * 100) / 100;
        this._processFullwidth = (w_ecar - 0) ;

        // width header
        let _header_val = document.querySelector<HTMLElement>('.PTA .HEADER .val');
        if(_header_val) _header_val.style.width = (w - 6) + 'px';

        //header txt1
        let _header_txt1 = document.querySelector<HTMLElement>('.PTA  .HEADER .txt1');
        if(_header_txt1) _header_txt1.style.width = (w_col - 4) + 'px';

        let _header_txt2 = document.querySelector<HTMLElement>('.PTA  .HEADER .txt2');
        if(_header_txt2) _header_txt2.style.width =  (w - 6) - (w_col) + 'px';

        // width process left
        let _process_panel_box = document.querySelector<HTMLElement>('.PTA  .PROGRESS_BAR .process_panel_box');
        if(_process_panel_box) _process_panel_box.style.width = (w_col - 4) + 'px';

        //width process left  - name
        let _process_name = document.querySelector<HTMLElement>('.PTA  .PROGRESS_BAR .process_name');
        if(_process_name) _process_name.style.width = (w_number*3) + 'px';


        //width process left  - number
        let _process_number = document.querySelector<HTMLElement>('.PTA  .PROGRESS_BAR .process_number');
        if(_process_number) _process_number.style.width = (w_number ) + 'px';

        //width process right  - content
        let _process_content = document.querySelector<HTMLElement>('.PTA  .PROGRESS_BAR .process_content');
        if(_process_content) _process_content.style.width = (w_col*3 - 2) + 'px';

        let _process_warp = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_content .process_warp, .PTA  .PROGRESS_BAR .process_content .process_run');
        // if(_process_warp) _process_warp.style.width = this._processFullwidth - 6 + 'px';
        for(let i=0; _process_warp[i]; i++){
            _process_warp[i].style.width = this._processFullwidth - 6 + 'px';
        }


        this._processFullwidth = this._processFullwidth - 6;

        //eff
        let _eff = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .EFF, .PTA  .ECAR_CONTENT .EFF .val');
        for(let i=0; _eff[i]; i++){
            _eff[i].style.width = (w_number - 4) + 'px';
        }


        //ecar title - number
        let _ecar_number = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_NUMBER');
        for(let i=0; _ecar_number[i]; i++){
            _ecar_number[i].style.width =(w_number - 4) + 'px';
        }

        //ecar title - name
        let _ecar_name = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_NAME');
        for(let i=0; _ecar_name[i]; i++){
            _ecar_name[i].style.width = (w_number*2 - 4) + 'px';
        }


        //
        let _ecar_delivery = document.querySelectorAll<HTMLElement>('.PTA  .ECAR_CONTENT .ecar_item .ECAR_DELIVERY');
        for(let i=0; _ecar_delivery[i]; i++){
            _ecar_delivery[i].style.width = (w_ecar - 6) + 'px';

            let _ecar_del = _ecar_delivery[i].querySelectorAll<HTMLElement>('.ecar_process, .ecar_process .ecar_process_bg, .ecar_process, .ecar_process .ecar_process_task');
            for(let i=0; _ecar_del[i]; i++){
                _ecar_del[i].style.width = (w_ecar - 6) + 'px';
            }
        }

        // process cycle no tack
        let w_cycle  = Math.floor((this._processFullwidth / this.TotalCycle) * 100) / 100;
        let _cycle_process_no = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_content .process_run .process_no');
        for(let i=0; _cycle_process_no[i]; i++){
            _cycle_process_no[i].style.width = (w_cycle + 0.9) + 'px';
        }

        //ecar cycle no tack
        let _ecar_process_no = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process_task .ecar_process_no');
        for(let i=0; _ecar_process_no[i]; i++){
            _ecar_process_no[i].style.width = (w_cycle + 0.9) + 'px';
        }


        ///////////////////////////design height
        let h = window.innerHeight - 4;
        let h_header = Math.floor((h / 8) * 100) / 100;
        let h_process = h_header;

        let h_ecar_content = h - (h_header + h_process);
        let c_car = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_for .ecar_item');
        let c_car_count = (c_car.length);

        let h_ecar1 = Math.floor((h_ecar_content / c_car_count) * 100) / 100;
        let h_ecar2 = Math.floor((h_ecar1 / 2) * 100) / 100;

       //Header
       let _hearder = document.querySelector<HTMLElement>('.PTA .HEADER');
       if(_hearder) _hearder.style.height = (h_header - 0) + 'px';
       let _hearder_val = document.querySelector<HTMLElement>('.PTA .HEADER .val');
       if(_hearder_val) _hearder_val.style.height = (h_header - 3) + 'px';
        //header txt1
       if(_header_txt1) _header_txt1.style.height = (h_header) + 'px';

        //process
        let _process = document.querySelectorAll<HTMLElement>('.PTA .PROGRESS_BAR, .PTA .PROGRESS_BAR .process_panel');
        for(let i=0; _process[i]; i++){
            _process[i].style.height = (h_process - 0) + 'px';
        }

        let _process_sub = document.querySelectorAll<HTMLElement>('.PTA .PROGRESS_BAR .process_panel_box,  '+
                                                                                                        '.PTA .PROGRESS_BAR .process_panel_box .process_name,  '+
                                                                                                        '.PTA .PROGRESS_BAR .process_panel_box .process_number,  '+
                                                                                                        '.PTA .PROGRESS_BAR .process_panel .process_warp, '+
                                                                                                        '.PTA .PROGRESS_BAR .process_panel .process_run');
        for(let i=0; _process_sub[i]; i++){
            _process_sub[i].style.height = (h_process - 3) + 'px';
        }

        //cycler _process_no(no_in_date) title
        let _process_no = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_content .process_run .process_no');
        for(let i=0; _process_no[i]; i++){
            _process_no[i].style.height = (h_process - 3) + 'px';
        }




        //Eff
        let _h_Eff = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .EFF, .PTA .ECAR_CONTENT .EFF .val');
        for(let i=0; _h_Eff[i]; i++){
            _h_Eff[i].style.height = (h_ecar_content - 6) + 'px';
        }



        //Ecar
        let _car_item = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item');
        for(let i=0; _car_item[i]; i++){
            _car_item[i].style.height = (h_ecar1) + 'px';
        }

        let _car_panel = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ecar_panel');
        for(let i=0; _car_panel[i]; i++){
            _car_panel[i].style.height = (h_ecar1 - 4) + 'px';
        }


        let _car_del = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ECAR_NAME, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ECAR_NUMBER, ' +
                                                                                                '.PTA .ECAR_CONTENT .ecar_item .ecar_panel_box');
        for(let i=0; _car_del[i]; i++){
            _car_del[i].style.height = (h_ecar1 - 6) + 'px';
        }



        //Ecar pk or del
        let _car_pk_del = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ecar_process, ' +
                                                                                                      '.PTA .ECAR_CONTENT .ecar_item .ECAR_NUMBER .ecar_process_name, ' +
                                                                                                      '.PTA .ECAR_CONTENT .ecar_item .ecar_panel .ecar_process .ecar_process_bg');
        for(let i=0; _car_pk_del[i]; i++){
            _car_pk_del[i].style.height = (h_ecar2 - 4) + 'px';
        }

        //ecar cycle tack
        // let _ecar_process_no = document.querySelectorAll<HTMLElement>('.PTA .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process_task .ecar_process_no');
        for(let i=0; _ecar_process_no[i]; i++){
            _ecar_process_no[i].style.height = (h_ecar2 - 4) + 'px';
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
        this._serviceProxy.getMonitorScreenData(this.prodLine)
        .subscribe((result) => {
            if (result) {
                this.bindData(result);
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

    bindData(_data: LgaBp2ProgressMonitorScreenDto[]){

        //clear
        let _cycle_no = document.querySelectorAll<HTMLElement>('.PTA  .PROGRESS_BAR .process_content .process_run .process_no, .PTA .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process_task .ecar_process_no');
        for(let i=0; _cycle_no[i]; i++){ _cycle_no[i].classList.remove('FINISHED'); }

        if(_data.length > 0) {
            //tacktime
            let _tt = "T.T : "+(_data[0].taktTime * 1 / 60).toFixed(1) + "'";
            let _txtHeader = document.querySelector<HTMLElement>('.HEADER .val .txt1');
            if(_txtHeader) _txtHeader.textContent = _tt;


            //eff
            let _eff = document.querySelector<HTMLElement>('.ECAR_CONTENT .EFF .val');
            if(_eff) _eff.textContent = _data[0].efficiency.toString();
        }

        //bind data
        for(let i =0; _data[i]; i++){

            if(_data[i].status == "FINISHED") {

                //ecar
                let _cssTask = '.PTA .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process.' + _data[i].ecarName.replace(' ','') + '.' + _data[i].processName + ' ' +
                                       '.ecar_process_task .ecar_process_no.NO_' + _data[i].numberNo;
                let _noTask = document.querySelector<HTMLElement>(_cssTask);
                if(_noTask) {
                    _noTask.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
                    _noTask.classList.add("FINISHED");
                    _noTask.textContent = "";
                }

                //process
                let _cssProcess = '.PTA .PROGRESS_BAR .process_panel .process_run .process_no.NO_' + _data[i].numberNo;
                let _noProcess = document.querySelector<HTMLElement>(_cssProcess);
                if(_noProcess) {
                    _noProcess.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
                    _noProcess.classList.add("FINISHED");
                }

            }

            if(_data[i].isDelay == "Y") {

                let _delayFinish = (_data[i].delaySecond * 1 / 60).toFixed(1) + "'";
                //ecar
                let _cssTask = '.PTA .ECAR_CONTENT .ecar_item .ECAR_DELIVERY .ecar_process.' + _data[i].ecarName.replace(' ','') + '.' + _data[i].processName + ' ' +
                                       '.ecar_process_task .ecar_process_no.NO_' + _data[i].numberNo;
                let _noTask = document.querySelector<HTMLElement>(_cssTask);
                if(_noTask) {
                    _noTask.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
                    _noTask.classList.add("DELAYED");
                    _noTask.textContent = _delayFinish;
                }

                //process   -phuongdv
                let _cssProcess = '.PTA .PROGRESS_BAR .process_panel .process_run .process_no.NO_' + _data[i].numberNo;
                let _noProcess = document.querySelector<HTMLElement>(_cssProcess);
                if(_noProcess) {
                    _noProcess.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
                    _noProcess.classList.add("FINISHED");
                }


            }

        };
    }


    ecarProcess(ecarId:number){
        return this.configData.filter(a => a.ecarId === ecarId);
    }

}
