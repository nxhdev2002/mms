import { Component, ViewChild, Injector,  Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { LgwMwhPxpModuleInputAndonServiceProxy, GetPxpModuleCaseListOutput, GetPxpModuleSuggestionListOutput } from '@shared/service-proxies/service-proxies';




@Component({
    selector: 'popuprobbingModal',
    templateUrl: './popuprobbing-modal.component.html',
    styleUrls: ['./popup-small.component.less']
})
export class PopuprobbingModalComponent extends AppComponentBase {
    @ViewChild('popuprobbingModal', { static: true }) modal: ModalDirective | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();


    POPUP_NAME: String = 'ROBBING';
    LIST_CASE_TYPE:String = 'MANUAL';
    CaseList: GetPxpModuleCaseListOutput[] = [];
    SuggestionList: GetPxpModuleSuggestionListOutput[] = [];

    active: boolean = false;
    saving: boolean = false;

    focus_form:string = '1';

    locationId:number = 0;
    column_id:number = 0
    rowname:number = 0;
    cellName: String = '';

    constructor(
        injector: Injector,
        private _serviceProxy: LgwMwhPxpModuleInputAndonServiceProxy,
    ) {
        super(injector);


    }

    show(p_columnId: number, p_cellName: string, p_rowid: number): void {

        this.column_id = p_columnId + 1;
        this.cellName = p_cellName;
        this.rowname = p_rowid;

        // document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .header_location').textContent = p_cellName;

        this. _serviceProxy.getPxpModuleCaseList('')
        .pipe(finalize(() => {  }))
        .subscribe((result) => {
            if(result){
                console.log(result)
                this.CaseList =  result;
            }
        });


        this. _serviceProxy.getPxpModuleSuggestionList('')
        .pipe(finalize(() => {  }))
        .subscribe((result) => {
            if(result){
                this.SuggestionList =  result;
            }
        });

        this.active = true;
        let el = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase'));
        el.focus();
        this.modal.show();
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
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase')).value = '';
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_toCase')).value = '';
        this.locationId = 0;
        this.column_id = 0;
        this.cellName = '';
        this.rowname = 0;
        document.querySelector<HTMLSpanElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._casenofull').textContent = '';

        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .header_location').textContent = '';
        this.focus_form == '1';
    }

    TAB_CASE_LIST(type:string){

        let _tab = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .btn-mode, ' +
                                                                                          this.GET_CSS_POPUP_NAME() + ' .CASE_DEFAULT, ' +
                                                                                          this.GET_CSS_POPUP_NAME() + ' .CASE_GOI_Y');
        for(let i=0; _tab[i]; i++){ _tab[i].classList.remove('active'); }
        _tab = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .' + type);
        for(let i=0; _tab[i]; i++){ _tab[i].classList.add('active'); }


        //clear from
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase')).value = '';
        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_toCase')).value = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = '';
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._casenofull').textContent = '';

        let _box = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL');
        for(let i=0; _box[i]; i++){ _box[i].classList.remove('selected'); }

        this.focus_form == '1';

        this.LIST_CASE_TYPE = type;
        // if(type == 'GOI_Y') {
        //     document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_TO').style.display = 'none';
        // }
        // else if (type == 'MANUAL') {
        //     document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .CASE_TO').style.display = 'block';
        // }

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

        let el = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #' +_input));
        let val = el.value.trim();
        val = val.concat(num);
        el.value = val;
        el.focus();
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

    CaseSelectMANUAL(p_id:string, p_case_no: string, p_supplier_no: string, p_renban: string) {

        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = p_renban;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = p_supplier_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = '';

        let _box = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL');
        for(let i=0; _box[i]; i++){ _box[i].classList.remove('selected'); }
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL.BoxID_' + p_case_no+ '_' + p_supplier_no + '_' +p_renban).classList.add('selected');

    }
    CaseSelectGOI_Y(p_id:string, p_case_no: string, p_supplier_no: string, p_renban: string, p_casePrefix: string) {

        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._casenofull').textContent = p_case_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent = p_casePrefix;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent = p_supplier_no;
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent = p_renban;

        (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase')).value = p_case_no.replace(p_casePrefix,'');

        let _box = document.querySelectorAll<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL');
        for(let i=0; _box[i]; i++){ _box[i].classList.remove('selected'); }
        document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' .box.caseSMALL.BoxID_' + p_id).classList.add('selected');

    }

    MOVE_IN(type:String){

        let fromCase = (<HTMLInputElement>document.querySelector(this.GET_CSS_POPUP_NAME() + ' #_input_fromCase'));
        let fromVal = fromCase.value.trim();
        let _case = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._caseno').textContent;
        let renban = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._renban').textContent;
        let supplier_no = document.querySelector<HTMLElement>(this.GET_CSS_POPUP_NAME() + ' ._supplier_no').textContent;

        // change case_no input from number to padding 0; e.g. 1->0001, 12->0012, 123->0123
        let pad = "0000";
        let caseno = _case + pad.substring(0, pad.length - fromVal.length) + fromVal;


        if (0 >= caseno.length || caseno.length > 5) {
            alert("RENBAN không hợp lệ!");
            fromCase.focus();
            return;
        }

        if ((0 < caseno.length && caseno.length < 1) || (caseno.length == 0)) {
            alert("RENBAN không hợp lệ!");
            fromCase.focus();
            return;
        }


        alert("LGW_MWH_CASE_DATA_ROBBING_MOVE_IN  '" + caseno + "', '" + supplier_no + "', " + this.column_id.toString());
        this._serviceProxy.pxpModuleInputRobbingMoveIn(caseno, supplier_no, this.column_id.toString())
        .subscribe((result) => {
            this.close();
        });

    }


    GET_POPUP_NAME(){
        return this.POPUP_NAME;
    }

    GET_CSS_POPUP_NAME(){
        return '.' + this.POPUP_NAME + '.PustCaseOnRack_SMALL';
    }
    // @HostListener('document:keydown', ['$event'])
    // onKeydownHandler(event: KeyboardEvent) {
    //     if (event.key === "Escape") {
    //         this.close();
    //     }
    // }
}
