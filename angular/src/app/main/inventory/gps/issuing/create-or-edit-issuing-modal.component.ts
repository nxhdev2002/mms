import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvGpsIssuingDto, InvGpsIssuingServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';
import { log } from 'console';
import { TIMEOUT } from 'dns';


@Component({
    selector: 'create-or-edit-issuing-modal',
    templateUrl: './create-or-edit-issuing-modal.component.html',
    styleUrls: ['./create-or-edit-issuing-modal.component.less']
})
export class CreateOrEditIssuingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalStockIssuing', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('datepicker_e', { static: false }) datepicker_e!: BsDatepickerDirective;
    @ViewChild('datepicker_r', { static: false }) datepicker_r!: BsDatepickerDirective;

    rowdata: CreateOrEditInvGpsIssuingDto = new CreateOrEditInvGpsIssuingDto();

    active: boolean = false;
    saving: boolean = false;
    isCheckBudget: string = '';
    isBudgetSuccess: boolean = false;

    constructor(
        injector: Injector,
        private _gpsIssuing: InvGpsIssuingServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsIssuingDto): void {
        this._gpsIssuing.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG_2').subscribe((e) => {
            this.isCheckBudget = e.itemValue;
        });

        if (!rowdata) this.rowdata = new CreateOrEditInvGpsIssuingDto();
        else this.rowdata = rowdata;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.status = 'REQUESTED';
        this.rowdata.isGentani = 'N';
        if (this.isCheckBudget === 'Y') {
            this.fnCheckBudget();
        }
        setTimeout(() => {
            this._gpsIssuing.gpsIssuingRequestCheck(this.rowdata.partNo)
                .subscribe((res) => {

                    if (res.result == '0M0R') {
                        this.notify.warn('Part No ' + this.rowdata.partNo + ' không tồn tại');
                    }
                    else if (res.result == '0M') {
                        this.notify.warn('Part No ' + this.rowdata.partNo + ' không tồn tại');
                    }
                    else if (res.result == '0R') {
                        this.notify.warn('Part No ' + this.rowdata.partNo + ' không chưa có trong kho GPS');

                        if (this.isBudgetSuccess == true && this.isCheckBudget === 'Y') {
                            this._gpsIssuing.createOrEdit(this.rowdata)
                                .pipe(finalize(() => this.saving = false))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Create Request Successfully'));
                                    this.close();
                                    this.modal?.hide();
                                    this.modalSave.emit(this.rowdata);
                                });
                            this.saving = false;
                        }
                        else if (this.isBudgetSuccess == false && this.isCheckBudget === 'Y') {
                            this.notify.info(this.l('Budget is not enough'));
                        }
                        else if (this.isCheckBudget === 'N') {
                            this._gpsIssuing.createOrEdit(this.rowdata)
                                .pipe(finalize(() => this.saving = false))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Create Request Successfully'));
                                    this.close();
                                    this.modal?.hide();
                                    this.modalSave.emit(this.rowdata);
                                });
                            this.saving = false;
                        }

                    }
                    else if (res.result == '1') {
                        if (this.isBudgetSuccess == true && this.isCheckBudget === 'Y') {
                            this._gpsIssuing.createOrEdit(this.rowdata)
                                .pipe(finalize(() => this.saving = false))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Create Request Successfully'));
                                    this.close();
                                    this.modal?.hide();
                                    this.modalSave.emit(this.rowdata);
                                });
                            this.saving = false;
                        }
                        else if (this.isBudgetSuccess == false && this.isCheckBudget === 'Y') {
                            this.notify.info(this.l('Budget is not enough'));
                        }
                        else if (this.isCheckBudget === 'N') {
                            this._gpsIssuing.createOrEdit(this.rowdata)
                                .pipe(finalize(() => this.saving = false))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Create Request Successfully'));
                                    this.close();
                                    this.modal?.hide();
                                    this.modalSave.emit(this.rowdata);
                                });
                            this.saving = false;
                        }
                    }
                })
        }, 500);

    }
    fnCheckBudget() {
        this._gpsIssuing.checkBudget(this.rowdata.id.toString())
            .subscribe((budget) => {
                this.isBudgetSuccess = true;
            })
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
