import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditWldAdoWeldingSignalDto, WldAdoWeldingSignalServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-weldingsignal-modal',
    templateUrl: './create-or-edit-weldingsignal-modal.component.html',
    styleUrls: ['./create-or-edit-weldingsignal-modal.component.less']
    })
export class CreateOrEditWeldingSignalModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalWeldingSignal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditWldAdoWeldingSignalDto = new CreateOrEditWldAdoWeldingSignalDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptWeldingSignalService: WldAdoWeldingSignalServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditWldAdoWeldingSignalDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditWldAdoWeldingSignalDto();
        else this.rowdata = rowdata;
		const dateValue =  this.rowdata.signalDate ? new Date(this.rowdata.signalDate?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValue);

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstwptWeldingSignalService.createOrEdit(this.rowdata)
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
