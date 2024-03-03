import { Component, Injector, HostListener, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';

//app
import { finalize, filter } from 'rxjs/operators';
import { CommonFunction } from '@app/main/commonfuncton.component';

//add Service
import { LgaBp2BigPartDDLServiceProxy,
              BigPartTabletAndonOutput,
              BigPartTabletEcarOutput} from '@shared/service-proxies/service-proxies';


@Component({
  selector: 'bigparttablet2',
  templateUrl: './bigparttablet2.component.html',
  styleUrls: ['./bigparttablet2.component.less']
})


export class BigPartTablet2Component extends AppComponentBase {


    rowData: BigPartTabletAndonOutput[] = []
    rowEcar: BigPartTabletEcarOutput[] = []

    isLoading: boolean = false;
    datePipe = new DatePipe('en-US');
    fn: CommonFunction = new CommonFunction();

    PROD_LINE: string = 'A1';
    ECAR_ID: string = '1';
    EcarName: string = 'E-CAR 1';

    //chia 2 column data
    rowCount:number;
    itemSpitCount:number;

    passwordForNextC: string = '';
    inputPasswordForNextC: string = '';

    //data
    Title: string = '';
    ScreenStatus: string;
    ActualVolCount:number;
    PlanVolCount:number;
    TotalTaktTime:number;
    ProgressId:string;
    ActualTaktTime:number;
    ProcessTatkTime:number;
    MaxPartPerPage:number;
    CountDataActual:number =  1;
    IsCallLeader:string;
    countdown : number;
    ItemCount : string;
    TotalItem : string;
    ecarCount: number;

    ButtonName:string;

        constructor(
            injector: Injector,
            private _serviceProxy: LgaBp2BigPartDDLServiceProxy,
        ) {
            super(injector);
            console.log('1: constructor');

        }

        ngOnInit() {
            console.log('2: ngOnInit');
        }

        timecount:number = 0;
        refeshPage: number = 600;
        timersrepeatBindData;
        ngAfterViewInit() {
            console.log('3: ngAfterViewInit');

            // initLayoutData: load đồng bộ dữ liệu
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


        loadtotal:number;
        initLayoutData(){

            this.loadtotal = 0;
            let countload = 1;

            this.getParameter();


            this.ECAR_LOAD();

            this. _serviceProxy.getDataBigPartTabletAndon(this.PROD_LINE, this.ECAR_ID)
                .pipe(finalize(() => {  this.completeLayoutData(countload); }))
                .subscribe((result) => {
                    if(result){

                        this.MaxPartPerPage = result[0].maxPartPerPage;
                        //this.MaxPartPerPage = 6;

                        this.rowCount = result.length;
                        this.rowData =  result;
                        //phân page
                        this.PageCount = Math.ceil(this.rowCount/ this.MaxPartPerPage);
                        //phân column
                        this.itemSpitCount = Math.floor(this.MaxPartPerPage / 2);


                        this.BIND_DATA(result);
                    }
                });

            this.GETPASSWORD();
        }

        completeLayoutData(count:number){
            this.loadtotal+=1;
            if(this.loadtotal==count) {

                console.log("------------- complete DataLayout drawScreen:" + this.loadtotal);

                setTimeout(() => { this.drawScreen(); }, 500);
                this.repeatBindData();

            }
        }

        h_content:number;
        drawScreen(){
            ///////////////////////////design width
            let w = window.innerWidth;
            let w_coll_big = Math.floor((w / 2) * 100) / 100;
            let w_col_small = Math.floor((w_coll_big / 7) * 100) / 100;
            let w_col_smallbtn = Math.floor((w_coll_big / 5) * 100) / 100;

            let e_body = document.querySelector<HTMLElement>('.BIGPART_TABLET');
            e_body.style.width = w + 'px';
            let e_header = e_body.querySelector<HTMLElement>('.Header');
            e_header.style.width = w + 'px';
            let e_content = e_body.querySelectorAll<HTMLElement>('.Content');
            for(let i=0; e_content[i]; i++){ e_content[i].style.width = w + 'px'; }

            let e_bigCol = e_body.querySelectorAll<HTMLElement>('.Header .bigCol');
            for(let i=0; e_bigCol[i]; i++){ e_bigCol[i].style.width = w_coll_big - 1 + 'px'; }

            let e_headcoll = e_body.querySelectorAll<HTMLElement>('.headcoll.first, .headcoll.last');
            for(let i=0; e_headcoll[i]; i++){ e_headcoll[i].style.width = (w_col_small*2) -1 + 'px'; }
            let e_headcollmid = e_body.querySelector<HTMLElement>('.headcoll.mid');
            e_headcollmid.style.width = (w_col_small*3) -1 + 'px';

            let w_headsubtitle = Math.floor((((w_col_small*2) -1) / 3) * 100) / 100;
            let e_headsubtitle = e_body.querySelectorAll<HTMLElement>('.Header .headsubtitle');
            for(let i=0; e_headsubtitle[i]; i++){ e_headsubtitle[i].style.width = (w_headsubtitle*2) -1 + 'px'; }
            let e_headsubvalue = e_body.querySelectorAll<HTMLElement>('.Header .headsubvalue, .Header .headsubvalue .headsubvaluerow');
            for(let i=0; e_headsubvalue[i]; i++){ e_headsubvalue[i].style.width = (w_headsubtitle) -1 + 'px'; }

            //btn
            let e_colbtn = e_body.querySelectorAll<HTMLElement>('.Header .bigCol .col_btn, .Header .bigCol .col_btn .col_btnsub');
            for(let i=0; e_colbtn[i]; i++){ e_colbtn[i].style.width = w_col_smallbtn - 1 + 'px'; }
            let e_btn = e_body.querySelectorAll<HTMLElement>('.Header .bigCol .col_btn .btn');
            for(let i=0; e_btn[i]; i++){ e_btn[i].style.width = w_col_smallbtn - 12 + 'px'; }


            //content
            let e_concol = e_body.querySelectorAll<HTMLElement>('.Content .cont');
            for(let i=0; e_concol[i]; i++){ e_concol[i].style.width = w_coll_big - 1 + 'px'; }
            let e_contHeader = e_body.querySelectorAll<HTMLElement>('.Content .cont .contHeader');
            for(let i=0; e_contHeader[i]; i++){ e_contHeader[i].style.width = w_coll_big - 1 + 'px'; }

            //item
            let e_w_partname = e_body.querySelectorAll<HTMLElement>('.Content .cont .PART_NAME');
            for(let i=0; e_w_partname[i]; i++){ e_w_partname[i].style.width = (w_col_small*3) -1 + 'px'; }
            let e_w_1st2nd = e_body.querySelectorAll<HTMLElement>('.Content .cont .VAL_1ST, .Content .cont .VAL_2ND');
            for(let i=0; e_w_1st2nd[i]; i++){ e_w_1st2nd[i].style.width = (w_col_small*2) -1 + 'px'; }


            ///////////////////////////design height
            let h = window.innerHeight - 1;
            let h_header = Math.floor((h / 6) * 100) / 100;
            this.h_content = h - h_header - 12;
            let h_headersub = Math.floor((h_header / 5) * 100) / 100;


            e_body.style.height = h + 'px';
            e_header.style.height = h_header + 'px';

            for(let i=0; e_content[i]; i++){ e_content[i].style.height = this.h_content + 'px'; }

            for(let i=0; e_bigCol[i]; i++){ e_bigCol[i].style.height = h_header + 'px'; }

            let e_headersubtitle = e_body.querySelectorAll<HTMLElement>('.headcoll > .headtitle, .headcoll > .headtitle .headsubtitle');
            for(let i=0; e_headersubtitle[i]; i++){
                e_headersubtitle[i].style.height = (h_headersub*3) + 'px';
                e_headersubtitle[i].style.lineHeight = (h_headersub*3) + 'px'; }
            let e_headersubvalue = e_body.querySelectorAll<HTMLElement>('.headcoll .headvalue, .headcoll .headvalue .headsubtitle,  .headcoll .headsubvalue');
            for(let i=0; e_headersubvalue[i]; i++){
                e_headersubvalue[i].style.height = (h_headersub*2) + 'px';
                e_headersubvalue[i].style.lineHeight = (h_headersub*2) + 'px'; }

            //dropdownlist
            let e_ecar = e_body.querySelectorAll<HTMLElement>('.BIGPART_TABLET .ECAR, .BIGPART_TABLET .ECAR .ECAR_TITLE');
            for(let i=0; e_ecar[i]; i++){
                e_ecar[i].style.height = (h_headersub*2) + 'px';
                e_ecar[i].style.lineHeight = (h_headersub*2) + 'px';
             }


            let h_headsubvaluerow = Math.floor((( (h_headersub*3) - 1) / 2) * 100) / 100;
            let e_headsubvaluerow = e_body.querySelectorAll<HTMLElement>('.headcoll .headtitle .headsubvaluerow');
            for(let i=0; e_headsubvaluerow[i]; i++){
                e_headsubvaluerow[i].style.height = (h_headsubvaluerow) + 'px';
                e_headsubvaluerow[i].style.lineHeight = (h_headsubvaluerow) + 'px'; }

            //btn
            for(let i=0; e_colbtn[i]; i++){ e_colbtn[i].style.height = h_header + 'px'; }
            let e_btn1 = e_body.querySelectorAll<HTMLElement>('.Header .bigCol .col_btn .btn_1');
            for(let i=0; e_btn1[i]; i++){
                e_btn1[i].style.height = h_header - 20 + 'px';
                // e_btn1[i].style.lineHeight = h_header - 20 + 'px';
             }


            let h_btn2 = Math.floor((h_header / 2) * 100) / 100;
            let e_btn2 = e_body.querySelectorAll<HTMLElement>('.Header .bigCol .col_btn .btn_2');
            for(let i=0; e_btn2[i]; i++){
                e_btn2[i].style.height = h_btn2 - 12 + 'px';
                e_btn2[i].style.lineHeight = h_btn2 - 12+ 'px';
             }
            let e_btnsub2 = e_body.querySelectorAll<HTMLElement>('.Header .bigCol .col_btn .col_btnsub');
             for(let i=0; e_btnsub2[i]; i++){
                e_btnsub2[i].style.height = h_btn2 + 'px';
                e_btnsub2[i].style.lineHeight = h_btn2 + 'px';
             }


            //content
            for(let i=0; e_concol[i]; i++){ e_concol[i].style.height = this.h_content + 'px'; }
            //contHeader
            e_contHeader = e_body.querySelectorAll<HTMLElement>('.Content .cont .contHeader, .Content .cont .contheadercol ');
            for(let i=0; e_contHeader[i]; i++){
                e_contHeader[i].style.height = (h_headersub*2)  -1 + 'px';
                e_contHeader[i].style.lineHeight = (h_headersub*2)  -1 + 'px';
             }

             this.h_content = this.h_content -  (h_headersub*2);
             this.drawItem();
             this.BTN_STATUS();
        }

        PageCount:number;
        drawItem(){

            //phân column
            let rowCount = 5; // default 5 row
            if(this.MaxPartPerPage){
                rowCount = Math.floor(this.MaxPartPerPage/ 2);
            }

            console.log("------------- drawItem:" + this.timecount);
            let h_item = Math.floor((this.h_content / rowCount) * 100) / 100;
            let h_itemsub = Math.floor((h_item / 2) * 100) / 100;
            //contbody
            let e_item = document.querySelectorAll<HTMLElement>('.BIGPART_TABLET .Content .cont .item .contbodycol, .BIGPART_TABLET .Content .cont .item');
            for(let i=0; e_item[i]; i++){
               e_item[i].style.height = h_item + 'px';
               e_item[i].style.lineHeight =h_item + 'px';
            }

            let e_itemsub = document.querySelectorAll<HTMLElement>('.BIGPART_TABLET .Content .cont .item .contbodycol .contbodyval');
            for(let i=0; e_itemsub[i]; i++){
               e_itemsub[i].style.height = h_itemsub + 'px';
               e_itemsub[i].style.lineHeight =h_itemsub + 'px';
            }

            let e_itemsub2 = document.querySelectorAll<HTMLElement>('.BIGPART_TABLET .Content .cont .item .contbodycol .contbodyval.ID_MARK,'
                                                                    +' .BIGPART_TABLET .Content .cont .item .contbodycol .contbodyval.ID_MARK2');
            for(let i=0; e_itemsub2[i]; i++){
                e_itemsub2[i].style.lineHeight =h_itemsub/2 + 'px';
            }

        }

        getParameter() {
            let urlParams = new URLSearchParams(window.location.search);
            this.PROD_LINE = urlParams.get('prod_line');
            this.ECAR_ID = urlParams.get('ecar_id');
        }

        ngOnDestroy(): void{
            clearTimeout(this.timersrepeatBindData);
        }

        repeatBindData() {

            this.timersrepeatBindData = setInterval(() =>{
                if (this.timecount > this.refeshPage) window.location.reload();
                this.timecount = this.timecount + 1;

                try{
                     this.LOAD_DATA();
                }catch(ex){ console.error(ex);  }

            },2000);

        }


         LOAD_DATA() {

            this. _serviceProxy.getDataBigPartTabletAndon(this.PROD_LINE, this.ECAR_ID)
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                        if (this.timecount < 2) {  this.drawScreen(); }

                        this.BIND_DATA(result);

                        this.ItemCount = result[0].itemCount.toString();
                        this.TotalItem = result[0].totalItem.toString();
                    }
                });
         }

        BIND_DATA(_data:BigPartTabletAndonOutput[]){

            if(_data.length > 0){
                this.Title = _data[0].title;
                this.ScreenStatus = _data[0].screenStatus;
                this.SCREENSTATUS(_data);

                this.ActualVolCount = _data[0].actualVolCount;
                this.PlanVolCount = _data[0].planVolCount;
                this.TotalTaktTime = _data[0].totalTaktTime;
                // this.EcarName = _data[0].ecarName;
                this.ProgressId = _data[0].progressId;

                this.ActualTaktTime = _data[0].actualTaktTime;
                this.ProcessTatkTime = _data[0].processTatkTime;
                this.IsCallLeader = _data[0].isCallLeader;
                this.ecarCount = _data[0].ecarCount;

                let btncall = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_1.CALL_LEADER');
                btncall.classList.remove('IS_CALL_LEADER_N' , 'IS_CALL_LEADER_Y')
                btncall.classList.add('IS_CALL_LEADER_' + this.fn.isActive(this.IsCallLeader));


                let btnStart = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_1.START_PK');

                if(_data[0].buttonName.indexOf("WAIT") > 0 || _data[0].buttonName.indexOf("NOT APPLY") > 0){
                    if (!btnStart.classList.contains('DISABLE')){
                        btnStart.classList.add('DISABLE');
                    }
                }else{
                    btnStart.classList.remove('DISABLE');
                }
                this.ButtonName = _data[0].buttonName;

                this.BTN_STATUS();

            }

            _data.forEach(d => {
                let el = document.querySelector<HTMLElement>('.BIGPART_TABLET .Content .contBody .item.PARTID_' + d.partId);
                if(el) {

                    el.querySelector('.PART_NAME').textContent = d.partName;
                    el.querySelector('.PICK_ADDRESS .pka').textContent = d.pikAddress;
                    el.querySelector('.PICK_ADDRESS .ref').textContent = d.refNo;
                    el.querySelector('.ID_MARK').textContent = d.remark;
                    el.querySelector('.PICK_ADDRESS2 .pka').textContent = d.pikAddress2;
                    el.querySelector('.PICK_ADDRESS2 .ref').textContent = d.refNo2;
                    el.querySelector('.ID_MARK2').textContent = d.remark2;

                    el.classList.remove('PK', 'DEL', 'STARTED', 'FINISHED', 'DEFAULT' );
                    el.classList.add(d.processType);
                    el.classList.add(this.fn.isStatus(d.status, 'DEFAULT'));

                }


                var backgroundStatus = document.querySelectorAll<HTMLElement>('.BIGPART_TABLET .Content .contBody');
                if(backgroundStatus) {
                    for(let i=0; backgroundStatus[i]; i++){
                    var color = (d.status == 'FINISHED') ? '#e6fffa' :  ((d.status == 'STARTED') ? '#e2efda' : '#ffffff');
                    backgroundStatus[i].style.background = color;
                    }
                }
            });

            let progressCountDownt = document.querySelector<HTMLElement>('.progress-bar');
            progressCountDownt.style.width = 100 / this.TotalTaktTime * (this.ActualTaktTime <= this.TotalTaktTime ? this.ActualTaktTime :this.TotalTaktTime)+'%';
            this.countdown = this.TotalTaktTime - this.ActualTaktTime;
            this.countdown = (this.countdown < 0 ? 0 : this.countdown );


        }


        BTN_STATUS(){

            if(this.rowData) {

                let e_content = document.querySelector<HTMLElement>('.Content.active');

                let  _page = parseInt(e_content != null ? e_content.getAttribute("title") : "1");


                //check next btn
                //console.log('_page: ' + _page + ', PageCount:' + this.PageCount);

                let btnnext = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_2.next');
                if (this.PageCount == _page){
                    if (!btnnext.classList.contains('DISABLE')){
                        btnnext.classList.add('DISABLE');
                    }
                } else{
                    btnnext.classList.remove('DISABLE');
                }

                //check back btn
                let btnback = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_2.back');
                if (_page == 1){
                    if (!btnback.classList.contains('DISABLE')){
                        btnback.classList.add('DISABLE');
                        this.BTN_BACK();
                    }
                }else{
                    btnback.classList.remove('DISABLE');
                }

                //check finish btn
                let btnfinish = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_1.START_PK');
                var btnName = this.ButtonName.substring(0,6);
                if ((this.CountDataActual*this.MaxPartPerPage < this.rowCount) && btnName == 'FINISH'){
                    if (!btnfinish.classList.contains('DISABLE')){
                        btnfinish.classList.add('DISABLE');
                    }
                }else{
                   // var color = (btnName == 'FINISH') ? '#e6fffa' : '#e2efda';
                   // btnfinish.style.background = color;
                    btnfinish.classList.remove('DISABLE');
                }
            }

        }

        BTN_NEXT(){
            //check disable  - phuongdv
            let btnnext = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_2.next');
            if (btnnext.classList.contains('DISABLE')){
                return;
            }


            let e_content = document.querySelector<HTMLElement>('.Content.active');
            if(e_content){
                let _page = parseInt(e_content.getAttribute("title"));
                _page = _page + 1;
                this.CountDataActual = _page;
                let econt = document.querySelector<HTMLElement>('.Content.PAGE' + _page);
                if(econt){
                    e_content.classList.remove("active");
                    econt.classList.add("active");
                }

                this.BTN_STATUS();
            }
        }

        BACKPAGE1(){

            let e_content = document.querySelector<HTMLElement>('.Content.active');
            if(e_content){
                this.CountDataActual = 1;
                e_content.classList.remove("active");
            }
            let econt = document.querySelector<HTMLElement>('.Content.PAGE1');
            if(econt){
                econt.classList.add("active");
            }

            this.BTN_STATUS();
        }

        BTN_BACK(){
            //check disable
            let btnback = document.querySelector<HTMLElement>('.BIGPART_TABLET .col_btn .btn_2.back');
            if (btnback.classList.contains('DISABLE')){
                return;
            }

            let e_content = document.querySelector<HTMLElement>('.Content.active');
            if(e_content){
                let _page = parseInt(e_content.getAttribute("title"));
                _page = _page - 1;
                this.CountDataActual = _page;
                let econt = document.querySelector<HTMLElement>('.Content.PAGE' + _page);
                if(econt){
                    e_content.classList.remove("active");
                    econt.classList.add("active");
                }

                this.BTN_STATUS();

            }
        }

        dataPage() {
            return this.fornumbers(this.PageCount);
        }
        dataleft(_page:number){
            // console.log('----page:' + _page + '  left');
            if(this.MaxPartPerPage){
                //let maxitemnumber = this.MaxPartPerPage *_page;
                let minitemnumber = this.MaxPartPerPage *(_page-1);
                let leftcount = (minitemnumber + this.itemSpitCount);
                leftcount = (leftcount <= this.rowCount) ? leftcount:this.rowCount;

                let numRangeColLeft: BigPartTabletAndonOutput[] = [];
                for(let i = minitemnumber; i< leftcount; i++) {
                    numRangeColLeft.push(this.rowData[i]);
                    // console.log('--------pick: ' + this.rowData[i].pikAddress);
                }
                return numRangeColLeft;
            }
        }
        dataright(_page:number){
            // console.log('----page:' + _page + '  right');
            let maxitemnumber = this.MaxPartPerPage *_page;
            //let minitemnumber = this.MaxPartPerPage *(_page-1);
            let rightcount = (maxitemnumber - this.itemSpitCount);
            rightcount = (rightcount <= this.rowCount) ? rightcount:this.rowCount;
            maxitemnumber = (maxitemnumber <= this.rowCount) ? maxitemnumber:this.rowCount;

            let numRangeColRight: BigPartTabletAndonOutput[] = [];
            for (let i = rightcount; i < maxitemnumber; i++) {
                numRangeColRight.push(this.rowData[i]);
                // console.log('--------pick: ' + this.rowData[i].pikAddress);
            }
            return numRangeColRight;
        }
        START_FINISH(){
            this. _serviceProxy.bigPartTabletAndon_START_FINISH(this.ProgressId)
            .pipe(finalize(() => {   }))
            .subscribe((result) => {
                //alert("start success! progressid=" + this.ProgressId );
                this.BACKPAGE1();
            });
        }

        CALL_LEADER(){
            this. _serviceProxy.bigPartTabletAndon_CALL_LEADER(this.ProgressId)
            .pipe(finalize(() => {   }))
            .subscribe((result) => {
                alert("call leader success! progressid=" + this.ProgressId );
            });
        }

        UNDO(){
            this. _serviceProxy.bigPartTabletAndon_UNDO(this.ProgressId)
            .pipe(finalize(() => {   }))
            .subscribe((result) => {
                if(result){
                    alert("undo success! progressid=" + this.ProgressId );
                }
            });
        }


        ECAR_LOAD(){
            this. _serviceProxy.getDataBigPartEcarAndon()
                .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                        this.rowEcar = result;
                        let _rowitem =  result.filter(a => a.prodLine == this.PROD_LINE && a.id.toString() == this.ECAR_ID);
                        if(_rowitem.length > 0){
                            this.EcarName = _rowitem[0].ecarName;
                        }
                    }
                });
        }
        ECAR_CHANGE(id:number, prodLine:string, ecarName:string){
            this.PROD_LINE = prodLine;
            this.ECAR_ID = id.toString();
            /*this.EcarName = ecarName;
            this.ECAR_LOAD();*/
            // let ecar = document.querySelector<HTMLElement>('.BIGPART_TABLET .ECAR .ECAR_BODY');
            // if (ecar){ ecar.style.display = 'none'; }
            let url = "/screens/bigparttablet2?prod_line="+this.PROD_LINE+"&ecar_id="+this.ECAR_ID+"";
            document.location.href = url;
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

        SCREENSTATUS(data){
            //Status Screen
            var screenStatus = document.getElementById('screen-status');
            if (data[0].screenStatus == 'STOPED') {
                screenStatus.innerHTML = data[0].screenStatus.replace('ED', '');
                screenStatus.style.display = 'block';
            } else if (data[0].screenStatus == 'PAUSED') {
                screenStatus.innerHTML = data[0].screenStatus.replace('D', '');
                screenStatus.style.display = 'block';
            } else {
                screenStatus.style.display = 'none';
            }
        }

        GETPASSWORD(){
            this._serviceProxy.getItemValue("LGA_BP2_DIRECT_SUPPLY_TABLET_SCREEN", "PASSWORD_FOR_NEXTC_" + this.PROD_LINE)
            .pipe(finalize(() => {   }))
                .subscribe((result) => {
                    if(result){
                        if(result) this.passwordForNextC = result.itemValue;
                    }
                });
        }



        NEXTC_CONFIRM_PASSWORD() {
            this.OpenConfirmPassword();
            //this.NEXTC();
        }

        NEXTC(){
            this. _serviceProxy.bigPartTabletAndon_NEXTC(this.PROD_LINE, this.ECAR_ID)
            .pipe(finalize(() => {   }))
            .subscribe((result) => {
                if(result){
                    alert("nextc success! progressid=" + this.PROD_LINE + this.ECAR_ID );
                    this.CloseConfirmPassword();
                }
            });
        }

        ConfirmPassword(){
            var _inputPasswordForNextC = document.querySelector<HTMLInputElement>('.BIGPART_TABLET #inputPasswordForNextC');
            if(!_inputPasswordForNextC) return;

            this.inputPasswordForNextC = _inputPasswordForNextC.value;

            if(this.inputPasswordForNextC.trim() == "") {
                alert("Password is not blank!");
                var _popupConfirmPassword = document.querySelector<HTMLElement>('.BIGPART_TABLET #popupConfirmPassword');
                if(_popupConfirmPassword){
                    _popupConfirmPassword.focus();
                    return;
                }
            }
            else if (this.inputPasswordForNextC != this.passwordForNextC){
                alert("Password is Wrong!");
                var _popupConfirmPassword = document.querySelector<HTMLElement>('.BIGPART_TABLET #popupConfirmPassword');
                if(_popupConfirmPassword){
                    _popupConfirmPassword.focus();
                    return;
                }
            }
            else if(this.inputPasswordForNextC == this.passwordForNextC ) {
                //alert("OK :" + this.passwordForNextC);
                this.NEXTC();
            }
        }

        OpenConfirmPassword(){
            var _popupConfirmPassword = document.querySelector<HTMLElement>('.BIGPART_TABLET #popupConfirmPassword');
            var _inputPasswordForNextC = document.querySelector<HTMLInputElement>('.BIGPART_TABLET #inputPasswordForNextC');
            if(_popupConfirmPassword){
                _popupConfirmPassword.style.display = "flex";
                this.inputPasswordForNextC = "";
                if(_inputPasswordForNextC) _inputPasswordForNextC.value = "";
            }
        }

        CloseConfirmPassword(){

            var _popupConfirmPassword = document.querySelector<HTMLElement>('.BIGPART_TABLET #popupConfirmPassword');
            var _inputPasswordForNextC = document.querySelector<HTMLInputElement>('.BIGPART_TABLET #inputPasswordForNextC');
            if(_popupConfirmPassword){
                _popupConfirmPassword.style.display = "none";
                this.inputPasswordForNextC = "";
                if(_inputPasswordForNextC) _inputPasswordForNextC.value = "";
            }
        }

}
