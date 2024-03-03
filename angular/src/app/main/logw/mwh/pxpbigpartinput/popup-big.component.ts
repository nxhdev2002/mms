import { Component, ViewChild, Injector,  Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';

//add Service
import { LgwMwhPxpModuleInputAndonServiceProxy,
             GetPxpModuleCaseListOutput,
             GetPxpModuleSuggestionListOutput,
             GetPxpModuleLotCodeOutput } from '@shared/service-proxies/service-proxies';

//app
import { finalize } from 'rxjs/operators';
import { ModuleFunctionComponent } from '../pxpsmallpartinput/modulefunction.component';


@Component({
    selector: 'popupBig',
    templateUrl: '../pxpsmallpartinput/popup-small.component.html',
    styleUrls: ['../pxpsmallpartinput/popup-small.component.less']
})

export class PopupBigComponent extends AppComponentBase {
    @ViewChild('popupSmall', { static: false }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    POPUP_NAME: String = 'BIG';
    LIST_CASE_TYPE:String = 'MANUAL';
    CaseList: GetPxpModuleCaseListOutput[] = [];
    SuggestionList: GetPxpModuleSuggestionListOutput[] = [];
    LotCode: GetPxpModuleLotCodeOutput[] = [];

    fn: ModuleFunctionComponent = new ModuleFunctionComponent();
    active: boolean = false;
    saving: boolean = false;

    focus_form:string = '1';

    locationId:number = 0;
    rowname:number = 0;
    cellName:string = '';
    module_type:string = '';

    getdatatab:number;

    constructor(
        injector: Injector,
        private _serviceProxy: LgwMwhPxpModuleInputAndonServiceProxy,
    ) {
        super(injector);


    }

    show(_locationId: number, p_cellName: string, p_rowid: number): void {
        this.locationId = _locationId;
        this.cellName = p_cellName;
        this.rowname = p_rowid;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .header_location').textContent = p_cellName + ':' + p_rowid.toString();
        this.CLEAR_LOTCODE();
        this.SHOW_LOTCODE();
        this.HIDE_TO_CASE();

        this.getdatatab = 0;
        let counttab = 3;

        this. _serviceProxy.getPxpModuleCaseList('BIG')
        .pipe(finalize(() => {  this.layoutDraw(counttab); }))
        .subscribe((result) => {
            if(result){
                this.CaseList =  result;
            }
        });

        this. _serviceProxy.getPxpModuleSuggestionList('BIG')
        .pipe(finalize(() => { this.layoutDraw(counttab);   }))
        .subscribe((result) => {
            if(result){
                this.SuggestionList =  result;
            }
        });

        this. _serviceProxy.getPxpModuleLotCode()
        .pipe(finalize(() => { this.layoutDraw(counttab);  }))
        .subscribe((result) => {
            if(result){
                this.LotCode =  result;
            }
        });


        this.active = true;
        let el = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase'));
        el.focus();
        this.modal.show();
    }

    layoutDraw(counttab: number){
        this.getdatatab+=1;
        if(this.getdatatab==counttab) {
            console.log("------------- drawScreen popup:" + this.getdatatab);

            setTimeout(() => { this.layoutDrawScreen(); }, 500);
        }
    }

    layoutDrawScreen(){

         this.fn.drawBoxNumberPopup(this.GET_CSS_POPUP_NAME());

    }

    save(): void {
        this.saving = true;

        // this._mstwldPaintingProgressService.createOrEdit(this.rowdata)
        //     .pipe(finalize(() => this.saving = false))
        //     .subscribe(() => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.close();
        //         this.modal?.hide();
        //         this.modalSave.emit(this.rowdata);
        //     });
        this.saving = false;
    }

    close(): void {
        this.clear_value();

        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }
    clear_value(){
        this.CLEAR_CASENO();
        this.CLEAR_FROM_CASE();
        this.CLEAR_TO_CASE();

        this.focus_form = '1';
        this.locationId = 0;
        this.cellName = '';
        this.rowname = 0;
        this.module_type = '';

        this.LIST_CASE_TYPE = 'MANUAL';
        this.SHOW_TO_CASE();
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .header_location').textContent = '';

        this.CLEAR_LOTCODE();
        // this.HIDE_LOTCODE();

    }

    TAB_CASE_LIST(type:string){

        let _tab = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .btn-mode, ' +
                                                                                          this.GET_CSS_POPUP_NAME() +' .CASE_DEFAULT, ' +
                                                                                          this.GET_CSS_POPUP_NAME() + ' .CASE_GOI_Y');
        for(let i=0; _tab[i]; i++){ _tab[i].classList.remove('active'); }
        _tab = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .' + type);
        for(let i=0; _tab[i]; i++){ _tab[i].classList.add('active'); }


        //clear from
        this.CLEAR_CASENO();
        this.CLEAR_FROM_CASE();
        this.CLEAR_TO_CASE();
        this.CLEAR_LOTCODE();
        this.SHOW_FROM_CASE();
        this.focus_form == '1';
        this.module_type = '';

        this.LIST_CASE_TYPE = type;
        if(type == 'GOI_Y') {
            this.SHOW_TO_CASE();
        }
        else if (type == 'MANUAL') {
            this.HIDE_TO_CASE();
        }

    }


    textFocus_SMALL(_form:string){
        this.focus_form = _form;
    }

    NumberSelectSMALL(num:string){
        if(this.LIST_CASE_TYPE == 'GOI_Y'){ return; }

        let _input = '';
        if(this.focus_form == '1'){
            _input = '_input_fromCase';
        }
        else if (this.focus_form == "2") {
            _input = '_input_toCase';
        }

        let el = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #' + _input));
        let val = el.value.trim();
        val = val.concat(num);
        el.value = val;
        el.focus();
    }

    ADD_CASE(){

        // var cNo = $(".PustCaseOnRack_ONFLR ._grade").text().trim() + $("#_fromCase_ONFLR").val().trim();
        // if (cNo.length <= 0 || cNo.length > 4) {
        //     alert("Case No không hợp lệ!");
        //     $("#_fromCase_ONFLR").focus();
        //     return;
        // }

        // // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
        // var pad = "0000";
        // cNo = $("#lblLocation_ONFLR").text() + pad.substring(0, pad.length - $("#_fromCase_ONFLR").val().trim().length) + $("#_fromCase_ONFLR").val().trim();

        // //check exists case
        // var obj = $(".PustCaseOnRack_ONFLR .popupONFLR .caseitem");
        // for (var i = 0; i < obj.length; i++) {
        //     if ($(obj[i]).text() == cNo) {
        //         if (confirm("Case No đã được add, bạn muốn xóa khỏi list !")) {
        //             $(".PustCaseOnRack_ONFLR .popupONFLR .caseitem." + cNo).remove();
        //         }
        //         $("#_fromCase_ONFLR").val("");
        //         $("#_fromCase_ONFLR").focus();
        //         return;
        //     }
        // }

        // var _htm = "<div class='caseitem " + cNo + "'>" + cNo + "</div>" +
        //     "<div class='removecase " + cNo + "' onclick='ONFLR_REMOVE_CASE(\"" + cNo + "\")' >-</div>" +
        //     "<div class='clear'></div>";


        // $(".PustCaseOnRack_ONFLR .popupONFLR").append(_htm);
        // $("#_fromCase_ONFLR").val("");
        // $("#_fromCase_ONFLR").focus();
    }

    BackspaceSMALL(){
        if(this.LIST_CASE_TYPE == 'GOI_Y'){ return; }

        let _input = '';
        if(this.focus_form == '1'){
            _input = '_input_fromCase';
        }
        else if (this.focus_form == "2") {
            _input = '_input_toCase';
        }

        let el = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #' + _input));
        let val = el.value.trim();
        if (val.length > 0) {
            val = val.substring(0, val.length - Math.min(1, val.length));
        }
        el.value = val;

    }

    SELECTED_LOTCODE(lotcode:string){

        //check exists selected
        let lotitem = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .lotcode .lotcodeitem.' + lotcode);
        if(lotitem){
            if(lotitem.classList.contains('selected')) {

                this.CLEAR_LOTCODE();
                this.SHOW_FROM_CASE();
                this.SHOW_TO_CASE();

                return;
            }
        }

        this.CLEAR_LOTCODE();
        // this.CLEAR_CASENO();
        // this.HIDE_CASENO();
        this.SHOW_FROM_CASE();
        this.HIDE_TO_CASE();


        this.module_type = 'LOTCODE';
        this.SHOW_LOTCODE();
        lotitem = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .lotcode .lotcodeitem.' + lotcode);
        if(lotitem){
            lotitem.classList.add('selected');
        }
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' ._lotcode').textContent = lotcode;

        //To Case
    }

    CaseSelectMANUAL(p_id:string, p_case_no: string, p_supplier_no: string, p_renban: string) {

        // this.CLEAR_LOTCODE();
        // this.HIDE_LOTCODE();
        this.CLEAR_CASENO();
        this.SHOW_CASENO();
        this.SHOW_FROM_CASE();
        // this.SHOW_TO_CASE();

        this.module_type = 'CASENO';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = p_case_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = p_supplier_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = p_renban;

        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL.BoxID_' + p_case_no+ '_' + p_supplier_no + '_' +p_renban).classList.add('selected');

    }

    CaseSelectGOI_Y(p_id:string, p_case_no: string, p_supplier_no: string, p_renban: string, p_casePrefix: string) {

        this.CLEAR_LOTCODE();
        // this.HIDE_LOTCODE();
        this.CLEAR_CASENO();
        this.SHOW_CASENO();


        this.module_type = 'CASENO';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._casenofull').textContent = p_case_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = p_casePrefix;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = p_supplier_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = p_renban;

        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase')).value = p_case_no.replace(p_casePrefix,'');
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL.BoxID_' + p_id).classList.add('selected');

    }




    MOVE_IN_OLD(type:String){

        let listcaseitem = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .popupSMALL .caseitem');
        if(listcaseitem.length > 0){

        }else{

            let fromCase = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase'));
            let fromVal = fromCase.value.trim();
            let toCase = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_toCase'));
            let toVal = toCase.value.trim();

            if(toVal != '') {
                if(this.validateCase(fromCase, fromVal, toCase, toVal)){

                }
            }else {

                let caseno = '';
                let supplier_no ='';
                if (this.module_type == 'CASENO') {
                    let _case = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent;
                    let renban = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent;
                    supplier_no = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent;

                    // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
                    let pad = "0000";
                    caseno = _case + pad.substring(0, pad.length - fromVal.length) + fromVal;
                }
               else if(this.module_type == 'LOTCODE') {
                    let lotcode = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._lotcode').textContent;
                     // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
                     let pad = "0000";
                     caseno = lotcode + '-' + pad.substring(0, pad.length - fromVal.length) + fromVal;
               }



                if ((0 < caseno.length && caseno.length < 4)) {
                    alert("Case No không hợp lệ!");
                    fromCase.focus();
                    return;
                }

                this.saving = true;
                alert("LGW_MWH_CASE_DATA_MOVE_IN '" + caseno + "', '" + supplier_no + "', '" + this.locationId.toString() + "', null, '' ");
                this._serviceProxy.pxpModuleInputMoveIn(caseno, supplier_no, this.locationId.toString(), null, '')
                .subscribe((result) => {
                    this.close();
                });
                this.saving = false;
            }
        }
    }

    MOVE_IN(type:String){

            let fromCase = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase'));
            let fromVal = fromCase.value.trim();
            // let toCase = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_toCase'));
            // let toVal = toCase.value.trim();


            let caseno = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent;
            let renban = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent;
            let supplier_no = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent;
            let lotcode = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._lotcode').textContent;

            // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
            let pad = "0000";
            let padFromVal = '';

            lotcode = (lotcode!='') ? (lotcode + '-'):'';
            caseno = (caseno!='') ? (caseno + '-'):'';
            if(lotcode!= ''){
                padFromVal = fromVal;
            }else{
                padFromVal = pad.substring(0, pad.length - fromVal.length) + fromVal;
            }
            let module =  caseno + lotcode + padFromVal;

            if(caseno == ''){
                alert("Case không được để trống!");
                fromCase.focus();
                return;
            }

            this.saving = true;
            alert("LGW_MWH_CASE_DATA_MOVE_IN '" + module + "', '" + supplier_no + "', '" + this.locationId.toString() + "', null, '' ");
            this._serviceProxy.pxpModuleInputMoveIn(module, supplier_no, this.locationId.toString(), null, '')
            .subscribe((result) => {
                this.close();
            });
            this.saving = false;

    }

    validateCase(fromCase: HTMLInputElement, _fromVal: String, toCase : HTMLInputElement, _toVal : String){

        if (_fromVal.length <= 0 || _fromVal.length > 4) {
            alert("Case No không hợp lệ!");
            fromCase.focus();
            return false;
        }
        if (_toVal.length <= 0 || _toVal.length > 4) {
            alert("Case No không hợp lệ!");
            toCase.focus();
            return false;
        }

        // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
        let pad = "0000";
        let _case = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' ._caseno')).value;
        if(_case) { _case = _case.trim(); }
        let caseno = _case + pad.substring(0, pad.length - _fromVal.length) + _fromVal;

        //check exists case
        let caseitem = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .popupSMALL .caseitem');

        for (var i = 0; i < caseitem.length; i++) {
            if (caseitem[i].textContent == caseno) {
                if (confirm("Case No đã được add, bạn muốn xóa khỏi list !")) {
                    document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .popupSMALL .caseitem.' + caseno).remove();
                }
                fromCase.value = '';
                fromCase.focus();
                return false;
            }
        }

        // document.querySelectorAll<HTMLElement>('.SCREEN_AREA .cellData.');
        // let maxColumn = $(".SCREEN_AREA .cellData.COL_" + location + ":not(.exists_data)").length;

        return true;
    }


    GET_POPUP_NAME(){
        return this.POPUP_NAME;
    }
    GET_CSS_POPUP_NAME(){
        return '.' + this.POPUP_NAME + '.PustCaseOnRack_SMALL';
    }

    SHOW_CASENO(){
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .CASENO_TXT').style.display = 'block';
    }
    HIDE_CASENO(){
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .CASENO_TXT').style.display = 'none';
    }
    CLEAR_CASENO(){
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._casenofull').textContent = '';

        let _box = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL');
        for(let i=0; _box[i]; i++){ _box[i].classList.remove('selected'); }
    }





    SHOW_LOTCODE(){
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .LOTCODE_TXT').style.display = 'block';
    }
    HIDE_LOTCODE(){
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' .LOTCODE_TXT').style.display = 'none';
    }
    CLEAR_LOTCODE(){
            let lotitemlist = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME()  + ' .lotcode .lotcodeitem');
            for(let i=0; lotitemlist[i]; i++){ lotitemlist[i].classList.remove('selected'); }
            document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME()  +' ._lotcode').textContent = '';
    }

    CLEAR_FROM_CASE(){
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase')).value = '';
    }
    SHOW_FROM_CASE(){
        this.CLEAR_FROM_CASE();
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_FROM').style.display = 'block';
    }
    HIDE_FROM_CASE(){
        this.CLEAR_FROM_CASE();
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_FROM').style.display = 'none';
    }

    CLEAR_TO_CASE(){
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_toCase')).value = '';
    }
    SHOW_TO_CASE(){
        this.CLEAR_TO_CASE();
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_TO').style.display = 'block';
    }
    HIDE_TO_CASE(){
        this.focus_form = '1';
        this.CLEAR_TO_CASE();
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_TO').style.display = 'none';
    }


    // @HostListener('document:keydown', ['$event'])
    // onKeydownHandler(event: KeyboardEvent) {
    //     if (event.key === "Escape") {
    //         this.close();
    //     }
    // }
}
