import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgaSpsStockDto, LgaSpsStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-stock-modal',
    templateUrl: './create-or-edit-stock-modal.component.html',
    styleUrls: ['./create-or-edit-stock-modal.component.less']
    })
export class CreateOrEditStockModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalStock', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgaSpsStockDto = new CreateOrEditLgaSpsStockDto();

    isActive: boolean = false;
    saving: boolean = false;
    _isActice: boolean;

    constructor(
        injector: Injector,
        private _mstwptStockService: LgaSpsStockServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgaSpsStockDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgaSpsStockDto();
        else this.rowdata = rowdata;

        this._isActice = (this.rowdata.isActive == "N")? false: true;
        this.isActive = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActice = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isActive = (this._isActice == true) ? 'Y' : 'N'
    this._mstwptStockService.createOrEdit(this.rowdata)
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
