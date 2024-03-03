import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgaBarScanInfoDto, LgaBarScanInfoServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-barscaninfo-modal',
    templateUrl: './create-or-edit-barscaninfo-modal.component.html',
    styleUrls: ['./create-or-edit-barscaninfo-modal.component.less']
    })
export class CreateOrEditBarScanInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBarScanInfo', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgaBarScanInfoDto = new CreateOrEditLgaBarScanInfoDto();

    active: boolean = false;
    saving: boolean = false;
    _status
    _isActive
: boolean;

    constructor(
        injector: Injector,
        private _mstwptBarScanInfoService: LgaBarScanInfoServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgaBarScanInfoDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgaBarScanInfoDto();
        else this.rowdata = rowdata;
		const dateValue =  this.rowdata.scanType ? new Date(this.rowdata.scanType?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValue);

        this._status= (this.rowdata.status== "N")? false: true;
        this._isActive= (this.rowdata.isActive== "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
    this.saving = true;
    this.rowdata.status = (this._status== true) ? 'Y' : 'N'
    this.rowdata.isActive = (this._isActive== true) ? 'Y' : 'N'
    this._mstwptBarScanInfoService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            this.modalSave.emit(this.rowdata);
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
