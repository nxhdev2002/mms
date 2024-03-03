import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCkdCustomsLeadtimeDto, MstCkdCustomsLeadtimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-customsleadtime-modal',
    templateUrl: './create-or-edit-customsleadtime-modal.component.html'
    })
export class CreateOrEditCustomsLeadtimeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalCustomsLeadtime', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCkdCustomsLeadtimeDto = new CreateOrEditMstCkdCustomsLeadtimeDto();

    active: boolean = false;
    saving: boolean = false;
    _: boolean;

    constructor(
        injector: Injector,
        private _mstwptCustomsLeadtimeService: MstCkdCustomsLeadtimeServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCkdCustomsLeadtimeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCkdCustomsLeadtimeDto();
        else this.rowdata = rowdata;

        this.modal.show();
    } 

    changeActive(event){
        this._ = event.target.checked;
    }
 
    save(): void {
    this.saving = true; 
    this._mstwptCustomsLeadtimeService.createOrEdit(this.rowdata)
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
