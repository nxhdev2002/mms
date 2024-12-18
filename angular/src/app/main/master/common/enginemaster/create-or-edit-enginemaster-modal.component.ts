import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmEngineMasterDto, MstCmmEngineMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-enginemaster-modal',
    templateUrl: './create-or-edit-enginemaster-modal.component.html'
    })
export class CreateOrEditEngineMasterModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalEngineMaster', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmEngineMasterDto = new CreateOrEditMstCmmEngineMasterDto();

    active: boolean = false;
    saving: boolean = false;

    _: boolean;

    constructor(
        injector: Injector,
        private _mstwptEngineMasterService: MstCmmEngineMasterServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCmmEngineMasterDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCmmEngineMasterDto();
        else this.rowdata = rowdata;


        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._ = event.target.checked;
    }
 
    save(): void {
    this.saving = true; 
   
    this._mstwptEngineMasterService.createOrEdit(this.rowdata)
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
