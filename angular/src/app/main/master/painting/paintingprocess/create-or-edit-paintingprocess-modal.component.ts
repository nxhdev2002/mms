import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstPtsPaintingProcessDto, MstPtsPaintingProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-paintingprocess-modal',
    templateUrl: './create-or-edit-paintingprocess-modal.component.html',
    styleUrls: ['./create-or-edit-paintingprocess-modal.component.less']
})
export class CreateOrEditPaintingProcessModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPaintingProcess', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstPtsPaintingProcessDto = new CreateOrEditMstPtsPaintingProcessDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptPaintingProcessService: MstPtsPaintingProcessServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstPtsPaintingProcessDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstPtsPaintingProcessDto();
        else this.rowdata = rowdata;
        const dateValue = this.rowdata.processSeq ? new Date(this.rowdata.processSeq?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptPaintingProcessService.createOrEdit(this.rowdata)
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
