import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmMaterialMasterDto, MstCmmMaterialMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-materialmaster-modal',
    templateUrl: './create-or-edit-materialmaster-modal.component.html',
    styleUrls: ['./create-or-edit-materialmaster-modal.component.less']
})
export class CreateOrEditMaterialMasterModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalMaterialMaster', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmMaterialMasterDto = new CreateOrEditMstCmmMaterialMasterDto();

    _isActive: boolean = false;
    saving: boolean = false;
    _industrySector
    _effectiveDateFrom
    _effectiveDateTo
    deletionFlag
    procurementType
    repetManufacturing
    doNotCost
    valuationCategory
    priceDetermination
    priceControl
    withQtyStructure
    materialOrigin
    isActive: boolean;
    

    constructor(
        injector: Injector,
        private _mstwptMaterialMasterService: MstCmmMaterialMasterServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCmmMaterialMasterDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCmmMaterialMasterDto();
        else this.rowdata = rowdata;

        this._industrySector
        this.deletionFlag
        this.procurementType
        this.repetManufacturing
        this.doNotCost
        this.valuationCategory
        this.priceDetermination
        this.priceControl
        this.withQtyStructure
        this.materialOrigin
        this.isActive
        this.deletionFlag
        this.procurementType
        this.repetManufacturing
        this.doNotCost
        this.valuationCategory
        this.priceDetermination
        this.priceControl
        this.withQtyStructure
        this.materialOrigin
        this.isActive == false ? "N" : "Y";
  
        this.modal.show();
    }

    changeActive(event) {
        this._industrySector
        this.deletionFlag
        this.procurementType
        this.repetManufacturing
        this.doNotCost
        this.valuationCategory
        this.priceDetermination
        this.priceControl
        this.withQtyStructure
        this.materialOrigin
        this.isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.industrySector;
        this.deletionFlag
        this.procurementType
        this.repetManufacturing
        this.doNotCost
        this.valuationCategory
        this.priceDetermination
        this.priceControl
        this.withQtyStructure
        this.materialOrigin
        this.isActive = this._industrySector
        this.deletionFlag
        this.procurementType
        this.repetManufacturing
        this.doNotCost
        this.valuationCategory
        this.priceDetermination
        this.priceControl
        this.withQtyStructure
        this.materialOrigin
        this.isActive == true ? 'Y' : 'N'
        this._mstwptMaterialMasterService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
    }

    close(): void {
        this.isActive = false;
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
