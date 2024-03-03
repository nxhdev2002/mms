import { DatePipe } from '@angular/common';
import { Component, Injector, HostListener, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';

//add Service
import { LgwMwhPxpModuleInputAndonServiceProxy,
             GetPxpModuleInputAndonLayoutOutput,
             GetPxpModuleInputAndonDataOutput,
             GetPxpModuleCaseListOutput,
             GetPxpModuleCaseNoLocationOutput } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { PopupBigComponent } from './popup-big.component';
import { ModuleFunctionComponent } from '../pxpsmallpartinput/modulefunction.component';

//form popup

@Component({
    templateUrl: './pxpbigpartinput.component.html',
    styleUrls: [
        '../pxpsmallpartinput/pxpsmallpartinput.component.less',
    ],
})

export class PxpbigpartinputComponent extends AppComponentBase {

    @ViewChild('bigPopup', { static: true }) bigPopup!: PopupBigComponent;

    isLoading: boolean = false;
    pipe = new DatePipe('en-US');
    TabActive:String;

    fn: ModuleFunctionComponent = new ModuleFunctionComponent();

    TabLayout11: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout12: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout21: GetPxpModuleInputAndonLayoutOutput[] = [];
    TabLayout22: GetPxpModuleInputAndonLayoutOutput[] = [];

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

                this.loadtab = 0;
                let counttab = 3;

                console.log("getPxpModuleInputAndonLayout('BIG', '2')");
                this. _serviceProxy.getPxpModuleInputAndonLayout('BIG', '2')
                .pipe(finalize(() => { this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        this.TabLayout11 =  result.filter(a=> a.screenArea == '1');
                        this.TabLayout12 =  result.filter(a=> a.screenArea == '2');
                    }
                });

                console.log("getPxpModuleInputAndonLayout('BIG', '3')");
                this. _serviceProxy.getPxpModuleInputAndonLayout('BIG', '3')
                .pipe(finalize(() => { this.completeLayoutData(counttab); }))
                .subscribe((result) => {
                    if(result){
                        this.TabLayout21 =  result.filter(a=> a.screenArea == '1');
                        this.TabLayout22 =  result.filter(a=> a.screenArea == '2');
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
        console.log("------------- drawScreen:" + this.loadtab + ', count: ' + count );
        if(this.loadtab==count) {
            console.log("-- first repeatBindData :" + this.loadtab + ', count: ' + count );

            // bắt đầu request data
            this.repeatBindData();

            setTimeout(() => { this.drawScreen(); }, 500);
        }
    }

    ngOnDestroy(): void{
        clearTimeout(this.timerstorage);
    }
    timecount:number = 0;
    refeshPage: number = 300;
    timerstorage;
    secondDelay = 3000;
    repeatBindData(){

        if (this.timecount > this.refeshPage) window.location.reload();
        this.timecount = this.timecount + 1;
        console.log("repeatBindData:" + this.timecount);
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
            case '0':
                this. _serviceProxy.getPxpModuleCaseList('BIG')
                    .subscribe((result) => {
                        if(result){
                            if(this.TabActive == '0'){
                                this.BIND_UPLINE(result);
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
            case '1':
                this. _serviceProxy.getPxpModuleInputAndonData('BIG', '2')
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
                this. _serviceProxy.getPxpModuleInputAndonData('BIG', '3')
                    .subscribe((result) => {
                        if(result){
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
            default:
                this.TabActive = '0';
                this.LOAD_TAB();
                break;
        }

        this. _serviceProxy.getPxpModuleCaseNoLocation()
        .subscribe((result) => {
            if(result){
                this.NoLocation =  result;
            }
        },(error) => {
            console.log(error);
        });
    }

    BIND_UPLINE(_data: GetPxpModuleCaseListOutput[]) {

        let _d = document.querySelectorAll<HTMLElement>('.UP_LINE .UNPACK_MODULE .upmodule_item .caseno, ' +
                                                                                        '.UP_LINE .UNPACK_MODULE .upmodule_item .remark'); //
        for(let i=0; _d[i]; i++){ _d[i].textContent = ''; }

        let _clear = document.querySelectorAll<HTMLElement>('.UP_LINE .UNPACK_MODULE .upmodule_item'); //
        for(let i=0; _clear[i]; i++){
            _clear[i].setAttribute('CaseNo', '');
            _clear[i].setAttribute('SupplierNo', '');
            _clear[i].setAttribute('Renban', '');
        }


        for(let i= 0; i < _data.length; i++){
            let e = document.querySelector('.UP_LINE .UNPACK_MODULE .upmodule_item.STT' + i);
            if(e){
                e.querySelector('.caseno').textContent = _data[i].caseNo;
                e.querySelector('.remark').textContent = '('+_data[i].remark+ ')';
                // e.querySelector('.module_id').textContent = _data[i].id.toString();
                e.setAttribute('CaseNo',_data[i].caseNo);
                e.setAttribute('SupplierNo',_data[i].supplierNo);
                e.setAttribute('Renban',_data[i].renban);
            }
        }

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

    CASE_SELECTED(tab:string, locationId :number, cellName: string, rowid:number, columnId:number, screenArea:string){

        let e = document.querySelector('._tab'+ tab +' .SCREEN_AREA .cellData#SCR_NO'+tab+screenArea+'_Column' + columnId + '_Row' + (rowid) + ' .boxCellText');
        // console.log('._tab'+ tab +' .SCREEN_AREA .cellData#SCR_NO'+tab+screenArea+'_Column' + columnId + '_Row' + (rowid) + ' .boxCellText');
        if(!e.classList.contains('exists_data')){
            console.log('popupRobbing.show()');
            this.bigPopup.show(locationId, cellName, rowid);
        }
        else {
            let _d = document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData .boxCellText'); //
            for(let i=0; _d[i]; i++){  _d[i].classList.remove('selected');  }
            e.classList.add('selected');
            this.DETECT_SELECTED = 'TAB_CASE_SELECTED';
        }
    }

    drawScreen(){
        ///////////////////////////design width
        let w = window.innerWidth - 36;

        let tabcontent = document.querySelectorAll<HTMLElement>('.pxp_big_content .content_tab .SCREEN_AREA');
        for(let i=0; tabcontent[i]; i++){ tabcontent[i].style.width = w + 'px'; }


        //Tab0
        let wzone =  Math.floor((w / 16) * 100) / 100;
        let zitem =  document.querySelectorAll<HTMLElement>('.UP_LINE .zitem .zitembox');
        for(let i=0; zitem[i]; i++){ zitem[i].style.width = (wzone - 1) + 'px'; }




        w = w + 12;
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

        //Tab2 SCR_NO22
        this.fn.drawWidthTab('SCR_NO22', w, colcount);



        ///////////////////////////design height

        let hcontent = this.fn.drawHeightContent() - 10;
        let hcontent2 = Math.floor((hcontent  / 2) * 100) / 100;

        let hzone = Math.floor(((hcontent -10)/ 11) * 100) / 100;

        //Tab0
        zitem =  document.querySelectorAll<HTMLElement>('.UP_LINE .zitem .zitembox, .UP_LINE .zitem.zblank, .UP_LINE .scroll_unpack .tab img');
        for(let i=0; zitem[i]; i++){ zitem[i].style.height = (hzone - 1) + 'px'; }
        let img =  document.querySelectorAll<HTMLElement>('.UP_LINE .scroll_unpack .tab img');
        for(let i=0; img[i]; i++){ img[i].style.height = (hzone - 12) + 'px'; }

        let zitem2 =  document.querySelectorAll<HTMLElement>('.UP_LINE .zitem .upmodule_item');
        let hzone2 = (hzone*2);
        for(let i=0; zitem2[i]; i++){ zitem2[i].style.height =  (hzone2 - 1)+ 'px'; }


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

    }

    changetab(tab:String){

        this.fn.changetab(tab);
        this.TabActive = tab; //1,2,3
        // this.ngOnDestroy();

        // this.timerstorage = setTimeout(()=>{
        //     this.repeatBindData();
        // }, 10);
    }
    unpacking_change(tab:String){

        let _tab = document.querySelectorAll<HTMLElement>('.UP_LINE .scroll_unpack, .UP_LINE .UNPACK_MODULE');
        for(let i=0; _tab[i]; i++){ _tab[i].classList.remove('active'); }
        _tab = document.querySelectorAll<HTMLElement>('.UP_LINE .scroll_unpack .' + tab + ', .UP_LINE .UNPACK_MODULE.' + tab);
        for(let i=0; _tab[i]; i++){ _tab[i].classList.add('active'); }
    }

    SelectedUnpack(stt:number){

        let _item = document.querySelectorAll<HTMLElement>('.UP_LINE .zitem .zitembox.upmodule_item');
        for(let i=0; _item[i]; i++){ _item[i].classList.remove('selected'); }

        let _el = document.querySelector<HTMLElement>('.UP_LINE .zitem .zitembox.upmodule_item.STT' + stt);
        let module_id = '0';
        let supplierNo = '';
        let caseNo  = '';
        let renban = '';
        if(_el){
            _el.classList.add('selected');
            module_id = _el.querySelector('.module_id').textContent;
            supplierNo = _el.getAttribute("supplierNo");
            caseNo = _el.getAttribute("caseNo");
            renban = _el.getAttribute("renban");
        }

        if (module_id != '0'){

            this._serviceProxy.pxpModuleInputUnpackingCall(caseNo, renban, supplierNo)
            .subscribe((result) => {
                // if(result){
                    alert('LGW_MWH_CASE_DATA_UNPACKING_CALL ' + caseNo + ', ' + renban + ', ' + supplierNo);
                    // _el.classList.remove('selected');
                // }
            });
        }

    }




    MOVE_UOT_DELETE(){
        try{
            switch (this.DETECT_SELECTED) {
                case 'TAB_CASE_SELECTED':
                    let _selected = document.querySelector('._tab'+ this.TabActive + ' .SCREEN_AREA .cellData .boxCellText.selected');
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
                default:
                    break;
            }
         }catch(ex){ alert(ex);  }

    }

    NO_LOCATION() {
        let nolocation = document.querySelector<HTMLElement>('.NO_LOCATION'); //
        if(nolocation.classList.contains('show')){
            nolocation.classList.remove('show');
        }else{
            nolocation.classList.add('show');
        }
    }


    ShowChooseCase() {

    }

    ChooseLocation(type:String, locationId :number, cellName: string, rowid: number){
        this.bigPopup?.show(locationId, cellName, rowid);
    }

    ClosePopupSmall() {
        console.log('Move In:  Close');
        this.LOAD_TAB();
    }
    SavePopupSmall() {
        console.log('Save: MOVE IN');
    }
}
