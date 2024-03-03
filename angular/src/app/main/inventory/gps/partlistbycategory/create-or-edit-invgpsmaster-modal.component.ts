import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CreateOrEditInvGpsPartListByCategoryDto, InvGpsPartListByCategoryServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'create-or-edit-invgpsmaster-modal',
    templateUrl: './create-or-edit-invgpsmaster-modal.component.html'
})
export class CreateOrEditGpsMasterModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsPartListByCategoryDto = new CreateOrEditInvGpsPartListByCategoryDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _monthStk: any;
    category;
    categoryList = [];

    constructor(
        injector: Injector,
        private _gpsService: InvGpsPartListByCategoryServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsPartListByCategoryDto): void {
        this.getCategoryCbx()
        if (!rowdata) this.rowdata = new CreateOrEditInvGpsPartListByCategoryDto();
        else this.rowdata = rowdata;

        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this._gpsService.createOrEdit(this.rowdata)
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
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }


    getCategoryCbx() {
        this.categoryList = []
        this._gpsService.getCbxCategory().subscribe((result) => {
            result.forEach(e => this.categoryList.push({ value: e.name, label: e.name }));
        });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
