import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstPtsBmpPartListDto, MstPtsBmpPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-bmppartlist-modal',
    templateUrl: './create-or-edit-bmppartlist-modal.component.html',
    styleUrls: ['./create-or-edit-bmppartlist-modal.component.less']
})
export class CreateOrEditBmpPartListModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBmpPartList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstPtsBmpPartListDto = new CreateOrEditMstPtsBmpPartListDto();

    active: boolean = false;
    saving: boolean = false;
    _isPunch: boolean;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptBmpPartListService: MstPtsBmpPartListServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstPtsBmpPartListDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstPtsBmpPartListDto();
        else this.rowdata = rowdata;

        this._isPunch = (this.rowdata.isPunch == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isPunch = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isPunch = (this._isPunch == true) ? 'Y' : 'N';
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptBmpPartListService.createOrEdit(this.rowdata)
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

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
