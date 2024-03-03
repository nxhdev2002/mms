import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMasterPlmMatrixLotCodeDto, MasterPlmMatrixLotCodeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-matrixlotcode-modal',
    templateUrl: './create-or-edit-matrixlotcode-modal.component.html',
    styleUrls: ['./create-or-edit-matrixlotcode-modal.component.less']
    })
export class CreateOrEditMatrixLotCodeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalMatrixLotCode', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMasterPlmMatrixLotCodeDto = new CreateOrEditMasterPlmMatrixLotCodeDto();

    active: boolean = false;
    saving: boolean = false;
    _: boolean;

    constructor(
        injector: Injector,
        private _mstwptMatrixLotCodeService: MasterPlmMatrixLotCodeServiceProxy,
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMasterPlmMatrixLotCodeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMasterPlmMatrixLotCodeDto();
        else this.rowdata = rowdata;

        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._ = event.target.checked;
    }
 
    save(): void {
    this.saving = true; 
    this._mstwptMatrixLotCodeService.createOrEdit(this.rowdata)
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
