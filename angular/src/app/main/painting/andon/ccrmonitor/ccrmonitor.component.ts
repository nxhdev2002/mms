import { catchError, finalize } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { DateTime } from 'luxon';
import { Component, Injector, HostListener, ViewChild, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';


//add Service
import { PtsAdoCCRMonitorServiceProxy, GetWeldingDataOutput, GetPaintingDataOutput, GetAssemblyDataOutput, GetInspectionDataOutput, GetAllBufferDataOutput, GetFrameOutput, GetVehicleDetailsOutput } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';


//app


//form edit

@Component({
    templateUrl: './ccrmonitor.component.html',
    styleUrls: [
        './ccrmonitor.component.less',
    ],
})

export class CCRMonitorComponent extends AppComponentBase {

    isLoading: boolean = false;
    rowData: any[] = [];
    VehicleDetail:GetVehicleDetailsOutput = new GetVehicleDetailsOutput();
    pipe = new DatePipe('en-US');
    dataIventoryStd: any[] = [];

    @ViewChild('', { static: true }) modal: ModalDirective | undefined;
    // @Output('ngAfterViewInit') AfterViewInitEvent: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    constructor(
        injector: Injector,
        private _serviceProxy: PtsAdoCCRMonitorServiceProxy,
    ) {
        super(injector);
        //alert('1: constructor');
    }

    ngOnInit(): void {
        //alert('2: ngOnInit');
        
    }

    ngAfterViewInit() {
        //alert('3: ngAfterViewInit')
        setTimeout(() => {
            this.layoutCcrMonitor();
            this.repeatBindData();
        }, 1000);
    }

    @HostListener('load', ['$event'])
    onLoad() {
        this.layoutCcrMonitor();
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        //alert('4: onWindowResize')
        this.layoutCcrMonitor();
            // this.notify.success('Height: ' + hButton);
    }
    ////end set width height

    ngOnDestroy(): void{
        clearTimeout(this.time_CCR_MONITOR_WELDING);
        clearTimeout(this.time_CCR_MONITOR_PAINTING);
        clearTimeout(this.time_CCR_MONITOR_ASSEMBLY);
        clearTimeout(this.time_CCR_MONITOR_INSPECTION);
        clearTimeout(this.time_CCR_MONITOR_FRAME);
        clearTimeout(this.time_CCR_MONITOR_ALL_BUFFER);
        clearTimeout(this.timers_flash_search);
    }
    timecount:number = 0;
    time_CCR_MONITOR_WELDING;
    time_CCR_MONITOR_PAINTING;
    time_CCR_MONITOR_ASSEMBLY;
    time_CCR_MONITOR_INSPECTION;
    time_CCR_MONITOR_FRAME;
    time_CCR_MONITOR_ALL_BUFFER;
    timerGet:number = 2500;
    repeatBindData(){

        this.timecount = this.timecount + 1;

        try{
            this.CCR_MONITOR_WELDING();
            this.CCR_MONITOR_PAINTING();
            this.CCR_MONITOR_ASSEMBLY();
            this.CCR_MONITOR_INSPECTION();
            this.CCR_MONITOR_FRAME();
            this.CCR_MONITOR_ALL_BUFFER();
            this.getIventoryStdData();
            // console.log("this.getIventoryStdData();")
        }catch(ex){
            console.log(ex);
            // this.timerstorage = setTimeout(()=>{
            //     this.repeatBindData();
            // }, this.timerGet);
        }

        console.log("repeat this.repeatBindData():"+ this.timecount);

        // this.timerstorage = setTimeout(()=>{
        //     this.repeatBindData();
        // }, this.timerGet);
    }

    CCR_MONITOR_WELDING() {
        try{
            this. _serviceProxy.getWeldingData()
            .subscribe((result) => {
                if(result){
                    this.WELDING_BindData(result);
                }

                this.time_CCR_MONITOR_WELDING = setTimeout(()=>{
                    this.CCR_MONITOR_WELDING();
                }, this.timerGet);
            });
        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_WELDING = setTimeout(()=>{
                this.CCR_MONITOR_WELDING();
            }, this.timerGet);
        }

    }
    CCR_MONITOR_PAINTING() {
        try{
            this. _serviceProxy.getPaintingData()
            .subscribe(
                (result) => {
                    if(result){
                        this.PAINTING_BindData(result);
                    }
                    this.time_CCR_MONITOR_PAINTING = setTimeout(()=>{
                        this.CCR_MONITOR_PAINTING();
                    }, this.timerGet);
                }
                ,(error) => {
                    console.log(error);
                    this.time_CCR_MONITOR_PAINTING = setTimeout(()=>{
                        this.CCR_MONITOR_PAINTING();
                    }, this.timerGet);
                }
            );

        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_PAINTING = setTimeout(()=>{
                this.CCR_MONITOR_PAINTING();
            }, this.timerGet);
        }



    }
    CCR_MONITOR_ASSEMBLY(){
        try{
            this. _serviceProxy.getAssemblyData()
            .subscribe((result) => {
                if(result){
                    this.ASSEMBLY_BindData(result);
                }

                this.time_CCR_MONITOR_ASSEMBLY = setTimeout(()=>{
                    this.CCR_MONITOR_ASSEMBLY();
                }, this.timerGet);
            });
        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_ASSEMBLY = setTimeout(()=>{
                this.CCR_MONITOR_ASSEMBLY();
            }, this.timerGet);
        }

    }
    CCR_MONITOR_INSPECTION(){
        try{
            this. _serviceProxy.getInspectionData()
            .subscribe((result) => {
                if(result){
                    this.INSPECTION_BindData(result);
                }
                this.time_CCR_MONITOR_INSPECTION = setTimeout(()=>{
                    this.CCR_MONITOR_INSPECTION();
                }, this.timerGet);
            });
        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_INSPECTION = setTimeout(()=>{
                this.CCR_MONITOR_INSPECTION();
            }, this.timerGet);
        }

    }
    CCR_MONITOR_FRAME(){
        try{
            this. _serviceProxy.getFrameData()
            .subscribe((result) => {
                if(result){
                    this.FRAME_BindData(result);
                }
                this.time_CCR_MONITOR_FRAME = setTimeout(()=>{
                    this.CCR_MONITOR_FRAME();
                }, this.timerGet);
            });
        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_FRAME = setTimeout(()=>{
                this.CCR_MONITOR_FRAME();
            }, this.timerGet);
        }

    }
    CCR_MONITOR_ALL_BUFFER(){
        try{
            this._serviceProxy.getAllBufferData()
            .subscribe((result) => {
                if(result){
                    this.ALL_BUFFER_BindData(result);
                }
                this.time_CCR_MONITOR_ALL_BUFFER = setTimeout(()=>{
                    this.CCR_MONITOR_ALL_BUFFER();
                }, this.timerGet);
            });
        }catch(ex){ console.log(ex);
            this.time_CCR_MONITOR_ALL_BUFFER = setTimeout(()=>{
                this.CCR_MONITOR_ALL_BUFFER();
            }, this.timerGet);
         }

    }
    CCR_MONITOR_VEHICLE_DETAILS(body_no:string, lot_no:string, sequence_no:string, vin_no:string){
        try{
            this._serviceProxy.getVehicleDetails(body_no,lot_no,sequence_no,vin_no)
            .subscribe((result) => {

                if(result){
                    this. VehicleDetail = result;
                    this.VEHICLE_DETAILS_bindData(result);
                    this.VEHICLE_DETAILS_SHOW();
                }
            });

        }catch(ex){ console.log(ex); }
    }

    WELDING_BindData(_data: GetWeldingDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .WELDING .content_left'); //
        this.ClearField(_d.querySelectorAll('.item .area:not(.none) .element'));
        _data.forEach(d => {

            switch(d.processCd){

                case 'VECTORON':
                    let VECTORON = _d.querySelector('.item .area:not(.none) .element.GROUP_VECTORON_1');
                    if(VECTORON){
                        let val = VECTORON.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
                case 'SP_CHECK':
                    let SP_CHECK = _d.querySelector('.item .area:not(.none) .element.GROUP_SP_CHECK_1');
                    if(SP_CHECK){
                        let val = SP_CHECK.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
                case 'OBS_TRANS':
                    let OBS_TRANS1 = _d.querySelector('.item .area:not(.none) .element.GROUP_OBS_TRANS_1');
                    if(OBS_TRANS1){
                        let val = OBS_TRANS1.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
                case 'NEXT_ED':
                    let OBS_TRANS2 = _d.querySelector('.item .area:not(.none) .element.GROUP_OBS_TRANS_2');
                    if(OBS_TRANS2){
                        let val = OBS_TRANS2.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
                case 'REPAIR':
                    let REPAIR = _d.querySelector('.item .area:not(.none) .element.GROUP_REPAIR_' + d.seq);
                    if(REPAIR){
                        let val = REPAIR.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
                case 'OBS':
                    switch(d.w_Line){
                        case 'OBS_W1':
                            // alert(d.w_Line + ' ' + d.seq)
                            let OBS_W1 = _d.querySelector('.item .area:not(.none) .element.GROUP_OBS_W1_' + d.seq);
                            if(OBS_W1){
                                let val = OBS_W1.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.model);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                            }
                            break;
                        case 'OBS_W2':
                            let OBS_W2 = _d.querySelector('.item .area:not(.none) .element.GROUP_OBS_W2_' + d.seq);
                            if(OBS_W2){
                                let val = OBS_W2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.model);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                            }
                            break;
                        case 'OBS_W1_A':
                            let OBS_W3 = _d.querySelector('.item .area:not(.none) .element.GROUP_OBS_W1_A_' + d.seq);
                            if(OBS_W3){
                                let val = OBS_W3.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.model);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                            }
                            break;
                    }
                    break;
                default:
                    let el = _d.querySelector('.item .area:not(.none) .element.GROUP_' + d.w_Line + '_' + d.seq);
                    if(el){
                        let val = el.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break;
            }
        });
    }
    PAINTING_BindData(_data: GetPaintingDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .PAINTING'); //
        this.ClearField(_d.querySelectorAll('.item .area:not(.none) .element'));
        _data.forEach(d => {
            switch(d.processGroup){
                case 5:
                    switch (d.subGroup) {
                        case 4:
                            let el4 = _d.querySelector('.item .area:not(.none) .element.GROUP5_A1_OBS_' + d.filler);
                            if(el4){
                                let val = el4.querySelector('.value');
                                val.textContent = d.bodyNo;
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.classList.add(d.modeL);
                                this.warning_delay(el4, d.delayFlag, d.possibleMissScan);
                                this.warning_bumperflag(el4,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break
                        case 3:
                            let el3 = _d.querySelector('.item .area:not(.none) .element.GROUP5_BUFFER_OBS_' + d.filler);
                            if(el3){
                                let val = el3.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el3, d.delayFlag, d.possibleMissScan);
                                this.warning_bumperflag(el3,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break
                        case 1:
                            let el1 = _d.querySelector('.item .area:not(.none) .element.GROUP5_A2_OBS_' + d.filler);
                            if(el1){
                                let val = el1.querySelector('.value');
                                val.textContent = d.bodyNo;
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.classList.add(d.modeL);
                                this.warning_delay(el1, d.delayFlag, d.possibleMissScan);
                                this.warning_bumperflag(el1,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 10:
                            let el20A1 = _d.querySelector('.item .area:not(.none) .element.GROUP5_A1_20_' + d.filler);
                            if(el20A1){
                                let val = el20A1.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el20A1, d.delayFlag, d.possibleMissScan);
                                this.warning_bumperflag(el20A1,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 20:
                            let el20A2 = _d.querySelector('.item .area:not(.none) .element.GROUP5_A2_20_' + d.filler);
                            if(el20A2){
                                let val = el20A2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el20A2, d.delayFlag, d.possibleMissScan);
                                this.warning_bumperflag(el20A2,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                    }
                    break;
                case 4: //REAPIR
                    switch (d.subGroup) {
                        case 1: //REPAIR
                            if (d.location != null && d.location != 'null') { //Repair
                                let el = _d.querySelector('.item .area:not(.none) .element.GROUP_1_' + d.location.trim());
                                if(el){
                                    let val = el.querySelector('.value');
                                    let lotno = '';
                                    if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                    val.textContent = d.bodyNo;
                                    val.classList.add(d.modeL);
                                    this.warning_delay(el, d.delayFlag, d.possibleMissScan);
                                    this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                                }
                            }
                            break;
                        case 2: //RECOAT
                            let el2 = _d.querySelector('.item .area:not(.none) .element.GROUP_3_R_Recoat_' + d.filler);
                            if(el2){
                                let val = el2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el2, d.delayFlag, d.possibleMissScan);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 3: //OTHER
                            let el3 = _d.querySelector('.item .area:not(.none) .element.GROUP_3_R_Other');
                            if(el3){
                                let val = el3.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el3, d.delayFlag, d.possibleMissScan);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 4: //P.Repair
                            let el4 = _d.querySelector('.item .area:not(.none) .element.GROUP_3_R_P_Repair');
                            if(el4){
                                let val = el4.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el4, d.delayFlag, d.possibleMissScan);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                    }
                    break;
                case 3: //INLINE
                    switch(d.subGroup){
                        case 1:
                            let el = _d.querySelector('.item .area:not(.none) .element.GROUP3_SEQ1_' + d.processSeq);
                            if(el){
                                let val = el.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.warning_bumperflag(el,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 2:
                            let el2 = _d.querySelector('.item .area:not(.none) .element.GROUP3_2_' + d.filler);
                            if(el2){
                                let val = el2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el2, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el2, d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.warning_bumperflag(el2, d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 3:
                            let el3 = _d.querySelector('.item .area:not(.none) .element.GROUP3_3_' + d.filler);
                            if(el3){
                                let val = el3.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el3, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el3, d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.warning_bumperflag(el3, d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 4:
                            let el4 = _d.querySelector('.item .area:not(.none) .element.GROUP3_4_' + d.filler);
                            if(el4){
                                let val = el4.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el4, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el4, d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.warning_bumperflag(el4, d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 5:
                            let el5 = _d.querySelector('.item .area:not(.none) .element.GROUP3_5_' + d.filler);
                            if(el5){
                                let val = el5.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el5, d.delayFlag, d.possibleMissScan);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                                // this.warning_mode(el5, d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                // this.warning_bumperflag(el5, d.bumperFlag);
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (d.subGroup) {
                        case 1:
                            let el = _d.querySelector('.item .area:not(.none) .element.GROUP2_SEQ1_' + d.processSeq);
                            if(el){
                                let val = el.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 2:
                            let el2 = _d.querySelector('.item .area:not(.none) .element.JOIN_GROUP2_234_' + d.filler);
                            if(el2){
                                let val = el2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el2, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el2,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 3:
                            let el3 = _d.querySelector('.item .area:not(.none) .element.JOIN_GROUP2_234_' + d.filler);
                            if(el3){
                                let val = el3.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el3, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el3,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 4:
                            let el4 = _d.querySelector('.item .area:not(.none) .element.JOIN_GROUP2_234_' + d.filler);
                            if(el4){
                                let val = el4.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el4, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el4,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 5:
                            let el5 = _d.querySelector('.item .area:not(.none) .element.GROUP2_5_' + d.filler);
                            if(el5){
                                let val = el5.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el5, d.delayFlag, d.possibleMissScan);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 6:
                            let el6 = _d.querySelector('.item .area:not(.none) .element.GROUP2_6_' + d.filler);
                            if(el6){
                                let val = el6.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el6, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el6,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.warning_bumperflag(el6,d.bumperFlag);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                    }
                    break;
                case 1:
                    switch (d.subGroup) {
                        case 1:
                            let el = _d.querySelector('.item .area:not(.none) .element.GROUP1_SEQ_' + d.processSeq);
                            if(el){
                                let val = el.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                        case 2:
                            let el2 = _d.querySelector('.item .area:not(.none) .element.GROUP1_2_' + d.filler);
                            if(el2){
                                let val = el2.querySelector('.value');
                                let lotno = '';
                                if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                                val.textContent = d.bodyNo;
                                val.classList.add(d.modeL);
                                this.warning_delay(el2, d.delayFlag, d.possibleMissScan);
                                this.warning_mode(el2,d.isRescan, d.bodyNo, d.subGroup, d.tcStatus);
                                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: d.color });
                            }
                            break;
                    }
                    break;
            }
        });
    }
    ASSEMBLY_BindData(_data: GetAssemblyDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .ASSEMBLY'); //
        this.ClearField(_d.querySelectorAll('.item .area.A1_TRIM .element, ' +
                                                '.item .area.A1_CLASSIS .element, '+
                                                '.item .area.A1_FINAL .element, '+
                                                '.item .area.A2_TRIM .element, '+
                                                '.item .area.A2_CLASSIS .element, '+
                                                '.item .area.A2_FINAL .element'));
        _data.forEach(d => {
            let el = _d.querySelector('.item .area:not(.none) .element.ASSY_' + d.prodLine + '_' + d.processCd);
            if(el){
                let val = el.querySelector('.value');
                val.textContent = d.bodyNo;
                val.classList.add(d.model);
                this.warning_delay(el, d.delayFlag, 'Y');
                this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: d.sequenceNo, vinNo: '', grade: d.grade, model: d.model, color: d.color });
            }
        });
    }
    INSPECTION_BindData(_data: GetInspectionDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .ASSEMBLY'); //
        this.ClearField(_d.querySelectorAll('.item .area.INSPECTION .element, ' +
                                            '.item .area.STANDBY .element, ' +
                                            '.item .area.WAITING .element'));
        let standby_seq = 1, waitting_seq = 1;
        _data.forEach(d => {
            switch(d.prodLine){
                case 'INSPECTION':
                    let el = _d.querySelector('.item .area:not(.none) .element.GROUP_' + d.prodLine + '_' + d.processCd);
                    if(el){
                        let val = el.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.warning_delay(el, d.delayFlag, 'Y');
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: d.sequenceNo, vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    break
                case 'STANDBY':
                    let el_stand = _d.querySelector('.item .area:not(.none) .element.GROUP_' + d.prodLine + '_' + standby_seq);
                    if(el_stand){
                        let val = el_stand.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.warning_delay(el_stand, d.delayFlag, 'Y');
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: d.sequenceNo, vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    standby_seq= standby_seq + 1;
                    break;
                case 'WAITING':
                    let el_wait = _d.querySelector('.item .area:not(.none) .element.GROUP_' + d.prodLine + '_' + waitting_seq);
                    if(el_wait){
                        let val = el_wait.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.model);
                        this.warning_delay(el_wait, d.delayFlag, 'Y');
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: d.sequenceNo, vinNo: '', grade: lotno, model: d.model, color: d.color });
                    }
                    waitting_seq= waitting_seq + 1;
                    break;
            }
        });
    }
    FRAME_BindData(_data: GetFrameOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .ASSEMBLY .item.FRAME'); //
        this.ClearField(_d.querySelectorAll('.area .element'));
        let _OVEN_OBS = '';
        let _OVEN_OBS_Units = 0;
        _data.forEach(d => {
             switch(d.processGroup){
                case 111:
                    let el = _d.querySelector('.area:not(.none) .element.GROUP111_1_' + d.filler);
                    if(el){
                        let val = el.querySelector('.value');
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        val.textContent = d.bodyNo;
                        val.classList.add(d.modeL);
                        this.warning_delay(el, 'd.delayFlag', 'Y');
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: '' });
                    }
                    break;
                case 100:
                        let el1 = _d.querySelector('.area:not(.none) .element.GROUP100_1_' + d.filler);
                        if(el1){
                            let val = el1.querySelector('.value');
                            let lotno = '';
                            if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                            val.textContent = d.bodyNo;
                            val.classList.add(d.modeL);
                            this.warning_delay(el1, 'd.delayFlag', 'Y');
                            this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: '' });
                        }
                        break;
                case 101:
                    if(d.seq == 1){
                        let el2 = _d.querySelector('.area:not(.none) .element.GROUP101_1_1');
                        if(el2){
                            let val = el2.querySelector('.value');
                            let lotno = '';
                            if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                            val.textContent = d.bodyNo;
                            val.classList.add(d.modeL);
                            this.warning_delay(el2, 'd.delayFlag', 'Y');
                            this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: '' });
                        }
                    }

                    let el2 = _d.querySelector('.area:not(.none) .element.GROUP101_1_OVEN_OBS');
                    if(el2){
                        _OVEN_OBS = (_OVEN_OBS.length > 0) ? _OVEN_OBS + ', ' + d.bodyNo: d.bodyNo;
                        _OVEN_OBS_Units = _OVEN_OBS_Units + 1;
                        let lotno = '';
                        if(d.lotNo){ lotno = d.lotNo.substring(0,2)}
                        let val = el2.querySelector('.value');
                        val.textContent = _OVEN_OBS_Units + ' Units';
                        val.classList.add(d.modeL);
                        this.warning_delay(el2, 'd.delayFlag', 'Y');
                        this.setAttributes(val,{ bodyNo: d.bodyNo, lotNo: d.lotNo, sequenceNo: '', vinNo: '', grade: lotno, model: d.modeL, color: '', value: _OVEN_OBS });
                    }
                    break;

             }
        });
    }
    ALL_BUFFER_BindData(_data: GetAllBufferDataOutput[]){

        let _d = document.querySelector('.PTA .PTA_Content .box_buffer .bufftable'); //
        this.ClearAllBuffer(_d.querySelectorAll('tr:nth-child(n+2) td:nth-child(n+2)'));
        _data.forEach(d => {
            let el = _d.querySelector<HTMLElement>('tr .ACT_' + d.loc);
            if(el){
                el.textContent = d.buffAct.toString();
                el.style.backgroundColor = d.colorCd;
            }
            el = _d.querySelector('tr .MIN_' + d.loc);
            if(el){
                el.textContent = d.minVal.toString();
            }
            el = _d.querySelector('tr .MAX_' + d.loc);
            if(el){
                el.textContent = d.maxVal.toString();
            }
        });
    }


    VEHICLE_DETAILS(_key:string){
        let _val = document.querySelector<HTMLElement>('.PTA .PTA_Content .area:not(.none) .element.' + _key + ' .value');
        if(_val){
            let _bodyno = _val.getAttribute('bodyno');
            let _lotno = _val.getAttribute('lotno');
            let _sequenceno = _val.getAttribute('sequenceno');
            let _vinno = _val.getAttribute('vinno');
            if(_bodyno){
                this.CCR_MONITOR_VEHICLE_DETAILS(_bodyno,_lotno,_sequenceno,_vinno);
            }
        }
    }

    VEHICLE_DETAILS_bindData(d:GetVehicleDetailsOutput){
        let el = document.querySelector<HTMLElement>('.PTA .DETAIL_OVER .popDetail');
        el.querySelector('.body_no').textContent = d.bodyNo;
        el.querySelector('.Model.val').textContent = d.model;
        el.querySelector('.Seq.val').textContent = d.sequenceNo;
        el.querySelector('.Driver_Air_Bag.val').textContent = d.driverAirBag;
        el.querySelector('.Lot_No.val').textContent = d.lotNo;
        el.querySelector('.No_in_lot.val').textContent = d.noInLot;
        el.querySelector('.Passenger_Air_Bag.val').textContent = d.passengerAirBag;
        el.querySelector('.Color.val').textContent = d.color;
        el.querySelector('.Key.val').textContent = d.keyNo;
        el.querySelector('.Side_Air_Bag_LH.val').textContent = d.sideAirBagLh;
        el.querySelector('.VIN.val').textContent = d.vin;
        el.querySelector('.Side_Air_Bag_RH.val').textContent = d.sideAirBagRh;
        el.querySelector('.EG_No.val').textContent = d.eng;
        el.querySelector('.Knee_Air_Bag.val').textContent = d.kneeAirBagLh;
        el.querySelector('.TM_No.val').textContent = d.trs;
        el.querySelector('.Curtain_Air_Bag_LH.val').textContent = d.curtainSideAirBagLh;
        el.querySelector('.ECU.val').textContent = d.ecu;
        el.querySelector('.Curtain_Air_Bag_RH.val').textContent = d.curtainSideAirBagRh;
        el.querySelector('.Total_Delay.val').textContent = d.totalDelay;

        el.querySelector('.WIn.val').textContent =  this.toDDMMYYY(d.wInDateActual);
        el.querySelector('.WIn.time').textContent = this.toHHSS(d.wInDateActual);
        el.querySelector('.TIn.val').textContent = this.toDDMMYYY(d.tInPlanDatetime);
        el.querySelector('.TIn.time').textContent = this.toHHSS(d.tInPlanDatetime);
        el.querySelector('.Shipping_Time.val').textContent = this.toDDMMYYY(d.shippingTime);
        el.querySelector('.Shipping_Time.time').textContent = this.toHHSS(d.shippingTime);
        el.querySelector('.AIn.val').textContent = this.toDDMMYYY(d.aInDateActual);
        el.querySelector('.AIn.time').textContent = this.toHHSS(d.aInDateActual);
        el.querySelector('.InpsIn.val').textContent = this.toDDMMYYY(d.insOutDateActual);
        el.querySelector('.InpsIn.time').textContent = this.toHHSS(d.insOutDateActual);
        el.querySelector('.VP4_Out.val').textContent = this.toDDMMYYY(d.insLineOutVp4DateActual);
        el.querySelector('.VP4_Out.time').textContent = this.toHHSS(d.insLineOutVp4DateActual);

    }
    close(): void {
        console.log(1);

        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === "Escape") {
                this.VEHICLE_DETAILS_CLOSE();
            }
        }

    VEHICLE_DETAILS_SHOW(){
        document.querySelector<HTMLElement>('.PTA .DETAIL_OVER').style.display = 'flex';
    }
    VEHICLE_DETAILS_CLOSE(){
        let el =document.querySelectorAll<HTMLElement>('.PTA .myoverlay');
        for(let i=0; el[i]; i++){
            el[i].style.display ='none';
        }
    }


    OVEN_OBS(_key:string){
        let _val = document.querySelector<HTMLElement>('.PTA .PTA_Content .ASSEMBLY .item.FRAME .area.OVEN_OBS .element.GROUP101_1_OVEN_OBS .value');
        if(_val){
            let elOVEN_OBS = document.querySelector<HTMLElement>('.PTA .OVEN_OBS .detail');
            elOVEN_OBS.textContent = _val.getAttribute('value');

            document.querySelector<HTMLElement>('.PTA .myoverlay.OVEN_OBS').style.display = 'flex';
        }
    }

    SHOW_OVF(t:any) {

        let _val = document.querySelector<HTMLElement>('.PTA .PTA_Content .ASSEMBLY .item .area.OVF .element.GROUP_OVF .value.OVF_VALUE');
        if(_val){
            let elOVF = document.querySelector<HTMLElement>('.PTA .myoverlay.OVF .popDetail_OVF');
            elOVF.querySelector<HTMLElement>('.OVF_BODY_NO1').textContent = '';
            elOVF.querySelector<HTMLElement>('.OVF_LOT_NO1').textContent = '';
            elOVF.querySelector<HTMLElement>('.OVF_DELAY1').textContent = '';

            elOVF.querySelector<HTMLElement>('.OVF_BODY_NO2').textContent = '';
            elOVF.querySelector<HTMLElement>('.OVF_LOT_NO2').textContent = '';
            elOVF.querySelector<HTMLElement>('.OVF_DELAY2').textContent = '';

            document.querySelector<HTMLElement>('.PTA .myoverlay.OVF').style.display = 'flex';
        }
    }


    numbers:Array<any> = [];
    fornumbers(num:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+1);
        return this.numbers;
    }

    fornumbers_next(num:number, start:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+start);
        return this.numbers;
    }

    numbers_desc:Array<any> = [];
    fornumbers_desc(num:number) {
        this.numbers_desc = Array.from({length:num},(v,k)=>num-k);
        return this.numbers_desc;
    }

    fornumbersRangeDesc(start:number, stop:number, step:number){
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    fornumbersRange(start:number, stop:number, step:number){
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }

    result:any = [];
    SearchAction(_type:string) {

        let _valSearch = document.querySelector<HTMLInputElement>('.PTA .PTA_Content .WELDING .content_right #SEARCH').value.trim();
        document.querySelector<HTMLElement>('.PTA .PTA_Content .WELDING .content_right .formsearch').style.display = 'none';

        if (_valSearch != "") {
            this.flash_search_end();
            this.result = document.querySelectorAll<HTMLElement>(".PTA .PTA_Content .area:not(.none) .element.SEARCH_DETAIL div[" + _type + "='" + _valSearch.toUpperCase() + "']");

            //clearTimeout(this.timers_flash_search);
            if(this.result){
                this.is_flash = false;
                this.flash_search(this.result);
            }
        }else{
            this.flash_search_end();
        }

    }

    showSearch(){
        document.querySelector<HTMLElement>('.PTA .PTA_Content .WELDING .content_right .formsearch').style.display = 'block';
    }


    layoutCcrMonitor(){
        ///////////////////////////design width
        let w = window.innerWidth -2;
        let wItem = Math.floor(((w-2)/ 38) * 100) / 100;
        let wWeldingLeft = Math.floor(((w / 38)*30) * 100) / 100;
        let wWeldingRight = w - wWeldingLeft;
        let wBufferItem = Math.floor(((wWeldingRight- 12) / 9) * 100) / 100;


        //bg_welding
        let bg_wel = document.querySelector<HTMLElement>('.PTA_Content .img_bg_wel');
        bg_wel.style.width =  wWeldingLeft + "px";

        let _weldingleft = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .content_left, ' +
                                                                  '.PTA .PTA_Content .content_left .spec.left');
        for(let i=0; _weldingleft[i]; i++){
            _weldingleft[i].style.width = wWeldingLeft - 1 + 'px';
        }

        let _weldingRight = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .WELDING .content_right, .PTA_Content .box.content_right .box_buffer');
        for(let i=0; _weldingRight[i]; i++){
            _weldingRight[i].style.width = wWeldingRight - 1 + 'px';
        }

        let _item = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area:not(.none) .element');
        for(let i=0; _item[i]; i++){
            _item[i].style.width = wItem + "px";
        }
        let _itemW2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area:not(.none) .element.w2');
        for(let i=0; _itemW2[i]; i++){
            _itemW2[i].style.width = wItem*2 + "px";
        }
        let _itemNone = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area.none .element');
        for(let i=0; _itemNone[i]; i++){
            _itemNone[i].style.width = wItem + "px";
        }

        let _itemHeader = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area .element.header');
        for(let i=0; _itemHeader[i]; i++){
            _itemHeader[i].style.width = wItem*3 + "px";
        }
        let _itemBot = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area .element .istatus');
        for(let i=0; _itemBot[i]; i++){
            _itemBot[i].style.width = wItem + 1 + "px";
        }

        //search
        let _itemBuffer = document.querySelectorAll<HTMLElement>('.PTA_Content .box_buffer .bufftable .buff');
        for(let i=0; _itemBuffer[i]; i++){
            _itemBuffer[i].style.width = wBufferItem + "px";
        }
        let _search1 = document.querySelector<HTMLElement>('.PTA_Content .box.content_right .box_buffer .box_remark_search_sub .input_search');
        _search1.style.width = (wItem*4)/3 + "px";

        //remark1
        let _remark = document.querySelector<HTMLElement>('.PTA_Content .area.REMARK1');
        _remark.style.width = wItem*2 + "px";
        //remark 2
        let _remark2 =  document.querySelector<HTMLElement>('.PTA .PTA_Content .box_remark_vuong');
        _remark2.style.width = wItem*2.5 + "px";
        //tracking_point
        let _tracking_point = document.querySelector<HTMLElement>('.PTA .PTA_Content .Tracking_point');
        _tracking_point.style.width = wItem*2 + "px";
        //inventory_std
        let _inventory_std = document.querySelector<HTMLElement>('.PTA .PTA_Content .inventory_std');
        _inventory_std.style.width = wItem*2.5 + "px";
        

        let img_barcode = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .item .area:not(.none) .barcode');
        let _wbar = wItem/2;
        let _wbarleft = wItem/4;
        for(let i=0; img_barcode[i]; i++){
            img_barcode[i].style.width = _wbar + "px";
            img_barcode[i].style.height = _wbar + "px";
            img_barcode[i].style.marginTop = '-'+ _wbar + "px";
            img_barcode[i].style.marginLeft = _wbarleft + "px";
        }


        //box icon number
        let wicon = Math.floor(((wItem*4)/5) * 100) / 100;
        let _boxicon =  document.querySelectorAll<HTMLElement>('.PTA_Content .box_icon');
        for(let i=0; _boxicon[i]; i++){
            _boxicon[i].style.width = wicon + "px";
            _boxicon[i].style.height = wicon + "px";
        }
        //waring icon
        let _icon = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area:not(.none) .element .mode_icon');
        let wIcon2 = Math.floor(((wItem)/ 5) * 100) / 100;
        for(let i=0; _icon[i]; i++){
            _icon[i].style.width = wIcon2 + "px";
            _icon[i].style.height = wIcon2 + "px";
        }



        ///////////////////////////design height
        let h = window.innerHeight - 2;
        let hbox = Math.floor(((h)/ 19.5) * 100) / 100;

        let hTitle = Math.floor(((hbox)/ 3) * 100) / 100;
        let hItem = hbox - hTitle;

        let hTitle2 = Math.floor(((hbox)/ 4) * 100) / 100;
        let hItem2 = hbox - (hTitle2*2);

        let hremark_coat = Math.floor(((hbox)/ 2) * 100) / 100;


        //item line
        let _itembox = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .item');
        for(let i=0; _itembox[i]; i++){
            _itembox[i].style.height = hbox - 1 + "px";
        }
        //title1
        let _title = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area:not(.v2):not(.none) > .title, ' +
                                                  '.PTA .PTA_Content .area:not(.v2):not(.none) > .element > .subtitle');
        for(let i=0; _title[i]; i++){
            _title[i].style.height = hTitle -1 + "px";
        }
        //item 1
        _item = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area:not(.v2):not(.none) .element .value, '+
                                                        '.PTA .PTA_Content .area:not(.v2):not(.none) .element .flash, '+
                                                        '.PTA .PTA_Content .area.none:not(.v2) .element');
        for(let i=0; _item[i]; i++){
            _item[i].style.height = (hItem) - 1 + "px";
        }
        //title2
        let _title2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area.v2:not(.none) > .title, ' +
                                                '.PTA .PTA_Content .area.v2:not(.none) > .element > .subtitle');
        for(let i=0; _title2[i]; i++){
            _title2[i].style.height = hTitle2 -1 + "px";
        }
        //item 2
        let _item2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area.v2:not(.none) .element .value, '+
                                                            '.PTA .PTA_Content .area.v2:not(.none) .element .flash, '+
                                                            '.PTA .PTA_Content .area.v2.none .element');
        for(let i=0; _item2[i]; i++){
            _item2[i].style.height = (hItem2) - 1 + "px";
        }
        //title 2 subtitle2
        let _titlebox2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area.v2:not(.none) > .element > .subtitle2');
        for(let i=0; _titlebox2[i]; i++){
            _titlebox2[i].style.height = (hTitle2*2) - 2 + "px";
        }
        //spec line
        let _Spec = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .spec, .PTA_Content .spec .element');
        for(let i=0; _Spec[i]; i++){
            _Spec[i].style.height = (hTitle) + "px";
        }
        //search
        let hboxright = (hbox*3) + (hTitle*4);
        let hsearch = hTitle2*2
        let htable = hboxright - hsearch;

        let _search = document.querySelector<HTMLElement>('.PTA .PTA_Content .WELDING .box_buffer');
        _search.style.height = hboxright - 1 + "px";

        let _searchtop = document.querySelectorAll<HTMLElement>('.PTA_Content .box.content_right .box_buffer .box_buffer_top, ' +
            '.PTA_Content .box.content_right .box_buffer .box_remark_search,' +
            '.PTA_Content .box.content_right .box_buffer .box_remark_search_sub,' +
            '.PTA_Content .box.content_right .box_buffer .box_remark_search_sub .input_search,' +
            '.PTA_Content .box.content_right .box_buffer .box_remark1.dai .box_remark_color, ' +
            '.PTA_Content .box.content_right .box_buffer .box_buffer_top .box_remark1 .box_remark_txt');
        for(let i=0; _searchtop[i]; i++){
            _searchtop[i].style.height =  (hTitle2*2) - 10 + "px";
        }

        let tabletr = document.querySelectorAll<HTMLElement>('.PTA_Content .box_buffer .bufftable tr');
        for(let i=0; tabletr[i]; i++){
            tabletr[i].style.height =  (htable/4) - 2 + "px";
        }

        //bg_welding
        //let bg_wel = document.querySelector('.PTA_Content .img_bg_wel');
        bg_wel.style.height =  hboxright + "px";

        //remark 1
        let _remark_coat = document.querySelectorAll<HTMLElement>('.PTA .PTA_Content .area.REMARK1 .coat_remark');
        for(let i=0; _remark_coat[i]; i++){
            _remark_coat[i].style.height = hremark_coat - 1 + "px";
        }
        //remark 2
        //_remark2 =  document.querySelectorAll('.PTA .PTA_Content .item .box_remark_vuong');
        _remark2.style.height = (hbox*3) + hTitle*2 - 1 + "px";

        //tracking point
        _tracking_point.style.height = (hbox*3) + hTitle*2 - 1 + "px";

        //_inventory_std
        _inventory_std.style.height = (hbox*3) + hTitle*2 - 1 + "px";

        //popup ovf
        let _ovf = document.querySelector<HTMLElement>('.PTA .myoverlay.OVF .popDetail_OVF');
        _ovf.style.height = Math.floor((h/ 4) * 100) / 100 + 'px';
    }

    timers_flash_search:any;
    is_flash:boolean;
    flash_search(result:NodeListOf<HTMLElement>){


        for(let i=0; result[i]; i++){
            result[i].parentNode.querySelector<HTMLElement>('.flash').style.display = (this.is_flash == false)? 'block':'none';
        }

        this.is_flash = (this.is_flash == false) ? true:false;
        this.timers_flash_search = setTimeout(()=>{
            this.flash_search(result);
        }, 800);

    }

    flash_search_end(){
        if(this.result){
            for(let i=0; this.result[i]; i++){
                (this.result[i] as HTMLElement).parentNode.querySelector<HTMLElement>('.flash').style.display = 'none';
            }
            this.result = null;
        }
        clearTimeout(this.timers_flash_search);
    }

    ClearField(obj:NodeListOf<HTMLElement>){
        if(obj) {
            for(let i=0; obj[i]; i++){

                let el = obj[i].querySelector('.value');
                if(el){
                    el.textContent = '';
                    el.classList.remove('K','V','I','F','C','A');
                    this.setAttributes(el,{ bodyNo: "", lotNo: "", sequenceNo: "", vinNo: "", grade: "", model: "", color: "" });
                }
    
                let flag = obj[i].querySelector('.flag_img');
                if(flag)
                    flag.classList.remove('SHOW');
    
                let icon = obj[i].querySelector('.mode_icon');
                if(icon)
                    icon.classList.remove('COLOR_R', 'COLOR_B');
    
                let bumper = obj[i].querySelector('.bumper_flag');
                if(bumper)
                    bumper.classList.remove('SHOW');
    
            }
        } 
    }

    ClearAllBuffer(obj:NodeListOf<HTMLElement>){
        for(let i=0; obj[i]; i++){
            obj[i].textContent = '';
            //obj[i].style.backgroundColor = 'transparent';
        }
    }

    warning_delay(el:Element, delayFlag:string, possibleMissScan:string){
        if (possibleMissScan == "Y" && (delayFlag == 'R' ||  delayFlag == 'Y' )) {
            el.querySelector('.flag_img').classList.add('SHOW');
        }
    }
    warning_mode(el:Element, isRescan:string, bodyNo:string, subGroup:number, tcStatus:number){

        let mode_icon = el.querySelector('.mode_icon');
        if(mode_icon){
            if (isRescan != "Y" && bodyNo  != "") {
                if (subGroup == 6){
                    if (tcStatus >= 3){
                        mode_icon.classList.add('COLOR_R');
                        return;
                    }
                    else{
                        mode_icon.classList.add('COLOR_B');
                        return;
                    }
                }
                else{
                    if (tcStatus == 1) return;
                    else if (tcStatus == 2){
                        mode_icon.classList.add('COLOR_B');
                        return;
                    }
                    else if (tcStatus >=3){
                        mode_icon.classList.add('COLOR_R');
                        return;
                    }
                }
            }
            mode_icon.classList.add('COLOR_B');
        }
    }
    warning_bumperflag(el:Element, bumperFlag:string){
        if (bumperFlag != null && bumperFlag != "null") {
            el.querySelector('.bumper_flag').classList.add('SHOW');
        } else {
            el.querySelector('.bumper_flag').classList.remove('SHOW');
        }
    }

    setAttributes(element:Element, attributes:any) {
        Object.keys(attributes).forEach(attr => {
          element.setAttribute(attr, attributes[attr]);
        });
    }

    getIventoryStdData(){
        this._serviceProxy.getInventoryStdData()
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                // console.log(result)
                this.dataIventoryStd = result ?? [];
            });
    }


    //datetime format
    toDDMMYYY(d:DateTime){
        const months = ["Jan", "Feb", "Mar","Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        if(d){
            let _d = d.day.toString() + '-' + months[d.month.toString()] + '-' + d.year.toString().slice(-2);
            return _d;
        }
        return '';
    }
    toHHSS(d:DateTime){
        if(d){
            let _d = d.hour.toString() + ':' + d.second.toString();
            return _d;
        }
        return '';
    }

}
