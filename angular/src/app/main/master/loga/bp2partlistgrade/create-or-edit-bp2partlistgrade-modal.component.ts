import { Component, ElementRef, EventEmitter, HostListener, Injector, Output, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { CreateOrEditMstLgaBp2PartListGradeDto, MstLgaBp2PartListGradeServiceProxy } from "@shared/service-proxies/service-proxies";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker";
import { ModalDirective } from "ngx-bootstrap/modal";
import { finalize } from "rxjs/operators";

@Component({
    selector: 'create-or-edit-bp2partlistgrade-modal',
    templateUrl: './create-or-edit-bp2partlistgrade-modal.component.html',
    styleUrls: ['./create-or-edit-bp2partlistgrade-modal.component.less'],
})
export class CreateOrEditBp2PartListGradeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBp2PartListGrade', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgaBp2PartListGradeDto = new CreateOrEditMstLgaBp2PartListGradeDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;


    constructor(
        injector: Injector,
        private _mstLgaBp2PartListGrade: MstLgaBp2PartListGradeServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgaBp2PartListGradeDto): void {
        if (!rowdata)
            this.rowdata = new CreateOrEditMstLgaBp2PartListGradeDto();
        else
            this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }


    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstLgaBp2PartListGrade.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
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
        onKeydownHandler(event: KeyboardEvent){
            if(event.key === "Escape" ){
                this.close();
            }

        }
}
