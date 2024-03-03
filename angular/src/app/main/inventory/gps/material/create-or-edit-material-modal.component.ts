import { MstGpsUomServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvGpsMaterialDto, InvGpsMaterialServiceProxy, MstGpsMaterialCategoryMappingServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-material-modal',
    templateUrl: './create-or-edit-material-modal.component.html',
    })
export class CreateOrEditInvGpsMaterialModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvGpsMaterial', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsMaterialDto = new CreateOrEditInvGpsMaterialDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _hasExpiryDate:boolean;
    PartTypeList = [
        {value: 'IDM' , label: "IDM"},
        {value: 'DM' , label: "DM"},
        ];
    CategoryList = [{label: '', value: '' }];
    LocationList = [
        {value: 'GPS' , label: "GPS"},
        {value: 'Shop' , label: "Shop"},
    ];
    uomList = [{label: '', value: '' }];
    _partNo;
    constructor(
        injector: Injector,
        private _mstwptInvGpsMaterialService: InvGpsMaterialServiceProxy,
        private _category: MstGpsMaterialCategoryMappingServiceProxy,
        private _uom: MstGpsUomServiceProxy
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsMaterialDto): void {

        this.CategoryList = [{label: '', value: '' }];
        this._category.getListCategory().subscribe(result => {
            result.forEach(e => {
                this.CategoryList.push({label: e.yvCategory, value: e.yvCategory});
            })
        })

        this.uomList = [{label: '', value: '' }];
        this._uom.getListGpsUom().subscribe(result => {
            result.forEach(e => {
                this.uomList.push({label: e.code + ' - ' + e.name, value: e.code});
            })
        })

        if (!rowdata) this.rowdata = new CreateOrEditInvGpsMaterialDto();
        else this.rowdata = rowdata;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this._hasExpiryDate = (this.rowdata.hasExpiryDate == "N") ? false : true;
        this.active = true;
        this._partNo = this.rowdata.partNo;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
        this.rowdata.hasExpiryDate = (this._hasExpiryDate == true) ? 'Y' : 'N'
        this.rowdata.partName = this.rowdata.partName.toUpperCase();
        this._mstwptInvGpsMaterialService.checkExistPartInGpsMaterial(this._partNo, this.rowdata.partNo)
        .subscribe(result => {
            if(!result){
                this.rowdata.partNo = this._partNo;
                this._mstwptInvGpsMaterialService.createOrEdit(this.rowdata)
                .pipe( finalize(() =>  this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            }
            else {
                this.notify.warn('PartNo đã tồn tại, hãy nhập PartNo khác!');
                document.getElementById('partNo').focus();
            }
        })
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
