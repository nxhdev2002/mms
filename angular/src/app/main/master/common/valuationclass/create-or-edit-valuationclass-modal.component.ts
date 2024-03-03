import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { MstCmmValuationClassDto, MstCmmValuationClassServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-valuationclass-modal',
    templateUrl: './create-or-edit-valuationclass-modal.component.html',
    })
export class CreateOrEditValuationClassModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalValuationClass', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmValuationClassDto = new MstCmmValuationClassDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptValuationClassService: MstCmmValuationClassServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: MstCmmValuationClassDto): void {
        if (!rowdata) this.rowdata = new MstCmmValuationClassDto();
        else this.rowdata = rowdata;

        this.modal.show();
    } 

    // changeActive(event){
    //     this._isActive = event.target.checked;
    // }


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
