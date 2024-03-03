import { LookupModule } from './../../../master/common/lookup/lookup.module';
import { QuerySelector } from '@ag-grid-enterprise/all-modules';

import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

//app
import { finalize } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//form popup
import { LgaEkbProgressScreenServiceProxy, LgaEkbProgressScreenDto, MstLgaEkbUserDto, LgaBp2BigPartDDLServiceProxy, MstCmmLookupDto } from '@shared/service-proxies/service-proxies';




@Component({
    templateUrl: './progressscreen.component.html',
    styleUrls: [
        './progressscreen.component.less',
    ],
})



export class  ProgressScreenComponent extends AppComponentBase {


    datePipe = new DatePipe('en-US');
    fn: CommonFunction = new CommonFunction();
    rowData: LgaEkbProgressScreenDto[];
    PickingMember: MstLgaEkbUserDto[];
    lookupData: MstCmmLookupDto;

    MODE: string = 'PK';
    PROD_LINE:  string = 'A1';
    PICKING_MEMBER:string = "PK1_A1"; // PIK USER CODE e.g. PK1_A1

    constructor(
        injector: Injector,
        private _serviceProxy: LgaEkbProgressScreenServiceProxy,
        private _serviceLookup: LgaBp2BigPartDDLServiceProxy,
    ) {
        super(injector);
        console.log('1: constructor');

        this.getParameter();

        this._serviceLookup.getItemValue('LGA_EKB_EKANBAN_PROGRESS_SCREEN', 'MAX_LINE_PER_PAGE_' + this.PROD_LINE.toUpperCase())
        .subscribe((result) => {
            if (result) {
                this.lookupData = result;
                // this.BindPage(result);
            }
        });

        // this.repeatBindData();
        this._serviceProxy.lgaEkbEkanbanProgressGetDataScreen(this.PICKING_MEMBER, this.MODE)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.rowData = result;
                if (result) {
                    this.bindPickingmenber();
                    this.BindPage();
                    this.drawScreen();
                    this.BTN_UPDATE_STATUS();

                    // current page?

                    this.repeatBindData();
                }
            });
    }

    ngOnInit(): void {
        console.log('2: ngOnInit');
    }

    ngAfterViewInit() {
        console.log('3: ngAfterViewInit');
        this.drawScreen();
       // this.fn.loading(true);
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        //alert('4: onWindowResize')
        this.drawScreen();
            // this.notify.success('Height: ' + hButton);
    }
    ////end set width height


    ngOnDestroy(): void{
        // this.timerstorage = null;
        clearTimeout(this.timer_repeat);
    }

     getParameter() {
        let urlParams = new URLSearchParams(window.location.search);
        let _prodLine  = urlParams.get('prodLine');
        let _userCode  = urlParams.get('userCode');

        if(_prodLine == null || _prodLine=="") return;

        this.PICKING_MEMBER = _userCode ;
        this.PROD_LINE = _prodLine;
    }


    totalItem:number = 4; // total count
    numInPage:number = 4;   //phuongdv
    pageCount: number =  Math.ceil(this.totalItem/ this.numInPage);
    BindPage(){

        if(this.lookupData) {
            this.totalItem = Number(this.lookupData.itemValue);
            this.numInPage = Number(this.lookupData.itemValue);
        }
        if(this.rowData && this.rowData.length > 0) {
            this.totalItem = (this.rowData.length < this.numInPage) ? this.numInPage: this.rowData.length;
        }

        this.pageCount = Math.ceil(this.totalItem/ this.numInPage);
        console.log('pageCount: ' + this.pageCount + ' totalItem: ' + this.totalItem );
    }


    timecount:number = 0;
    isfirst:number = 0;
    refeshPage: number = 600; // 10phut
    timer_repeat;
    secondDelay = 1000;
    repeatBindData(){

        if (this.timecount > this.refeshPage) window.location.reload();
        this.timecount = this.timecount + 1;
        this.isfirst = this.isfirst + 1;
        try {

            this.getData();
            this.fn.showtime('time_now_log');

        } catch(ex) {
            console.log(ex);

            this.timer_repeat = setTimeout(() => {
                this.repeatBindData();
            }, this.secondDelay);
        }
    }

    getData() {
            // this.fn.loading(true);
            this._serviceProxy.lgaEkbEkanbanProgressGetDataScreen(this.PICKING_MEMBER, this.MODE)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.rowData = result;
                if (result) {

                    // if(this.totalItem != result.length || this.isfirst <= 3)  {

                    this.BindPage();
                    this.drawScreen();
                    // }
                    // else  if (this.isfirst <= 3) {
                    //     console.log('this.isfirst <= 3');
                    //     this.drawScreen();
                    //     this.BTN_UPDATE_STATUS();
                    // }

                    this.BindData(result);
                    this.BTN_UPDATE_STATUS();

                }else {
                    this.BindPage();
                    this.drawScreen();

                    this.BindData(result);
                    this.BTN_UPDATE_STATUS();

                }

                // this.fn.loading(false);
                this.timer_repeat = setTimeout(() => {
                    this.repeatBindData();
                }, this.secondDelay);
            },(error) => {
                //this.fn.loading(false);
                this.timer_repeat = setTimeout(() => {
                    this.repeatBindData();
                }, this.secondDelay);
        });
    }

    BindData(_data: LgaEkbProgressScreenDto[]){
        try {
            //clear data defaul 4 dòng
            let _rowItems = document.querySelectorAll<HTMLElement>('.MODE_'+this.MODE+' .progress_line .progress_line_warp');
            for(let k = 0; _rowItems[k]; k++ ){

                _rowItems[k].querySelector<HTMLElement>('.progress_line_column.GIA .val').textContent = "";
                _rowItems[k].querySelector<HTMLElement>('.progress_line_column.BACKNO .val').textContent = "";
                _rowItems[k].setAttribute('EKanban_ID', "");
                _rowItems[k].classList.remove('STARTED', 'FINISHED', 'START_DEL', '_DATA_', 'DEL_DATA');

                let _btn = _rowItems[k].querySelector<HTMLElement>('.progress_line_column.DA_NHAT .btnAdd');
                if(_btn) {
                    _btn.setAttribute('PHAINHAT', "" );
                    _btn.setAttribute('DANHAT', "" );
                    _btn.setAttribute('EKanban_ID', "" );
                }
                if(this.MODE == "PK") {
                    _rowItems[k].querySelector<HTMLElement>('.progress_line_column.PHAINHAT .val').textContent =  "";
                    _rowItems[k].querySelector<HTMLElement>('.progress_line_column.DA_NHAT .val').textContent =  "";
                }
                else if(this.MODE == "DEL") {
                    _rowItems[k].querySelector<HTMLElement>('.progress_line_column.SOLUONGPHAT .val').textContent =  "";
                }
                let _btnadd = _rowItems[k].querySelector<HTMLElement>('.progress_line_column.DA_NHAT .btnAdd');
                if(_btnadd) _btnadd.classList.remove("ACTIVE");
            }
            // console.log("binddata");
            if(_data){
                // console.log("binddata 1" );
                if(_data.length > 0){
                    // console.log("binddata 2 _data.length" );
                    let _isCallLeader = this.fn.isActive(_data[0].isCallLeader);
                    if(_isCallLeader == "Y"){
                        let btnCallLD = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.CALL_LD');
                        if(!btnCallLD.classList.contains('CALL_LEADER_Y')) btnCallLD.classList.add('CALL_LEADER_Y');

                        let obj = document.querySelector<HTMLElement>('.MODE_'+(this.MODE=='PK') ? 'DEL': 'PK' + ' .btnUi.CALL_LD');
                        if (obj) obj.classList.remove('CALL_LEADER_Y');
                    }
                    else {
                        let btnCall =document.querySelectorAll<HTMLElement>('.btnUi.CALL_LD');
                        for(let i=0; btnCall[i]; i++) btnCall[i].classList.remove('CALL_LEADER_Y');
                    }

                    //COUNT_DOWN
                    document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .COUNT_DOWN').textContent = (_data[0].taktTime - _data[0].actualTime).toString();
                    // #FFD966
                }

                let finish_item = 0;

                for(let i=0; i< (_data.length + 4); i++ )
                {
                    let _row:LgaEkbProgressScreenDto;
                    if(i<_data.length)  _row = _data[i];
                    else _row = new LgaEkbProgressScreenDto();

                    // console.log( _row.partSequence + ' ' + _row.backNo + ' ' + i)

                    let rowItem = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .progress_line.ROW_NO' + _row.partSequence + ' .progress_line_warp');
                    if(rowItem){
                        // console.log(_data[i].backNo);
                        // console.log(_data[i].spsAddress);
                        rowItem.querySelector<HTMLElement>('.progress_line_column.GIA .val').textContent =  _row.spsAddress; //spsAddress  pcAddress
                        rowItem.querySelector<HTMLElement>('.progress_line_column.BACKNO .val').textContent =   _row.backNo;

                        rowItem.setAttribute('EKanban_ID', (_row.id).toString() );
                        rowItem.classList.add("_DATA_");

                        let btn = rowItem.querySelector<HTMLElement>('.progress_line_column.DA_NHAT .btnAdd');
                        if(btn) {
                            btn.setAttribute('PHAINHAT', (_row.requestQty - _row.qty).toString() );
                            btn.setAttribute('DANHAT', (_row.qty).toString() );
                            btn.setAttribute('EKanban_ID', (_row.id).toString() );
                        }

                        //status
                        let _status = this.fn.isStatus(_row.status, '');
                        //rowItem.classList.remove('STARTED', 'FINISHED', 'START_DEL');

                        if(this.MODE == "PK") {

                            rowItem.querySelector<HTMLElement>('.progress_line_column.PHAINHAT .val').textContent =  _row.requestQty.toString();
                            rowItem.querySelector<HTMLElement>('.progress_line_column.DA_NHAT .val').textContent =  (_row.qty == 0)? "":_row.qty.toString();
                            // rowItem.querySelector<HTMLElement>('.progress_line_column.ZERO_KB').textContent =  _row.backNo;
                            // rowItem.querySelector<HTMLElement>('.progress_line_column.STOCK_UP_PC').textContent =  _row.backNo;

                        }
                        else if(this.MODE == "DEL") {
                            rowItem.querySelector<HTMLElement>('.progress_line_column.SOLUONGPHAT .val').textContent =  _row.qty.toString();
                            // console.log(this.MODE  + " - " + _row.qty.toString())
                            rowItem.classList.add("DEL_DATA");
                        }


                        if(_status!='') rowItem.classList.add(_status);
                        if(_row.qty != 0 && this.MODE == "PK") rowItem.classList.add("STARTED");
                        if(_status == "START_DEL" && this.MODE == "PK")  finish_item++;
                        if(_status == "FINISHED" && this.MODE == "DEL")  finish_item++;
                        //btnadd
                        if(_row.id > 0)
                        {
                            let _btnadd = rowItem.querySelector<HTMLElement>('.progress_line_column.DA_NHAT .btnAdd');
                            if(_btnadd) _btnadd.classList.add("ACTIVE");

                        } else {
                            let _btnadd = rowItem.querySelector<HTMLElement>('.progress_line_column.DA_NHAT .btnAdd');
                            if(_btnadd) _btnadd.classList.remove("ACTIVE");
                        }
                    }
                }

                //16/16
                document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .WORK_ITEM').textContent = finish_item + "/" + _data.length;
            }

        } catch(ex){
            console.log(ex);
        }
    }




    // _pageIndex start 1
    bindDataInPage(_pageIndex:number){
        let startRowIndex = this.numInPage*(_pageIndex-1) + 1; //_pageIndex start 0
        return startRowIndex;
    }



    dataPage() {
        return this.fornumbers_next(this.pageCount,1)
    }

    fornumbers_next(num:number, start:number) {
        let numbers:Array<any> =  Array.from({length:num},(v,k)=>k+start);
        return numbers;
    }


    CHANGE_MODE(_mode:string){

        this.isfirst = 0;
        this.MODE = _mode;
        // this.drawScreen();
        this.BTN_UPDATE_STATUS();
    }


    PAGE_NEXT(){
        //check disable  - phuongdv
        let btnnext = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.PAGE_NEXT');
        if (btnnext.classList.contains('DISABLE')){
            return;
        }



        if (this.MODE == "DEL" && btnnext.classList.contains('FINISH')){
            // call finish
            this.fn.loading(true);
            this.NEXT_PAGE_DEL();
            //LgaEkbEkanbanProgressFinishDel
            //this.FINISH_DEL();
            return;
        }else {
            // alert("NEXT PAGE");
            // return;
            let page_active_old = document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.ACTIVE');
            if(page_active_old){
                let _page = parseInt(page_active_old.getAttribute("title"));
                _page = _page + 1;
                // this.CountDataActual = _page;
                let page_active = document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.PAGE' + _page);
                if(page_active){
                    if(this.MODE == "DEL") {
                        // update với mode = DEL

                        this.NEXT_PAGE_DEL(); //phuongdv next_page no finish

                        page_active_old.classList.remove("ACTIVE");
                        page_active.classList.add("ACTIVE");
                        this.BTN_UPDATE_STATUS();
                        console.log('PAGE_NEXT / DEL');
                    }
                    else {
                        page_active_old.classList.remove("ACTIVE");
                        page_active.classList.add("ACTIVE");
                        this.BTN_UPDATE_STATUS();
                        console.log('PAGE_NEXT / PK');
                    }
                }
            }
        }



    }

    FINISH_DEL() {
        try{

            this._serviceProxy.lgaEkbEkanbanProgressFinishDel(this.PROD_LINE, this.PICKING_MEMBER, this.MODE)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                if(result > 0) {
                    console.log('FINISH / '+this.MODE+' / :Success!');
                }else {
                    console.log('FINISH / '+this.MODE+' / : fail??!');
                }
                this.fn.loading(false);
            });
        }catch(e){
            console.error(e);
        }
    }

    FINISH_DEL_ITEM(row_no:number) {
        try{
            let _row_no = document.querySelector<HTMLElement>('.MODE_DEL .progress_page.ACTIVE .progress_line.ROW_NO' +row_no + ' .progress_line_warp.DEL_DATA');
            if(_row_no){
                this.fn.loading(true);

                let ekanban_id = _row_no.getAttribute('ekanban_id');
                this._serviceProxy.lgaEkbEkanbanProgressConfirmDel(this.PROD_LINE, this.PICKING_MEMBER, ekanban_id, this.MODE)
                .pipe(finalize(() => { }))
                .subscribe((result) => {
                    if(result > 0) {
                        console.log('CONFIRM DEL('+ekanban_id+'):Success!');
                    }else {
                        console.log('CONFIRM DEL('+ekanban_id+') : fail??!');
                    }
                    this.fn.loading(false);
                });

            }
            else console.log("DEL_DATA("+row_no+") : NOT EXISTS");


        }catch(e){
            console.error(e);
        }
    }

    NEXT_PAGE_DEL() {
        let rowItems = document.querySelectorAll<HTMLElement>('.MODE_'+this.MODE+' .progress_page.ACTIVE .progress_line .progress_line_warp');
        for(let i = 0; i < rowItems.length; i++){
            try{
                let _ekanban_id = rowItems[i].getAttribute("ekanban_id");
                // console.log(_ekanban_id);
                if (_ekanban_id == null || _ekanban_id == "" ) continue;

                this._serviceProxy.lgaEkbEkanbanProgressNextPageDel(this.PROD_LINE, this.PICKING_MEMBER, _ekanban_id, this.MODE)
                .pipe(finalize(() => { }))
                .subscribe((result) => {
                    if(result > 0) {
                        console.log('NEXT PAGE / '+this.MODE+' / '+_ekanban_id+': Success!');
                    }else {
                        console.log('NEXT PAGE / '+this.MODE+' / '+_ekanban_id+': fail??!');
                    }
                    this.fn.loading(false);
                });
            }catch(e){
                console.error(e);
            }
        }
    }

    PAGE_BACK(){
        //check disable  - phuongdv
        let btnnext = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.PAGE_BACK');
        if (btnnext.classList.contains('DISABLE')){
            return;
        }


        let page_active_old = document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.ACTIVE');
        if(page_active_old){
            let _page = parseInt(page_active_old.getAttribute("title"));
            _page = _page - 1;
            // this.CountDataActual = _page;
            let page_active = document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.PAGE' + _page);
            if(page_active){
                page_active_old.classList.remove("ACTIVE");
                page_active.classList.add("ACTIVE");
                this.BTN_UPDATE_STATUS();
                console.log('PAGE_BACK');
            }
        }
    }

    BTN_UPDATE_STATUS(){



            let page_active = document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.ACTIVE');
            if(!page_active) {  document.querySelector<HTMLElement>('.MODE_'+this.MODE+ ' .progress_page.PAGE1').classList.add("ACTIVE"); }
            let  _page = parseInt(page_active != null ? page_active.getAttribute("title") : "1");


            //check next btn
            //console.log('_page: ' + _page + ', PageCount:' + this.PageCount);

            let btnnext = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.PAGE_NEXT');
            let btnfinish_del = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.FINISH_DEL');

            if (this.pageCount == _page){
                // console.log("BTN_UPDATE_STATUS")
                if(this.MODE == "PK") {
                    if (!btnnext.classList.contains('DISABLE')){
                        btnnext.classList.add('DISABLE');
                    }
                } else if (this.MODE == "DEL") {
                    btnnext.classList.add('FINISH');
                    //btnnext.querySelector<HTMLElement>('span').textContent = "FINISH";
                    let rowItems = document.querySelectorAll<HTMLElement>('.MODE_'+this.MODE+' .progress_line .progress_line_warp._DATA_:not(.FINISHED)');
                    // console.log("-- " + rowItems.length)
                    if(rowItems.length == 0) {
                        btnnext.classList.add('DISABLE');
                    }else {
                        btnnext.classList.remove('DISABLE');
                    }
                }

            } else{
                if (btnnext.classList.contains('DISABLE')) btnnext.classList.remove('DISABLE');
                if (this.MODE == "DEL") {
                    btnnext.classList.remove('FINISH');
                    btnnext.querySelector<HTMLElement>('span').textContent = "NEXT PAGE";
                }
            }

            //check back btn
            let btnback = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.PAGE_BACK');
            if (_page == 1){
                if (!btnback.classList.contains('DISABLE')){
                    btnback.classList.add('DISABLE');
                    // this.BTN_BACK();
                }
            }else{
                if (btnback.classList.contains('DISABLE')) btnback.classList.remove('DISABLE');
            }

    }


    UNDO(){

        this.fn.loading(true);
        try{
            this._serviceProxy.lgaEkbEkanbanProgressUndo(this.PROD_LINE, this.PICKING_MEMBER)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                console.log('UNDO:' + result);
                this.fn.loading(false);
            });
        }catch(e){
            console.log(e);
            this.fn.loading(false);
        }
    }

    NEXT_C() {
        this.fn.loading(true);
        try{
            this._serviceProxy.lgaEkbEkanbanProgressNextC(this.PROD_LINE, this.PICKING_MEMBER)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                console.log('NEXT_C:' + result);
                this.fn.loading(false);
            });
        }catch(e){
            console.log(e);
            this.fn.loading(false);
        }
    }

    CALL_LD() {
        try{
            this.fn.loading(true);

            let btnCallLD = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .btnUi.CALL_LD');
            if(btnCallLD.classList.contains('CALL_LEADER_Y')) btnCallLD.classList.remove('CALL_LEADER_Y');
            else btnCallLD.classList.add('CALL_LEADER_Y');

            let obj = document.querySelector<HTMLElement>('.MODE_'+(this.MODE=='PK') ? 'DEL': 'PK' + ' .btnUi.CALL_LD');

            if(obj) obj.classList.remove('CALL_LEADER_Y');

            console.log('.MODE_'+(this.MODE=='PK') ? 'DEL': 'PK' + ' .btnUi.CALL_LD')

            this._serviceProxy.lgaEkbEkanbanProgressCallLD(this.PROD_LINE, this.PICKING_MEMBER)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                console.log('CALL LD:' + result);
                this.fn.loading(false);
            });
        }catch(e){
            console.log(e);
            this.fn.loading(false);
        }
    }

    drawScreen(){

        ///////////////////////////design width
        let w = window.innerWidth;
        let w_content = w-6;
        let w_progress_line_columnPK =  Math.floor((w_content / 7) * 100) / 100;
        let w_progress_line_columnDEL =  Math.floor((w_content / 5) * 100) / 100;

        //title pk
        let o_title_colPK = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop.MODE_PK .title.left .title_column");
        for(let i=0; o_title_colPK[i] ; i++){  o_title_colPK[i].style.width = (w_progress_line_columnPK) + 'px';  }
        //title del
        let o_title_colDEL = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop.MODE_DEL .title.left .title_column");
        for(let i=0; o_title_colDEL[i] ; i++){  o_title_colDEL[i].style.width = (w_progress_line_columnPK) + 'px';  }
        //btn top
        let o_btn_top = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop .title.right .btnUi");
        for(let i=0; o_btn_top[i] ; i++){  o_btn_top[i].style.width = (w_progress_line_columnPK-10) + 'px';  }
        //btn bot
        let o_btn_bot = document.querySelectorAll<HTMLElement>(".PTA .Bottom_Content .btnUi");
        for(let i=0; o_btn_bot[i] ; i++){  o_btn_bot[i].style.width = (w_progress_line_columnPK*1.5) + 'px';  }

        //content
        let o_content = document.querySelectorAll<HTMLElement>('.PTA .progress_line .progress_line_warp, .PTA .HeaderTitle, .PTA .Bottom_Content');
        for(let i=0; o_content[i] ; i++){  o_content[i].style.width = (w_content) + 'px';  }

        let o_progress_line_columnPK = document.querySelectorAll<HTMLElement>('.PTA .Progress_Content.MODE_PK .progress_line_column, .PTA .HeaderTitle.MODE_PK .progress_line_column');
        for(let i=0; o_progress_line_columnPK[i] ; i++){  o_progress_line_columnPK[i].style.width = (w_progress_line_columnPK) + 'px';  }

        let o_progress_line_columnPK_BACKNO = document.querySelectorAll<HTMLElement>('.PTA .Progress_Content.MODE_PK .progress_line_column.BACKNO, .PTA .HeaderTitle.MODE_PK .progress_line_column.BACKNO');
        for(let i=0; o_progress_line_columnPK_BACKNO[i] ; i++){  o_progress_line_columnPK_BACKNO[i].style.width = (w_progress_line_columnPK*2) + 'px';  }

        let o_progress_line_columnDEL = document.querySelectorAll<HTMLElement>('.PTA .Progress_Content.MODE_DEL .progress_line_column, .PTA .HeaderTitle.MODE_DEL .progress_line_column');
        for(let i=0; o_progress_line_columnDEL[i] ; i++){  o_progress_line_columnDEL[i].style.width = (w_progress_line_columnDEL) + 'px';  }

        let o_progress_line_columnDEL_BACKNO = document.querySelectorAll<HTMLElement>('.PTA .Progress_Content.MODE_DEL .progress_line_column.BACKNO, .PTA .HeaderTitle.MODE_DEL .progress_line_column.BACKNO');
        for(let i=0; o_progress_line_columnDEL_BACKNO[i] ; i++){  o_progress_line_columnDEL_BACKNO[i].style.width = (w_progress_line_columnDEL*3) + 'px';  }




        ///////////////////////////design height
        let h = window.innerHeight;
        // h = Math.floor((h / 2) * 100) / 100;    //debug


        // let c_progressline =  this.numInPage;
        // let c_progressline = 4;
        // if(o_progressline) c_progressline = (o_progressline.length < 4 ? 4 :o_progressline.length);
        let c_progressline = this.numInPage + 2;

        let h_progressline = Math.floor((h / c_progressline) * 100) / 100;
        let h_progressline_title = Math.floor((h_progressline / 3) * 100) / 100;

        //title text
        let o_title_col_txt = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop .title.left .title_column .title_txt");
        for(let i=0; o_title_col_txt[i] ; i++){  o_title_col_txt[i].style.height = (h_progressline_title) + 'px';  }
        //title value
        let o_title_col_val = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop .title.left .title_column .title_val");
        for(let i=0; o_title_col_val[i] ; i++){  o_title_col_val[i].style.height = (h_progressline_title*2) + 'px';  }

        let o_combobox = document.querySelector<HTMLElement>(".PTA .HeaderTop .title.left .title_column .title_val .LISTCOMBO");
        if(o_combobox) o_combobox.style.top = (h_progressline_title*2) -10 + "px";

        //btn top
        //let o_btn = document.querySelectorAll<HTMLElement>(".PTA .HeaderTop .title.right .btnUi_warp");
        for(let i=0; o_btn_top[i] ; i++){  o_btn_top[i].style.height = (h_progressline-10) + 'px';  }
        //btn bot
        // let o_btn_bot = document.querySelectorAll<HTMLElement>(".PTA .Bottom_Content .btnUi");
        for(let i=0; o_btn_bot[i] ; i++){  o_btn_bot[i].style.height = (((h_progressline_title*2))-10) + 'px';  }

        //content
        let i_progressline = document.querySelectorAll<HTMLElement>('.PTA .PTA_Body .progress_line, .PTA .PTA_Body .progress_line .progress_line_warp');
        for(let i=0; i_progressline[i] ; i++){  i_progressline[i].style.height = (h_progressline) + 'px';  }

        let o_progressline_title = document.querySelectorAll<HTMLElement>('.PTA .PTA_Body .HeaderTitle');
        for(let i=0; o_progressline_title[i] ; i++){  o_progressline_title[i].style.height = (h_progressline_title) + 'px';  }


        let o_progressline_bottom = document.querySelectorAll<HTMLElement>('.PTA .PTA_Body .Bottom_Content');
        for(let i=0; o_progressline_bottom[i] ; i++){  o_progressline_bottom[i].style.height = (h_progressline_title*2) + 'px';  }

    }


    NumberSelect(_num:number){

        let _input =  document.querySelector<HTMLInputElement>('.NUMBER .TEXT .INPUT_QTY');
        let _val = _input.value
        _val = _val + '' + _num;
        _input.value  = _val;

    }

    NUMBER_BACKSPACE() {
        let _input =  document.querySelector<HTMLInputElement>('.NUMBER .TEXT .INPUT_QTY');
        let _val = _input.value
        if(_val.length){
            _val = _val.substring(0, _val.length - Math.min(1, _val.length));

            _input.value  = _val;
        }


    }

    StatusOk: boolean = false;
    NUMBER_OK() {

        if (this.StatusOk) return;

        let _input =  document.querySelector<HTMLInputElement>('.NUMBER .TEXT .INPUT_QTY');
        let _val = _input.value
        //validate
        if(_val.length <=0)
        {
            alert('chua nhập số lượng');
            return;
        }
        let _num = Number(_val);
        let _btn = document.querySelector<HTMLElement>('.MODE_'+this.MODE+' .progress_line.ROW_NO' + this.NUMBER_ROW + ' .progress_line_warp .DA_NHAT .btnAdd');
        let _btnphainhat = _btn.getAttribute('PHAINHAT');

        let _btnEkanbanId = _btn.getAttribute('EKanban_ID');
        let _phainhat = Number(_btnphainhat);
        if(_num > _phainhat){
            alert('vượt quá số lượng phải nhặt');
            return;
        }


        try{
            this.StatusOk = true;
            this._serviceProxy.lgaEkbEkanbanProgressInputQty(this.PROD_LINE, this.PICKING_MEMBER, _btnEkanbanId, _num.toString())
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                console.log('INPUT QTY result ' + result);
                this.NUMBER_CANCEL();
                this.StatusOk = false;
            });
        }catch(e){
            console.log(e);
        }

    }

    NUMBER_ROW:number;
    NUMBER_ADD(_row:number) {
        this.NUMBER_ROW = _row;
        document.querySelector<HTMLElement>('.NUMBER').style.display = "block";
    }

    NUMBER_CANCEL(){
        //clear
        document.querySelector<HTMLInputElement>('.NUMBER .TEXT .INPUT_QTY').value = "";
        document.querySelector<HTMLElement>('.NUMBER').style.display = "none";
    }



    bindPickingmenber(){
        try{
            this._serviceProxy.lgaEkbEkanbanProgressGetPickingMember()
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                if(result){
                    this.PickingMember = result;
                    this.bindMember(result);
                }
            });
        }catch(e){
            console.log(e);
        }
    }
    bindMember(_data: MstLgaEkbUserDto[] ) {
        _data.forEach(_row => {
            let _mem = document.querySelector<HTMLElement>(".PTA .HeaderTop .title.left .title_column .title_val .LISTCOMBO .memberItem[title='"+_row.id+"']");
            if(_mem) {
                _mem.setAttribute("prodLine",_row.prodLine);
                _mem.setAttribute("userCode",_row.userCode);
                // _mem.setAttribute("userCode",_row.userCode);
            }
        });
    }

    CHANGE_MEMBER(_userCode:string, _prodLine:string) {
        this.PICKING_MEMBER = _userCode;
        this.PROD_LINE = _prodLine;
        window.location.href = "/screens/progressscreen?prodLine="+_prodLine+"&userCode=" + _userCode;
    }
}
