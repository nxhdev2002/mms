import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMasterPlmPartDto, MasterPlmPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-part-modal',
    templateUrl: './create-or-edit-part-modal.component.html',
    styleUrls: ['./create-or-edit-part-modal.component.less']
    })
export class CreateOrEditPartModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPart', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMasterPlmPartDto = new CreateOrEditMasterPlmPartDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;


    constructor(
        injector: Injector,
        private _mstwptPartService: MasterPlmPartServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMasterPlmPartDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMasterPlmPartDto();
        else this.rowdata = rowdata;

        //this._ = (this.rowdata._ == "N")? false: true;
        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._isActive = event.target.checked;
    }
 
    save(): void {
    this.saving = true; 
    //this.rowdata.isActive = (this._ == true) ? 'Y' : 'N'
    this._mstwptPartService.createOrEdit(this.rowdata)
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
