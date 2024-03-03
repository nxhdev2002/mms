import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptPatternDDto, MstWptPatternDServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-patternd-modal',
    templateUrl: './create-or-edit-patternd-modal.component.html',
})
export class CreateOrEditPatternDModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPatternD', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstWptPatternDDto = new CreateOrEditMstWptPatternDDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;


    constructor(
        injector: Injector,
        private _mstwptPatternDService: MstWptPatternDServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstWptPatternDDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptPatternDDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }


    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        if (this.rowdata.seasonType == "Low") {
            this.rowdata.seasonType = "L";
        }
        else if (this.rowdata.seasonType == "High") {
            this.rowdata.seasonType = "H";
        }
        else if (this.rowdata.seasonType == "Normal") {
            this.rowdata.seasonType = "N";
        }
        this._mstwptPatternDService.createOrEdit(this.rowdata)
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
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
