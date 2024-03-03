import { Component, Injector, HostListener, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';

//app
import { finalize, filter } from 'rxjs/operators';
import { PopupUnpack2Component } from './popup-unpack2.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

//add Service
import { MstLgwPickingTabletProcessServiceProxy,
             MstLgwPickingTabletProcessGetdataByLayoutOutput, } from '@shared/service-proxies/service-proxies';



@Component({
  selector: 'app-pickingtablet',
  templateUrl: './pickingtabletandon2.component.html',
  styleUrls: ['./pickingtabletandon2.component.less']
})


export class PickingTabletAndon2Component extends AppComponentBase {

    @ViewChild('popupUnpack2', { static: true }) popupUnpack2:| PopupUnpack2Component| undefined;
    rowData: MstLgwPickingTabletProcessGetdataByLayoutOutput[] = []
    rowDataColumn1: MstLgwPickingTabletProcessGetdataByLayoutOutput[] = []
    rowDataColumn2: MstLgwPickingTabletProcessGetdataByLayoutOutput[] = []
    rowCount:number;
    row1:number;

    isLoading: boolean = false;
    datePipe = new DatePipe('en-US');
    fn: CommonFunction = new CommonFunction();

    TackTime: number;
    PickingPosition: string = '';
    CAN_CALL_UNPACKING:string;
    IS_CALL_LEADER: string = '';
    SEQ_NO: string = '';
    TabletId: string;
    loadtotal:number;

        constructor(
            injector: Injector,
            private _serviceProxy: MstLgwPickingTabletProcessServiceProxy,
        ) {
            super(injector);
            console.log('1: constructor');

        }

        ngOnInit() {
            console.log('2: ngOnInit');
        }

        ngAfterViewInit() {
            console.log('3: ngAfterViewInit');
            this.initLayoutData();
        }

        @HostListener('load', ['$event'])
        onLoad() {
            console.log('4: ngAfterViewInit');
            this.drawScreen();
        }

        //set width height
        @HostListener('window:resize', ['$event'])
        onWindowResize() {
            //alert('4: onWindowResize')
            this.drawScreen();
                // this.notify.success('Height: ' + hButton);
        }
        ////end set width height

        drawScreen(){
            ///////////////////////////design width
            let w = window.innerWidth ;
            let wboxheader = Math.floor((w / 6) * 100) / 100;

            let _cont = document.querySelector<HTMLElement>('.PICKINGTABLET .contentf');
            if(_cont) _cont.style.width = w - 1 + 'px';

            let _boxheader = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .headerf');
            for(let i=0; _boxheader[i]; i++){ _boxheader[i].style.width = wboxheader - 1 + 'px'; }

            let wItem = Math.floor((w / 14) * 100) / 100;
            let wsubline = Math.floor(((wItem*3) / 2) * 100) / 100;
            let wStt = Math.floor(((wItem*3) / 4) * 100) / 100;
            let wVal = wItem*3 - wStt;

            let _box1 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .icol.box1');
            for(let i=0; _box1[i]; i++){ _box1[i].style.width = wItem*3 - 1 + 'px'; }
            let _box2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .icol.box2');
            for(let i=0; _box2[i]; i++){ _box2[i].style.width = wItem - 1 + 'px'; }
            let _box3 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .icol.box3');
            for(let i=0; _box3[i]; i++){ _box3[i].style.width = wItem*2 - 1 + 'px'; }

            let _subline = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contentf .cont .itemp .icol .line .subline ');
            for(let i=0; _subline[i]; i++){ _subline[i].style.width = wsubline - 1 + 'px'; }

            let _boxhead4 = document.querySelectorAll<HTMLElement>( '.PICKINGTABLET .contentf .cont .itemp .icol .line .subcol.tit ');
            for(let i=0; _boxhead4[i]; i++){ _boxhead4[i].style.width = wStt - 1 + 'px'; }
            let _boxhead4val = document.querySelectorAll<HTMLElement>( '.PICKINGTABLET .contentf .cont .itemp .icol .line .subcol.val ');
            for(let i=0; _boxhead4val[i]; i++){ _boxhead4val[i].style.width = wVal - 1 + 'px'; }

            //pop
            let _pop = document.querySelectorAll<HTMLElement>( '.PICKINGTABLET .contentf .pop');
            for(let i=0; _pop[i]; i++){ _pop[i].style.width = w - 1 + 'px'; }




            ///////////////////////////design height
            let h = window.innerHeight - 2;
            let hboxhead = Math.floor((h / 7) * 100) / 100;
            let hsubboxhead = Math.floor((hboxhead / 4) * 100) / 100;
            let hcont = h - hboxhead;


            let _boxhead = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .headerf');
            for(let i=0; _boxhead[i]; i++){ _boxhead[i].style.height = hboxhead - 1 + 'px'; }
            let _subboxheadtit = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .headerf .tit');
            for(let i=0; _subboxheadtit[i]; i++){
                _subboxheadtit[i].style.height = hsubboxhead - 2 + 'px';
                _subboxheadtit[i].style.lineHeight = hsubboxhead - 2 + 'px'; }
            let _subboxheadval = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .headerf .val');
            for(let i=0; _subboxheadval[i]; i++){
                _subboxheadval[i].style.height = hboxhead - hsubboxhead - 2 + 'px';
                _subboxheadval[i].style.lineHeight = hboxhead - hsubboxhead - 2 + 'px';}

            // _cont = document.querySelector<HTMLElement>('.PICKINGTABLET .contentf');
            if(_cont) _cont.style.height = hcont + 1 + 'px';

            let cleft = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contentf .contleft .leftcolbutton .itemp').length;
            let cright = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contentf .contright  .leftcolbutton .itemp').length;
            let itemcount = ((cleft > cright) ? cleft: cright) + 1;

            let hitem = Math.floor(((hcont + 1) / itemcount) * 100) / 100;
            let hitem2 = Math.floor(((hitem - 4)/ 2) * 100) / 100;
            let hitem3 =  Math.floor((hitem / 3) * 100) / 100;
            let hitem4 = Math.floor((hitem2 / 2) * 100) / 100;

            document.querySelector<HTMLElement>('.PICKINGTABLET .contentf').style.paddingTop = hitem2 + 'px';

            // let _item = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contentf .cont .itemp .icol');
            // for(let i=0; _item[i]; i++){ _item[i].style.height = hitem - 1 + 'px'; }
            let _itemcont = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .itemp .box1');
            for(let i=0; _itemcont[i]; i++){ _itemcont[i].style.height = hitem - 5 + 'px'; }
            let _itemcont2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .itemp .box2');
            for(let i=0; _itemcont2[i]; i++){ _itemcont2[i].style.height = 5 + 'px'; }

            let _item2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .itemp .icol .line, ' +
                                                                                                    '.PICKINGTABLET .cont .itemp .icol .line .subcol, ' +
                                                                                                    '.PICKINGTABLET .cont .itemp .icol .line .subline ');
            for(let i=0; _item2[i]; i++){
                _item2[i].style.height = hitem2 - 2 + 'px';
                _item2[i].style.lineHeight = hitem2 - 2 + 'px'; }

            let _muiten = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .icol .linengang hr');
            for(let i=0; _muiten[i]; i++){ _muiten[i].style.marginTop = hitem4 - 1 + 'px'; }
            let _muiten2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .icol .linengang img');
            for(let i=0; _muiten2[i]; i++){ _muiten2[i].style.marginTop = hitem4 - 11 + 'px'; }
            let _muiten3 = document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .muitenlen');
            _muiten3.style.height = (hitem2 - 10) + 'px';

            let hbox3val = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .box3 .val');
            for(let i=0; hbox3val[i]; i++){
                hbox3val[i].style.height = hitem3*2 - 1 + 'px';
                hbox3val[i].style.lineHeight = hitem3*2 - 1 + 'px'; }
            let hbox3tit = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .cont .box3 .tit');
            for(let i=0; hbox3tit[i]; i++){
                hbox3tit[i].style.height = hitem3 - 1 + 'px';
                hbox3tit[i].style.lineHeight = hitem3 - 1 + 'px';}


            //pop
            // let _pop = document.querySelectorAll<HTMLElement>( '.PICKINGTABLET .contentf .pop');
            for(let i=0; _pop[i]; i++){ _pop[i].style.height = hcont + 'px'; }


            //arrow
            // let draw = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contleft .itemp .box2 .draw .myArrow');
            // for(let i=0; draw[i]; i++){
            //     draw[i].style.height =Math.floor(hitem - 5) + "px"
            //     draw[i].style.width = Math.floor(wItem - 1) + 'px';
            // }

            //ArrowDirection
            // this.ArrowDirectionLeft(hitem - 5, wItem - 1);
            // this.ArrowDirectionRight(hitem - 5, wItem - 1);

            this.ArrowImgLeft(hitem, wItem);
            this.ArrowImgRight(hitem, wItem);
        }

        ArrowImgLeft(p_heightArrow :number, p_widthArrow: number){

            let _box2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contleft .itemp .box2 .draw');
            for(let i=0; _box2[i]; i++){


                let _ArrowConn = _box2[i].querySelector<HTMLElement>('.ArrowDirection');
                let _heightArrow = p_heightArrow;
                let _widthArrow = p_widthArrow;
                if(_ArrowConn){

                    let _arrowCount = Number(_ArrowConn.textContent);
                    let _arrow_img = _box2[i].querySelector<HTMLImageElement>('.ImgArrow');


                    if(_arrowCount > 0){
                        let _isheightShow =  _box2[i].getAttribute("hasModel");
                        let _heightshow = (_isheightShow!='N') ? (_heightArrow/4): 0;

                        _heightArrow = _heightArrow * (_arrowCount + 1);
                        _arrow_img.src= "/assets/common/images/arrow-left-dow.png";

                    }

                    if(_arrowCount < 0){
                        _heightArrow = _heightArrow * ((_arrowCount*-1) + 1);
                        _arrow_img.style.marginTop  = "-" + Math.floor(_heightArrow/2)  + "px";
                        _arrow_img.src= "/assets/common/images/arrow-left-up.png";
                    }


                    _arrow_img.style.height =Math.floor(_heightArrow) + "px"
                    _arrow_img.style.width = Math.floor(_widthArrow) + 'px';

                }
            }
        }
        // phuongdv
        ArrowImgRight(p_heightArrow :number, p_widthArrow: number){
            let _box2 = document.querySelectorAll<HTMLElement>('.PICKINGTABLET .contright .itemp .box2 .draw');
            for(let i=0; _box2[i]; i++){

                let _ArrowConn = _box2[i].querySelector<HTMLElement>('.ArrowDirection');
                let _heightArrow = p_heightArrow;
                let _widthArrow = p_widthArrow;
                if(_ArrowConn){

                    let _arrowCount = Number(_ArrowConn.textContent);
                    let _arrow_img = _box2[i].querySelector<HTMLImageElement>('.ImgArrow');

                    if(_arrowCount > 0){
                        let _isheightShow =  _box2[i].getAttribute("hasModel");
                        let _heightshow = (_isheightShow!='N') ? (_heightArrow/4): 0;

                        _heightArrow = _heightArrow * (_arrowCount + 1);
                        _arrow_img.src= "/assets/common/images/arrow-right-dow.png";
                    }

                    if(_arrowCount < 0){
                        _heightArrow = _heightArrow * ((_arrowCount*-1) + 1);
                        _arrow_img.style.marginTop  = "-" + Math.floor(_heightArrow/2)  + "px";
                        _arrow_img.src= "/assets/common/images/arrow-right-up.png";
                    }


                    _arrow_img.style.height =Math.floor(_heightArrow) + "px"
                    _arrow_img.style.width = Math.floor(_widthArrow) + 'px';
                }
            }
        }


        initLayoutData(){

            this.loadtotal = 0;
            let countload = 1;

            let urlParams = new URLSearchParams(window.location.search);
            this.PickingPosition = urlParams.get('PickingPosition');
            this.TabletId = urlParams.get('TabletId');

            this. _serviceProxy.mstLgwPickingTabletProcessGetdataByLayout(this.PickingPosition, this.TabletId)
                .pipe(finalize(() => { this.completeLayoutData(countload) }))
                .subscribe((result) => {
                    if(result){

                        this.rowDataColumn1 = result.filter(a => a.colNo === 1);
                        this.rowDataColumn2 = result.filter(a => a.colNo === 2);
                        this.rowData =  result;


                    }
                });

        }

        completeLayoutData(count:number){
            this.loadtotal+=1;
            if(this.loadtotal==count) {
                console.log("------------- drawScreen:" + this.loadtotal);
                this.repeatBindData();
                setTimeout(() => { this.drawScreen(); }, 500);
            }
        }

        ngOnDestroy(): void{
            clearTimeout(this.timersrepeatBindData);
        }

        timersrepeatcount:number = 0;
        timersrepeatBindData;
        refeshPage: number = 600;
        repeatBindData() {

            if (this.timersrepeatcount > this.refeshPage) window.location.reload();
            this.timersrepeatcount = this.timersrepeatcount + 1;
            try{
                this.LOAD_DATA();

                // console.log("repeat this.repeatBindData():"+ this.timersrepeatcount);
                this.timersrepeatBindData = setTimeout(()=>{
                    this.repeatBindData();
                }, 1000);

        }catch(ex){

            this.timersrepeatBindData = setTimeout(() => {
                this.repeatBindData();
            }, 1000);
        }
        }

        LOAD_DATA() {

            this. _serviceProxy.mstLgwPickingTabletProcessGetdataByLayout(this.PickingPosition, this.TabletId)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                        this.TackTime  = result[0].taktTime;
                        this.CAN_CALL_UNPACKING = result[0].canCallUnpacking;
                        this.IS_CALL_LEADER =  result[0].isCallLeader;
                        this.SEQ_NO = result[0].seqNo;
                        if(result[0].startTime){
                            document.querySelector<HTMLElement>('.PICKINGTABLET .headerf.startfinish span').style.backgroundColor  = '#5ebdec';
                        }
                        else {
                            document.querySelector<HTMLElement>('.PICKINGTABLET .headerf.startfinish span').style.backgroundColor  = 'white';
                        }

                        this.BIND_DATA(result);
                    }
                });
        }
        BIND_DATA(_data:MstLgwPickingTabletProcessGetdataByLayoutOutput[]){
            _data.forEach(d => {
                let el = document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .cont .leftcolbutton .itemp.index' + d.logicSequenceNo);
                if(el){
                    let _box1 = el.querySelector<HTMLElement>('.box1');

                    let ispushed = _box1.querySelector<HTMLElement>('.line1');
                    if(ispushed){
                        let _isPushed = 'IsPushed_' + this.fn.isActive(d.isPushed);
                        ispushed.classList.remove('IsPushed_N', 'IsPushed_Y');
                        ispushed.classList.add(_isPushed);
                    }

                    let hasModel = _box1.querySelector<HTMLElement>('.line2');
                    if(hasModel){
                        let _hasModel = 'hasModel_' + this.fn.isActive(d.hasModel);
                        hasModel.classList.remove('hasModel_N', 'hasModel_Y', 'hasModel_G');
                        hasModel.classList.add(_hasModel);

                    }

                    let _process = _box1.querySelector<HTMLElement>('.line1 .subcol.val');
                    if (_process) _process.textContent = d.process;

                    if(this.fn.isActive(d.hasModel) != 'N') {
                        let _model = _box1.querySelector<HTMLElement>('.line2 .subline.model');
                        _model.textContent = d.model;
                        let _lotCode = _box1.querySelector<HTMLElement>('.line2 .subline.lotcode');
                        _lotCode.textContent = d.lotCode;
                    }

                    let _box2 = el.querySelector<HTMLElement>('.box2 .draw');
                    if(_box2) _box2.setAttribute("hasModel", this.fn.isActive(d.hasModel));


                    //let _box3 = el.querySelector<HTMLElement>('.box3');

                }

                let el3 = document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .cont .leftcollabel .itemp.index' + d.logicSequenceNo);
                if(el3){

                    let ispushed = el3.querySelector<HTMLElement>('.box1 .line1');
                    let _isPushedBox3 = 'IsPushed_' + this.fn.isActive(d.lblIsPushed);
                    if(ispushed){
                        let ispushedBox3 = el.querySelector<HTMLElement>('.box3 .val');
                        ispushedBox3.classList.remove('IsPushed_N', 'IsPushed_Y');
                        ispushedBox3.classList.add(_isPushedBox3);
                    }

                }

            });
        }

        PROCESSCALL(logicSequenceNo:string, seqNo:string) {
            try{

                this. _serviceProxy.mstLgwPickingTabletProcessSendSignal(this.PickingPosition, logicSequenceNo, seqNo, this.TabletId)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    // if(result){
                    //     alert('call : ' +  logicSequenceNo  +  '  ' +  seqNo + ' success!');
                    // }else{
                    //     alert(result + ' fail:  EXEC LGW_PIK_PICKING_PROGRESS_SEND_SIGNAL ' + this.PickingPosition + ', ' +logicSequenceNo+ ', ' +seqNo+ ', ' + this.TabletId);
                    // }
                });

            }catch(ex){ alert(ex);  }



        }
        CTRL_CALL() {
            document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .pop1').style.display = 'block';
        }
        START_FINISH() {
            try{
                this. _serviceProxy.mstLgwPickingTabletProcessStartFinish(this.PickingPosition, this.TabletId)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                       alert('EXEC LGW_PIK_PICKING_PROGRESS_START_FINISH ' + this.PickingPosition + ', ' + this.TabletId + '  ----Update Success!');
                    }
                });

            }catch(ex){ alert(ex);  }
        }
        CALL_LEADER() {
            try{
                this. _serviceProxy.mstLgwPickingTabletProcessCallLeader(this.PickingPosition, this.SEQ_NO, this.TabletId)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                       alert('EXEC LGW_PIK_PICKING_PROGRESS_CALL_LEADER ' + this.PickingPosition + ', ' + this.TabletId + '  ----Call Success!');
                    }
                });
            }catch(ex){ alert(ex);  }


        }


        UNPACK_LOT() {
            try{
                this. _serviceProxy.mstLgwPickingTabletProcessCallUPLot(this.PickingPosition, this.TabletId)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                        alert('Gọi hàng thành công');
                        document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .pop1').style.display = 'none';
                    }
                });

            }catch(ex){ alert(ex);  }

        }

        UNPACK_PXP() {
            this.popupUnpack2.show(this.PickingPosition, this.TabletId);
        }


        SavePopupUnpack(){

        }
        ClosePopupUnpack(){
            document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .pop1').style.display = 'none';
        }

        GET_PICKING_POISITION() {

            if(this.PickingPosition == '' || this.PickingPosition == null){
                let urlParams = new URLSearchParams(window.location.search);
                this.PickingPosition = urlParams.get('PickingPosition');
                this.TabletId = urlParams.get('TabletId');
            }

            return this.PickingPosition;
        }

        GET_IS_CALL_LEADER() {
            if(this.IS_CALL_LEADER){
                return this.IS_CALL_LEADER;
            }
            return '';
        }

        CLOSE(){
            document.querySelector<HTMLElement>('.PICKINGTABLET .contentf .pop1').style.display = 'none';
        }


        COLUMN1_LEFT(){
            return this.rowDataColumn1;
        }
        COLUMN1_LABEL_LEFT(){
            return this.rowDataColumn1.filter(a => a.lblIsHidden != "Y");;
        }
        COLUMN2_RIGHT(){
            return this.rowDataColumn2;
        }
        COLUMN2_LABEL_RIGHT(){
            return this.rowDataColumn2.filter(a => a.lblIsHidden != "Y");;
        }

        fornumbersRange(start:number, stop:number, step:number){
            let numRange: number[] = [];
            for (let i = start; i <= stop;) {
                numRange.push(i);
                i = i + step;
            }
            return numRange;
        }

        fornumbersRangeDesc(start:number, stop:number, step:number){
            let numRangeDesc: number[] = [];
            for (let i = start; i >= stop;) {
                numRangeDesc.push(i);
                i = i + step;
            }
            return numRangeDesc;
        }


        fornumbers(num:number) {
            let numbers:Array<any> = Array.from({length:num},(v,k)=>k+1);
            return numbers;
        }
}
