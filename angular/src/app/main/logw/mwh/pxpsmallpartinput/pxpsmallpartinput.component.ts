
import { style } from '@angular/animations';
import { element } from 'protractor';
import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

//add Service
import { LgwMwhPxpModuleInputAndonServiceProxy,
    GetPxpModuleInputAndonLayoutOutput,
    GetPxpModuleInputAndonDataOutput,
    GetPxpModuleCaseNoLocationOutput,
    GetPxpModuleContRenbanOutput } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { PopupSmallComponent } from './popup-small.component';
import { ModuleFunctionComponent } from '../pxpsmallpartinput/modulefunction.component';

//form popup
import { PopuprobbingModalComponent } from './popuprobbing-modal.component';


@Component({
    templateUrl: './pxpsmallpartinput.component.html',
    styleUrls: [
        './pxpsmallpartinput.component.less',
    ],
})

export class PxpsmallpartinputComponent extends AppComponentBase {

    @ViewChild('popuprobbingModal', { static: true }) popuprobbingModal:| PopuprobbingModalComponent| undefined;
    @ViewChild('popupSmall', { static: true }) popupSmall:| PopupSmallComponent| undefined;

    isLoading: boolean = false;
    datePipe = new DatePipe('en-US');
    TabActive:String;

    fn: ModuleFunctionComponent = new ModuleFunctionComponent();

    TabLayout11: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout12: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout21: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout22: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout3: GetPxpModuleInputAndonLayoutOutput[] = [];
    NoLocation: GetPxpModuleCaseNoLocationOutput[] = [];

    loadtab:number;
    wFullBar: number;
    DETECT_SELECTED:string = '';

    constructor(
        injector: Injector,
        private _serviceProxy: LgwMwhPxpModuleInputAndonServiceProxy,
    ) {
        super(injector);
        console.log('1: constructor');

    }

