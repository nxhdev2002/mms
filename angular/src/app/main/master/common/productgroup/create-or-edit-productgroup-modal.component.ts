import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { MstCmmProductGroupDto, MstCmmProductGroupServiceProxy } from '@shared/service-proxies/service-proxies';



@Component({
    selector: 'create-or-edit-productgroup-modal',
    templateUrl: './create-or-edit-productgroup-modal.component.html',
    })
export class CreateOrEditProductGropupModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalProductGroup', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmProductGroupDto = new MstCmmProductGroupDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptProductGropupService: MstCmmProductGroupServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: MstCmmProductGroupDto): void {
        if (!rowdata) this.rowdata = new MstCmmProductGroupDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._isActive = event.target.checked;
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
