import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmBusinessParterDto, MstCmmBusinessParterServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-businessparter-modal',
    templateUrl: './create-or-edit-businessparter-modal.component.html'
    })
export class CreateOrEditBusinessParterModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBusinessParter', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmBusinessParterDto = new CreateOrEditMstCmmBusinessParterDto();

    active: boolean = false;
    saving: boolean = false;
    _nation
    businessPartnerCategory
    statusFlagAb
    statusFlagCb
    statusFlagAd
    statusFlagCd: boolean;

    constructor(
        injector: Injector,
        private _mstwptBusinessParterService: MstCmmBusinessParterServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCmmBusinessParterDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCmmBusinessParterDto();
        else this.rowdata = rowdata;

        this._nation = (this.rowdata.nation == "N")? false: true;
        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._nation = event.target.checked;
    }
 
    save(): void {
        this.saving = true; 
        this.rowdata.nation = ( this._nation == true) ? 'Y' : 'N'
        this._mstwptBusinessParterService.createOrEdit(this.rowdata)
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
