import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {  CreateOrEditMstInvCustomsLeadTimeDto, MstInvCustomsLeadTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-customsleadtimemaster-modal',
    templateUrl: './create-or-edit-customsleadtimemaster-modal.component.html'
    })
export class CreateOrEditCustomsLeadTimeMasterModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvCustomsLeadTimeDto = new CreateOrEditMstInvCustomsLeadTimeDto();

    active: boolean = false;
    saving: boolean = false;
    _: boolean;

    constructor(
        injector: Injector,
        private _mstinvCustomsLeadTimeMstService: MstInvCustomsLeadTimeServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvCustomsLeadTimeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvCustomsLeadTimeDto();
        else this.rowdata = rowdata;
        this.modal.show();
    }

    changeActive(event){
        this._ = event.target.checked;
    }

    save(): void {
        if(this.validate()){
            this.saving = true;
            this._mstinvCustomsLeadTimeMstService.createOrEdit(this.rowdata)
                .pipe( finalize(() =>  this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            this.saving = false;
        }
    }

    validate(){
        
        if(this.rowdata.leadtime != null){
            if(isNaN(this.rowdata.leadtime)){
                this.message.warn(this.l('Type of leadtime must be number'));
                return false;
            }else{
                if(this.rowdata.leadtime < 0){
                    this.message.warn(this.l('Leadtime must be positive number'));
                    return false;
                }
            }
        }

        return true;
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
