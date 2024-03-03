import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditWldAdoPunchQueueDto, WldAdoPunchQueueServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-punchqueue-modal',
    templateUrl: './create-or-edit-punchqueue-modal.component.html',
    styleUrls: ['./create-or-edit-punchqueue-modal.component.less']
    })
export class CreateOrEditPunchQueueModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPunchQueue', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditWldAdoPunchQueueDto = new CreateOrEditWldAdoPunchQueueDto();

    active: boolean = false;
    saving: boolean = false;
    _punchFlag:boolean;
    _isCall : boolean;
    _isCf: boolean;

    constructor(
        injector: Injector,
        private _mstwptPunchQueueService: WldAdoPunchQueueServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditWldAdoPunchQueueDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditWldAdoPunchQueueDto();
        else this.rowdata = rowdata;
		const dateValue =  this.rowdata.scanTime ? new Date(this.rowdata.scanTime?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValue);


        this._punchFlag = (this.rowdata.punchFlag == "N") ? false : true;
        this._isCf = (this.rowdata.isCf == "N") ? false : true;
        this._isCall = (this.rowdata.isCall == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }


    save(): void {
    this.saving = true;
    this.rowdata.punchFlag = (this._punchFlag == true) ? 'Y' : 'N'
    this.rowdata.isCf = (this._isCf == true) ? 'Y' : 'N'
    this.rowdata.isCall = (this._isCall == true) ? 'Y' : 'N'

    this._mstwptPunchQueueService.createOrEdit(this.rowdata)
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
