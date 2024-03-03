import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstLgaPcRackDto, MstLgaPcRackServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-pcrack-modal',
    templateUrl: './create-or-edit-pcrack-modal.component.html',
    styleUrls: ['./create-or-edit-pcrack-modal.component.less']
    })
export class CreateOrEditPcRackModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPcRack', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgaPcRackDto = new CreateOrEditMstLgaPcRackDto();

    isActive: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptPcRackService: MstLgaPcRackServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgaPcRackDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLgaPcRackDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.isActive = true;
        this.modal.show();
    }

    changeActive(event){
        this.isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstwptPcRackService.createOrEdit(this.rowdata)
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
        this.isActive = false;
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
