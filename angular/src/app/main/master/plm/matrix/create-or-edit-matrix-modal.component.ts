import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMasterPlmMatrixDto, MasterPlmMatrixServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-matrix-modal',
    templateUrl: './create-or-edit-matrix-modal.component.html',
    styleUrls: ['./create-or-edit-matrix-modal.component.less']
    })
export class CreateOrEditMatrixModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalMatrix', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMasterPlmMatrixDto = new CreateOrEditMasterPlmMatrixDto();

    active: boolean = false;
    saving: boolean = false;
    

    constructor(
        injector: Injector,
        private _mstplmMatrixService: MasterPlmMatrixServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMasterPlmMatrixDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMasterPlmMatrixDto();
        else this.rowdata = rowdata;
        this.active = true;
        this.modal.show();
    } 

    
 
    save(): void {
    this.saving = true; 
    this._mstplmMatrixService.createOrEdit(this.rowdata)
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
