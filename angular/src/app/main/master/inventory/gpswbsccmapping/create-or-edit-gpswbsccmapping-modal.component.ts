import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstGpsWbsCCMappingDto, GetIssuingImportView, MstGpsWbsCCMappingServiceProxy, OnlineBudgetCheckRequest, SapIFServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ConfirmDialogComponent } from '../../common/vehiclecbu/confirm-dialog/confirm-dialog.component';



@Component({
    selector: 'create-or-edit-gpswbsccmapping-modal',
    templateUrl: './create-or-edit-gpswbsccmapping-modal.component.html',
})
export class CreateOrEditGpsWbsCCMappingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsWbsCCMapping', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('d', { static: false }) d!: BsDatepickerDirective;
    @ViewChild('d1', { static: false }) d1!: BsDatepickerDirective;
    @ViewChild('confirmDialog', { static: true }) confirmDialog: | ConfirmDialogComponent | undefined;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstGpsWbsCCMappingDto = new CreateOrEditMstGpsWbsCCMappingDto();
    rowData1: GetIssuingImportView = new GetIssuingImportView();
    active: boolean = false;
    saving: boolean = false;
    checkEdit: boolean = true;
    _effectiveFromDate: any;
    _effectiveFromTo: any;
    _isActive: boolean;
    _wbs;
    _costCenterFrom;
    _wbsFrom;

    constructor(
        injector: Injector,
        private _mstService: MstGpsWbsCCMappingServiceProxy,
        private _dateTimeService: DateTimeService,
        private _sapService: SapIFServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstGpsWbsCCMappingDto): void {
        //this._costCenterFrom = '';
        //this._wbsFrom = '';
        if (!rowdata) this.rowdata = new CreateOrEditMstGpsWbsCCMappingDto();
        else this.rowdata = rowdata;

        console.log(this.rowdata)

        const effectiveFromDate = this.rowdata.effectiveFromDate ? new Date(this.rowdata.effectiveFromDate?.toString()) : null;
        this.d?.bsValueChange.emit(effectiveFromDate);
        const effectiveFromTo = this.rowdata.effectiveFromTo ? new Date(this.rowdata.effectiveFromTo?.toString()) : null;
        this.d1?.bsValueChange.emit(effectiveFromTo);

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    fn: CommonFunction = new CommonFunction(); 

    submit(event) {

        // if(this.isSubmut) return;
        // this.isSubmut = true;
        
             
       
        this.fn.exportLoading(event, true); //disable nút create

        // if (this.rowdata.wbsFrom) {
        //     var month = Number(new Date().getMonth() + 1);
        //         var year = Number(new Date().getFullYear());
        //         var _fiscalYear = month > 4 ? (year + 1) : year;

        //         let obj = Object.assign(new OnlineBudgetCheckRequest(), {
        //             wbsMasterData: this.rowdata.wbsFrom,
        //             fiscalYear: _fiscalYear.toString(),
        //             costCenter: this.rowdata.costCenterFrom,
        //             fixedAssetNo: '', // fix 
        //             system: 'MMS', // fix cứng
        //             listItemId: [] // chỉ 1 item
        //         })
 
        //         this._sapService.sapOnlineBudgetCheck(obj)
        //             .pipe(finalize(() => { }))
        //             .subscribe((budget) => { 
        //                 if (budget.response?.dataValidation.dataValidationResult == 'T') {
        //                     console.log('check budget success! by Dậu OK', budget);
        //                     this.save(event);
        //                 }
        //                 else {
        //                     //this.confirmDialog.show("Waring", "Wbs hoặc Cost Center không chính xác"); 
        //                     console.log('budget ERROR', budget);
        //                     this.notify.error(this.l('Wbs hoặc Cost Center không chính xác'));
        //                     this.fn.exportLoading(event,false,0); //Show nút create
        //                     // this.isSubmut = false;
        //                 } 
                        
        //             })
            
        // } else { 
        //     this.save(event);
        //     console.log('a');
        // }

        // Check 

        this._mstService.checkCostCenterFromAndWbsFromInInvGpsMapping(this.rowdata.costCenterFrom, this.rowdata.wbsFrom).subscribe((e)=>{
            console.log(e.existInInvGpsMapping)
            if(e.existInInvGpsMapping == '1'){
                this.save(event)
            }else{
                this.notify.error(this.l('WBS OR CostCenter Not Exists !'));
                this.fn.exportLoading(event,false,0); //Show nút create
            }
        })
    }


    save(event): void {
        if(this.validate()){
        //this.saving = true;
        this.rowdata.effectiveFromDate = this._dateTimeService.convertToDatetime(this._effectiveFromDate);
        this.rowdata.effectiveFromTo = this._dateTimeService.convertToDatetime(this._effectiveFromTo);
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
    
       
        this._mstService.checkCostCenterFromAndWbsFrom(this.rowdata.costCenterFrom, this.rowdata.wbsFrom, this.rowdata.costCenterTo, this.rowdata.wbsTo).subscribe((e)=>{

            if(this.rowdata.id) {   //Edit bản ghi
                if(Number(e.existCostCenterFromAndWbsFrom) == this.rowdata.id || Number(e.existCostCenterFromAndWbsFrom) == 0) { 
                    //update chinh no
                    this._mstService.createOrEdit(this.rowdata)
                    .pipe(finalize(() => this.saving = false))
                    .subscribe(() => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.close();
                        this.modal?.hide();
                        this.modalSave.emit(this.rowdata);
                        this.fn.exportLoading(event,false,0); //open nút create
                    });
                    return;
                } else if(Number(e.existCostCenterFromAndWbsFrom) != this.rowdata.id) 
                {
                    // Update vào thằng khác
                    // this.confirmDialog.show("Waring", "Wbs và Cost Center đã tồn tại!"); 
                    this.notify.error(this.l('WBS and CostCenter Duplicate!'));
                    this.fn.exportLoading(event,false,0); //open nút create
                    return;
                }
            }else {  //Create bản ghi
                if(Number(e.existCostCenterFromAndWbsFrom) == 0) {
                    this._mstService.createOrEdit(this.rowdata)
                    .pipe(finalize(() => this.saving = false))
                    .subscribe(() => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.close();
                        this.modal?.hide();
                        this.modalSave.emit(this.rowdata);
                        this.fn.exportLoading(event,false,0); //open nút create
                    });
                    return;
                } else 
                {
                    // create vào thằng khác
                    // this.confirmDialog.show("Waring", "Wbs và Cost Center đã tồn tại!"); 
                    this.notify.error(this.l('WBS and CostCenter Duplicate!'));
                    this.fn.exportLoading(event,false,0); //open nút create
                    return;
                }
            } 
        }) 
    } 
    }

    validate() {
        if (this._effectiveFromDate !== null) {
            if (this._effectiveFromTo !== null) {
                if (this._dateTimeService.convertToDatetime(this._effectiveFromDate) > this._dateTimeService.convertToDatetime(this._effectiveFromTo)) {
                    this.message.warn(this.l('EffectiveFromTo phải sau EffectiveFromDate'));
                    return false;
                }
            }
        }

        return true;
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }


    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
