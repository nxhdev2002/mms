import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvIhpPartGradeDto, InvIhpPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { IhpPartListComponent } from '../ihppartlist.component';
import { finalize } from 'rxjs';

@Component({
    selector: 'editIhpPartGrade',
    templateUrl: './edit-ihppartgrade-modal.component.html',
    styleUrls: ['./edit-ihppartgrade-modal.component.less']
})
export class EditIhpPartGradeModalComponent extends AppComponentBase {
    @ViewChild('editPartGradeModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker1', { static: false }) datepicker1!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvIhpPartGradeDto = new InvIhpPartGradeDto();
    data = [];
    isLoading: boolean = false;
    saving: boolean = false;

    _partlistId;
    _grade;
    _usageQty;
    _firstDayProduct;
    _lastDayProduct;
    listGrade = [{ value: '', label: '', cfc: '' }];
    listGradeByCfc = [{ value: '', label: '', cfc: '' }];
    listExistGrade = [];

    constructor(
        injector: Injector,
        private _service: InvIhpPartListServiceProxy,
        private _dateTimeService: DateTimeService,
        private _partListComp: IhpPartListComponent,
    ) {
        super(injector);

    }

    ngOnInit(): void {
        this._service.getListGrade().subscribe(res => {
            res.forEach(e => {
                this.listGrade.push({ value: e.grade, label: e.grade, cfc: e.cfc });
            })
        })
    }

    show(rowdata: InvIhpPartGradeDto, cfc: string, partlistId?): void {

        this.listExistGrade = [];
        this._service.getDataPartGradebyId(partlistId, '', 0, 500).subscribe((result) => {
            this.data = result.items
            if(this.data.length > 0){
                this.data.forEach(e => {
                    if(e.grade != this.rowdata.grade) this.listExistGrade.push(e.grade);
                })
            }
        });

        this.rowdata = rowdata;

        this._usageQty = this.rowdata.usageQty;

        const dateValue = this.rowdata.firstDayProduct ? new Date(this.rowdata.firstDayProduct?.toString()) : undefined;
        this.datepicker?.bsValueChange.emit(dateValue);

        const dateValue1 = this.rowdata.lastDayProduct ? new Date(this.rowdata.lastDayProduct?.toString()) : undefined;
        this.datepicker1?.bsValueChange.emit(dateValue1);

        this._grade = this.rowdata.grade;

        this.listGradeByCfc = this.listGrade.filter(e => e.cfc == cfc);

        this._partlistId = partlistId;

        this.modal.show();
    }

    save(): void {
        if(this.listExistGrade.includes(this._grade)){
            this.notify.warn(this.l('Grade already exists!'));
            return;
        }
        if (this._usageQty == null) {
            this.notify.warn(this.l('UsageQty cannot be null!'));
            return;
        }
        if (this._usageQty != null && this._usageQty < 1) {
            this.notify.warn(this.l('UsageQty cannot be less than 1!'));
            return;
        }
        if (this._firstDayProduct == undefined || this._firstDayProduct == null) {
            this.notify.warn(this.l('FirstDayProduct cannot be null!'));
            return;
        }
        if (this._firstDayProduct != null && this._lastDayProduct != null && this._firstDayProduct > this._lastDayProduct) {
            this.notify.warn(this.l('LastDayProduct cannot be less than FirstDayProduct!'));
            return;
        }

        this.rowdata.grade = this._grade;
        this.rowdata.usageQty = this._usageQty;
        this.rowdata.firstDayProduct = this._dateTimeService.convertToDatetime(this._firstDayProduct);
        this.rowdata.lastDayProduct = this._dateTimeService.convertToDatetime(this._lastDayProduct);

        this.isLoading = true;
        this._service.editIhpPartGrade(this.rowdata)
            .pipe(finalize(() => this.isLoading = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this._partListComp.searchDataPartGrade(this._partlistId);
                this.close();
            });
    }

    close(): void {
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
