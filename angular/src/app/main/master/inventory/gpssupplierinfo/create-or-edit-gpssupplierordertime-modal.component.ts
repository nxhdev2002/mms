import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvGpsSupplierOrderTimeDto, MstInvGpsSupplierOrderTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { GpsSupplierInfoComponent } from './gpssupplierinfo.component';


@Component({
    selector: 'create-or-edit-gpssupplierordertime-modal',
    templateUrl: './create-or-edit-gpssupplierordertime-modal.component.html',
})
export class CreateOrEditGpsSupplierOrderTimeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsSupplierOrderTime', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsSupplierOrderTimeDto = new CreateOrEditMstInvGpsSupplierOrderTimeDto();

    active: boolean = false;
    saving: boolean = false;
    _orderType: boolean;
    _isActive: boolean;
    _supplierId: number;

    constructor(
        injector: Injector,
        private _mstwptMstInvGpsSupplierOrderTimeService: MstInvGpsSupplierOrderTimeServiceProxy,
        private load:GpsSupplierInfoComponent
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsSupplierOrderTimeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvGpsSupplierOrderTimeDto();
        else this.rowdata = rowdata;

        this._supplierId = this.load.supplierId;
        this._orderType = (this.rowdata.orderType == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._orderType = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.supplierId = this._supplierId;
        this.rowdata.orderType = (this._orderType == true) ? 'Y' : 'N';
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
        this._mstwptMstInvGpsSupplierOrderTimeService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
                this.load.searchDatasOrderTime(this.rowdata.supplierId);
            });
        this.saving = false;
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
