import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstLgwRobbingLaneDto, MstLgwRobbingLaneServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-robbinglane-modal',
    templateUrl: './create-or-edit-robbinglane-modal.component.html',
    styleUrls: ['./create-or-edit-robbinglane-modal.component.less']
    })
export class CreateOrEditRobbingLaneModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalRobbingLane', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgwRobbingLaneDto = new CreateOrEditMstLgwRobbingLaneDto();

    active: boolean = false;
    saving: boolean = false;
    _showOnly: boolean;
    _isDisabled: boolean;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptRobbingLaneService: MstLgwRobbingLaneServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgwRobbingLaneDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLgwRobbingLaneDto();
        else this.rowdata = rowdata;
        this._showOnly = (this.rowdata.showOnly == "N")? false: true;
        this._isDisabled = (this.rowdata.isDisabled == "N")? false: true;
        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._showOnly = event.target.checked;
        this._isDisabled = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.showOnly = (this._showOnly == true)? "Y": "N";
    this.rowdata.isDisabled = (this._isDisabled == true)? "Y": "N";
    this.rowdata.isActive = (this._isActive == true)? "Y": "N";
    this._mstwptRobbingLaneService.createOrEdit(this.rowdata)
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