    ngOnInit(): void {
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


    initLayoutData(){
                this.isLoading = true;
                this.loadtab = 0;
                let counttab = 4;

                console.log("getPxpModuleInputAndonLayout('SMALL', '4')");
                this. _serviceProxy.getPxpModuleInputAndonLayout('SMALL', '4')
                .pipe(finalize(() => { this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        this.TabLayout11 =  result.filter(a=> a.screenArea == '1');
                        this.TabLayout12 =  result.filter(a=> a.screenArea == '2');
                    }
                });

                console.log("getPxpModuleInputAndonLayout('SMALL', '5')");
                this. _serviceProxy.getPxpModuleInputAndonLayout('SMALL', '5')
                .pipe(finalize(() => { this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        this.TabLayout21 =  result.filter(a=> a.screenArea == '1');
                        this.TabLayout22 =  result.filter(a=> a.screenArea == '2');
                    }
                });

                console.log("getPxpModuleInputAndonLayout('RB', '6')");
                this. _serviceProxy.getPxpModuleInputAndonLayout('RB', '6')
                .pipe(finalize(() => { console.log('pipe?'); this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        console.log('subscribe?');
                        this.TabLayout3 =  result;
                    }
                });

                this. _serviceProxy.getPxpModuleCaseNoLocation()
                .pipe(finalize(() => {  this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        this.NoLocation =  result;
                    }
                });
    }

    completeLayoutData(count:number){
        this.loadtab+=1;
        if(this.loadtab==count) {
            console.log("------------- drawScreen, first repeatBindData:" + this.loadtab);

            // bắt đầu request data
            this.repeatBindData();

            setTimeout(() => { this.drawScreen(); }, 500);
        }
    }

    ngOnDestroy(): void{
        clearTimeout(this.timerstorage);
        // this.timerstorage = null;
    }
    timecount:number = 0;
    refeshPage: number = 600;
    timerstorage;
    secondDelay = 3000;
    repeatBindData(){

        if (this.timecount > this.refeshPage) window.location.reload();
        this.timecount = this.timecount + 1;

        try{

            this.LOAD_TAB();
            this.fn.showtime('time_now_log');

        }catch(ex){
            console.log(ex);

            this.timerstorage = setTimeout(() => {
                this.repeatBindData();
            }, this.secondDelay);
        }


    }

    LOAD_TAB(){     // phuongdv

        // let _select = document.querySelectorAll<HTMLElement>('.boxCellText.selected.exists_data');
        // for(let i=0; _select[i]; i++){ _select[i].classList.remove('selected'); }

        switch (this.TabActive) {
            case '1':
                this. _serviceProxy.getPxpModuleInputAndonData('SMALL', '4')
                    .subscribe((result) => {
                        if(result){
                            //console.log('BIND_ROBBING');
                            if(this.TabActive == '1'){
                                this.BIND_TAB_AREA(result,this.TabActive);
                            }
                            this.timerstorage = setTimeout(()=>{
                                this.repeatBindData();
                            }, this.secondDelay);
                        }
                    },(error) => {
                        console.log(error);
                        this.timerstorage = setTimeout(()=>{
                            this.repeatBindData();
                        }, this.secondDelay);
                    });

                break;
            case '2':
                    this. _serviceProxy.getPxpModuleInputAndonData('SMALL', '5')
                    .subscribe((result) => {
                        if(result){
                            //console.log('BIND_ROBBING');
                            if(this.TabActive == '2'){
                                this.BIND_TAB_AREA(result,this.TabActive);
                            }
                            this.timerstorage = setTimeout(()=>{
                                this.repeatBindData();
                            }, this.secondDelay);
                        }
                    },(error) => {
                        console.log(error);
                        this.timerstorage = setTimeout(()=>{
                            this.repeatBindData();
                        }, this.secondDelay);
                    });
                break;
            case '3':
                    this. _serviceProxy.getPxpModuleInputAndonData('RB', '6')
                    .subscribe((result) => {
                        if(result){
                            //console.log('BIND_ROBBING');
                            if(this.TabActive == '3'){
                                this.BIND_ROBBING(result);
                            }
                            this.timerstorage = setTimeout(()=>{
                                this.repeatBindData();
                            }, this.secondDelay);
                        }
                    },(error) => {
                        console.log(error);
                        this.timerstorage = setTimeout(()=>{
                            this.repeatBindData();
                        }, this.secondDelay);
                    });

                    this. _serviceProxy.getPxpModuleContRenban('RB', '6')
                    .subscribe((result) => {
                        if(result){
                            if(this.TabActive == '3'){
                                this.BIND_ROBBING_RENBAN(result);
                            }
                            //console.log('BIND_ROBBING');
                        }
                    });
                break;
            default:
                this.TabActive = '3';
                this.LOAD_TAB();
                break;
        }

            this. _serviceProxy.getPxpModuleCaseNoLocation()
            .subscribe((result) => {
                if(result){
                    this.NoLocation =  result;
                }
            });
    }

    BIND_TAB_AREA(_data: GetPxpModuleInputAndonDataOutput[], p_tab:String){

        let _d = document.querySelectorAll<HTMLElement>('._tab'+p_tab+' .SCREEN_AREA .cellData .boxCellText'); //
        for(let i=0; _d[i]; i++){
            _d[i].textContent = '';
            _d[i].classList.remove('exists_data');
            // _d[i].setAttribute("renban", '');
            _d[i].setAttribute("supplierNo", '');
            _d[i].setAttribute("caseNo", '');
            _d[i].setAttribute("locId", '');
        }


        let eitem =  document.querySelectorAll<HTMLElement>('._tab'+p_tab+' .SCREEN_AREA .cellData');
        let wItem = 0;
        let wItem2 = 0;
        if(eitem){
            wItem = eitem[0].clientWidth ;
            wItem2 = Math.floor((wItem/ 2) * 100) / 100;
        }

        _data.forEach(d => {
            let e = document.querySelector('._tab'+p_tab+' .SCREEN_AREA .cellData#SCR_NO'+p_tab + d.screenArea +'_Column'+d.columnId+'_Row'+ (d.rowId+1) + ' .boxCellText');
            if(e){
                e.textContent = d.caseNo;
                e.classList.add('exists_data');
                // e.setAttribute("renban", d.renban);
                e.setAttribute("supplierNo", d.supplierNo);
                e.setAttribute("caseNo", d.caseNo);
                e.setAttribute("locId", d.locId.toString());
            }


            let ItemCelldata =  document.querySelector('._tab'+p_tab+' .SCREEN_AREA .cellData#SCR_NO'+p_tab + d.screenArea +'_Column'+d.columnId+'_Row'+ (d.rowId+1));
            if(d.moduleSize == "S"){

                ItemCelldata.querySelector<HTMLElement>('.box_top').style.borderRight = '1px solid black';
                let boxsub = ItemCelldata.querySelectorAll<HTMLElement>('.box_sub');
                for(let i=0; boxsub[i]; i++){ boxsub[i].style.width =  (wItem2) + 'px'; }
                let itemcolle =  ItemCelldata.querySelectorAll<HTMLElement>('.flag, .boxCellText');
                for(let i=0; itemcolle[i]; i++){ itemcolle[i].style.width =  (wItem2 - 1) + 'px'; }

            }
            else {

                ItemCelldata.querySelector<HTMLElement>('.box_top').style.borderRight = 'none';
                let itemcolle =  ItemCelldata.querySelectorAll<HTMLElement>('.box_sub, .flag, .boxCellText');
                for(let i=0; itemcolle[i]; i++){ itemcolle[i].style.width =  (wItem) + 'px'; }

            }

        });
    }



    BIND_ROBBING(_data: GetPxpModuleInputAndonDataOutput[]){

        let _d = document.querySelectorAll<HTMLElement>('._tab3 .SCREEN_AREA .cellData.robbing .boxCellText'); //
        for(let i=0; _d[i]; i++){
            _d[i].textContent = '';
            _d[i].classList.remove('exists_data');
            _d[i].setAttribute("renban", '');
            _d[i].setAttribute("supplierNo", '');
            _d[i].setAttribute("caseNo", '');
        }

        _data.forEach(d => {
            let e = document.querySelector('._tab3 .SCREEN_AREA .cellData.robbing#SCR_NO31_Column'+d.columnId+'_Row'+ d.rowId + ' .boxCellText');
            if(e){
                e.textContent = d.caseNo;
                e.classList.add('exists_data');
                e.setAttribute("renban", d.renban);
                e.setAttribute("supplierNo", d.supplierNo);
                e.setAttribute("caseNo", d.caseNo);
            }
        });
    }

    BIND_ROBBING_RENBAN(_data: GetPxpModuleContRenbanOutput[]){

        let _d = document.querySelectorAll<HTMLElement>('._tab3 .SCREEN_AREA .cellData.renban_cont .boxCellText'); //
        for(let i=0; _d[i]; i++){
            _d[i].textContent = '';
            _d[i].classList.remove('exists_data');
            _d[i].setAttribute("renban", '');
            _d[i].setAttribute("supplierNo", '');
            _d[i].setAttribute("columnId", '');
        }

        _data.forEach(d => {
            let e = document.querySelector('._tab3 .SCREEN_AREA .cellData.renban_cont#SCR_NO31_RENBAN' + d.columnId + ' .boxCellText');
            if(e){
                e.textContent = d.renban;
                e.classList.add('exists_data');
                e.setAttribute("renban", d.renban);
                e.setAttribute("supplierNo", d.supplierNo);
                e.setAttribute("columnId", (d.columnId + 1).toString());
            }
        });

        let _dV = document.querySelectorAll<HTMLElement>('._tab3 .SCREEN_AREA .cellData.DEV_DATE .boxCellText'); //
        for(let i=0; _dV[i]; i++){
            _dV[i].textContent = '';
            _dV[i].classList.remove('exists_data');
        }

        _data.forEach(d => {
            let e = document.querySelector('._tab3 .SCREEN_AREA .cellData.DEV_DATE#SCR_NO31_DEV_DATE' + d.columnId + ' .boxCellText');
            if(e){
                e.textContent = d.devanningDate.day + '-' + d.devanningDate.monthShort;
                e.classList.add('exists_data');
            }
        });

    }


    drawScreen(){
        ///////////////////////////design width
        let w = window.innerWidth - 36;

        let tabcontent = document.querySelectorAll<HTMLElement>('.pxp_big_content .content_tab .SCREEN_AREA');
        for(let i=0; tabcontent[i]; i++){ tabcontent[i].style.width = w + 'px'; }

        w = w + 10;
        //Tab1 SCR_NO11
        let colcount =  this.fn.getRow(this.TabLayout11,0).length + 1;
        let colcount2 =  this.fn.getRow(this.TabLayout12,0).length + 1;
        colcount = (colcount > colcount2) ? colcount : colcount2;
        colcount = (colcount<=5) ? 5: colcount;
        this.fn.drawWidthTab('SCR_NO11', w, colcount);

        //Tab1 SCR_NO12
        this.fn.drawWidthTab('SCR_NO12', w, colcount);

        // Tab2 SCR_NO21
        colcount =  this.fn.getRow(this.TabLayout21,0).length + 1;
        colcount2 =  this.fn.getRow(this.TabLayout22,0).length + 1;
        colcount = (colcount > colcount2) ? colcount : colcount2;
        colcount = (colcount<=5) ? 5: colcount;
        this.fn.drawWidthTab('SCR_NO21', w, colcount);
        //Tab1 SCR_NO22
        this.fn.drawWidthTab('SCR_NO22', w, colcount);

        //Tab3 Robbing SCR_NO31
        colcount =  this.fn.getRow(this.TabLayout3,0).length + 1;
        colcount = (colcount<=10) ? 10: colcount;
        this.fn.drawWidthTab('SCR_NO31', w, colcount);

        ///////////////////////////design height

        let hcontent = this.fn.drawHeightContent() - 10;
        let hcontent2 = Math.floor((hcontent / 2) * 100) / 100;

        // ------------------
        //tab1 SCR_NO11
        let rowcount = this.fn.getRowCount(this.TabLayout11) + 1;
        rowcount = (rowcount<=5) ? 5: rowcount;
        this.fn.drawHeightTab('SCR_NO11', hcontent2, rowcount);

        //  tab1 SCR_NO12
        rowcount = this.fn.getRowCount(this.TabLayout12) + 1;
        rowcount = (rowcount<=5) ? 5: rowcount;
        this.fn.drawHeightTab('SCR_NO12', hcontent2, rowcount);

        //  tab2 SCR_NO21
        rowcount = this.fn.getRowCount(this.TabLayout21) + 1;
        rowcount = (rowcount<=5) ? 5: rowcount;
        this.fn.drawHeightTab('SCR_NO21', hcontent2, rowcount);

        //  tab2 SCR_NO22
        rowcount = this.fn.getRowCount(this.TabLayout22) + 1;
        rowcount = (rowcount<=5) ? 5: rowcount;
        this.fn.drawHeightTab('SCR_NO22', hcontent2, rowcount);

        //tab3 SCR_NO31 ROBBING
        this.fn.drawHeightTab('SCR_NO31', hcontent + 10, this.fn.getRowCount(this.TabLayout3) + 3);

    }

    changetab(tab:String){

        this.fn.changetab(tab);
        this.TabActive = tab; //1,2,3
        // this.ngOnDestroy();
        // if(!this.timerstorage){
        //     this.timerstorage = setTimeout(()=>{
        //         alert(this.timerstorage)
        //         this.repeatBindData();
        //     }, 10);
        // }
    }

    ShowChooseCase(){

    }



    MOVE_UOT_DELETE(){
        try{
            switch (this.DETECT_SELECTED) {
                case 'TAB_CASE_SELECTED':
                    let _selected = document.querySelector('._tab'+ this.TabActive + ' .SCREEN_AREA .cellData .boxCellText.selected');
                    console.log('._tab'+ this.TabActive + '.SCREEN_AREA .cellData .boxCellText.selected');

                    if(_selected) {
                        let _caseno = _selected.getAttribute('caseNo');
                        let _supplierNo = _selected.getAttribute("supplierNo");
                        let _locId = _selected.getAttribute("locId");

                        this._serviceProxy.pxpModuleInputCaseDelete(_caseno, _supplierNo, _locId, null,'')
                        .subscribe((result) => {
                            if(result){
                                alert("EXEC LGW_MWH_CASE_DATA_DELETE  " + _caseno + ", " + _supplierNo + ", " + _locId + ", null, ''  ----success!");
                                _selected.classList.remove('selected');
                            }
                        });
                    }
                    break;
                case 'TAB3_ROBBING_RENBAN_SELECTED':

                        let eRenban = document.querySelector('.SCREEN_AREA .cellData.renban_cont .boxCellText.selected');
                        if(eRenban){
                            let _renban = eRenban.getAttribute("renban");
                            let _supplierNo = eRenban.getAttribute("supplierNo");
                            let _columnId = eRenban.getAttribute("columnId");

                            this. _serviceProxy.pxpModuleInputRobbingMoveIn(_renban,_supplierNo, _columnId)
                            .subscribe((result) => {
                                if(result){
                                    alert('LGW_MWH_CASE_DATA_ROBBING_MOVE_IN ' + _renban + ', ' + _supplierNo + ', ' + _columnId);
                                    let _d = document.querySelector('.SCREEN_AREA .cellData.renban_cont#SCR_NO31_RENBAN' + _columnId + ' .boxCellText'); //
                                    if(_d) {  _d.classList.remove('selected');  }
                                }
                            });
                        }

                    break;
                case 'TAB3_ROBBING_CASENO_SELECTED':
                    let eCaseno = document.querySelector('.SCREEN_AREA .cellData.robbing .boxCellText.selected');
                    if(eCaseno){
                        let _renban = eCaseno.getAttribute("renban");
                        let _supplierNo =eCaseno.getAttribute("supplierNo");
                        let _caseNo =eCaseno.getAttribute("caseNo");

                        this. _serviceProxy.pxpModuleInputRobbingMoveOutDelete(_caseNo, _renban,_supplierNo)
                        .subscribe((result) => {
                            if(result){
                                alert('LGW_MWH_CASE_DATA_ROBBING_CASE_DELETE ' + _caseNo + ', ' + _renban + ', ' + _supplierNo);
                                let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData.robbing .boxCellText'); //
                                for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
                            }

                        });
                    }

                    break;
                default:

                    break;
            }

         }catch(ex){ alert(ex);  }

    }

    MOVE_UP() {
        try{
            switch (this.DETECT_SELECTED) {
                case 'TAB_CASE_SELECTED':
                    let _selected = document.querySelector('._tab'+ this.TabActive + ' .SCREEN_AREA .cellData .boxCellText.selected');
                    if(_selected) {
                        let _caseno = _selected.getAttribute('caseNo');
                        let _supplierNo = _selected.getAttribute("supplierNo");
                        let _locId = _selected.getAttribute("locId");

                        this._serviceProxy.pxpModuleInputCaseMoveUp(_caseno, _supplierNo, null,'')
                        .subscribe((result) => {
                            if(result){
                                alert("EXEC LGW_MWH_CASE_DATA_MOVE_OUT  " + _caseno + ", " + _supplierNo + ", null, ''  ----success!");
                                _selected.classList.remove('selected');
                            }
                        });
                    }
                    break;
                case 'TAB3_ROBBING_CASENO_SELECTED':

                    let eCaseno = document.querySelector('.SCREEN_AREA .cellData.robbing .boxCellText.selected');
                    if(eCaseno){
                        let _renban = eCaseno.getAttribute("renban");
                        let _supplierNo =eCaseno.getAttribute("supplierNo");
                        let _caseNo =eCaseno.getAttribute("caseNo");

                        this. _serviceProxy.pxpModuleInputRobbingCaseMoveUP(_caseNo, _renban,_supplierNo)
                        .subscribe((result) => {
                            if(result){
                                alert('LGW_MWH_CASE_DATA_ROBBING_CASE_MOVEUP ' + _caseNo + ', ' + _renban + ', ' + _supplierNo);
                                let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData.robbing .boxCellText'); //
                                for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
                            }

                        });
                    }


                    break;
                default:
                    break;
            }
        }catch(ex){ alert(ex);  }

    }

    CASE_SELECTED(tab:string, locationId :number, cellName: string, rowid:number, columnId:number, screenArea:string){

        let e = document.querySelector('._tab'+ tab +' .SCREEN_AREA .cellData#SCR_NO'+tab+screenArea+'_Column' + columnId + '_Row' + (rowid) + ' .boxCellText');
        console.log('._tab'+ tab +' .SCREEN_AREA .cellData#SCR_NO'+tab+screenArea+'_Column' + columnId + '_Row' + (rowid) + ' .boxCellText');
        if(!e.classList.contains('exists_data')){
            console.log('popupRobbing.show()');
            this.popupSmall.show(locationId, cellName, rowid);
        }
        else {
            let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData .boxCellText'); //
            for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
            e.classList.add('selected');
            this.DETECT_SELECTED = 'TAB_CASE_SELECTED';
        }
    }

    ROBBING_RENBAN_SELECTED(type:String, columnId: number, cellName: string){

        let e = document.querySelector('.SCREEN_AREA .cellData.renban_cont#SCR_NO31_RENBAN' + columnId + ' .boxCellText');
        if(!e.classList.contains('exists_data')){
            console.log('popupRobbing.show()');
            this.popuprobbingModal.show(columnId, cellName, 0);
        }
        else {
            let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData .boxCellText'); //
            for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
            e.classList.add('selected');
            this.DETECT_SELECTED = 'TAB3_ROBBING_RENBAN_SELECTED';
        }
    }

    ROBBING_CASENO_SELECTED(type:String, columnId: number, cellName: string, _Row:number){

        let e = document.querySelector('.SCREEN_AREA .cellData.robbing#SCR_NO31_Column' + columnId + '_Row' + _Row + ' .boxCellText');

        if(e.classList.contains('exists_data')){
            let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData .boxCellText'); //
            for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
            e.classList.add('selected');
            this.DETECT_SELECTED = 'TAB3_ROBBING_CASENO_SELECTED';
        }
    }

    NO_LOCATION() {
        let nolocation = document.querySelector<HTMLElement>('.NO_LOCATION'); //
        if(nolocation.classList.contains('show')){
            nolocation.classList.remove('show');
        }else{
            nolocation.classList.add('show');
        }
    }

    ChooseLocation(type:String, locationId :number, cellName: string, rowid:number){
        this.popupSmall.show(locationId, cellName, rowid);
    }


    ClosePopupRobbing() {
        console.log('Move In:  Close');
        this.LOAD_TAB();
    }
    SavePopupRobbing() {
        console.log('Save: MOVE IN');
    }

    ClosePopupSmall() {
        console.log('Move In:  Close');
        this.LOAD_TAB();
    }
    SavePopupSmall() {
        console.log('Save: MOVE IN');
    }
}
