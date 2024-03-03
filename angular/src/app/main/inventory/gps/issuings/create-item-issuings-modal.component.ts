import { Component, ViewChild, Injector, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { InvGpsIssuingItemInsert, InvGpsIssuingsHeaderDto, InvGpsIssuingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { IssuingsComponent } from './issuings.component';


@Component({
    selector: 'create-item-issuings-modal',
    templateUrl: './create-item-issuings-modal.component.html',
    styleUrls: ['./create-item-issuings-modal.component.less']
})
export class CreateItemIssuingsModalComponent extends AppComponentBase {
    @ViewChild('createItemIssuing', { static: true }) modal: ModalDirective | undefined;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    checkEdit: boolean = true;
    rowdata: InvGpsIssuingsHeaderDto = new InvGpsIssuingsHeaderDto();
    _partNo;
    _qtyRequest;
    partNoList = []
    minwidth0 = 'minwidth0'
    constructor(
        injector: Injector,
        private _gpsIssuing: InvGpsIssuingsServiceProxy,
        private _service: IssuingsComponent
    ) {
        super(injector);
    }

    show(rowdata?: InvGpsIssuingsHeaderDto): void {
        this.partNoList = [];
        this._partNo = '';
        this._qtyRequest = '';
        if (!rowdata) this.rowdata = new InvGpsIssuingsHeaderDto();
        else this.rowdata = rowdata;
        this.active = true;
        this.modal.show();
        this._gpsIssuing.getListPartNo().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.partNoList.push({ value: e.partNo, label: e.partNo });
                })
            }
        });
    }

    CheckRequest(e) {
        const reg = new RegExp('^[0-9]+$');
        if (Number(e.target.value) <= 0) {
            this.message.warn('Qty request phải lớn hơn 0');
            this.checkEdit = false;
            this._qtyRequest = '';
        }
    }


    save(): void {
        this.saving = true;
        if (this._qtyRequest) {
            if (Number(this._qtyRequest) <= 0 || Number(this._qtyRequest) > 10000000000) {
                this.message.warn('Qty request phải lớn hơn 0 và nhỏ hơn 10.000.000.000');
            }
            else {
                this._gpsIssuing.getNewItemRequestValidate(this.rowdata.documentNo, this._partNo, this.rowdata.shop, this.rowdata.costCenter)
                    .subscribe(res => {
                        if (res.errorDescription == '') {
                            let input = Object.assign(new InvGpsIssuingItemInsert(), {
                                partNo: this._partNo,
                                qtyRequest: this._qtyRequest,
                                issuingHeaderId: this.rowdata.id,
                                documentNo: this.rowdata.documentNo,
                                documentDate: this.rowdata.documentDate,
                                shop: this.rowdata.shop,
                                wbs: res.wbs,
                                wbsMapping: res.wbsMapping,
                                costCenter: this.rowdata.costCenter,
                                costCenterMapping: res.costCenterMapping,
                                glAccount: res.glAccount,
                                isBudgetCheck: res.isBudgetCheck
                            })

                            this._gpsIssuing.createItemRequest(input)
                                .pipe(finalize(() => this.saving = false))
                                .subscribe((res) => {
                                    this.notify.success(this.l('Create Request Successfully'));
                                    this.close();
                                    this._service.searchDataDetails(this.rowdata.id);
                                });
                            this.saving = false;
                        } else {
                            this.notify.warn(res.errorDescription);
                        }
                    })
            }
        }
        else {
            this.message.warn('Qty request không được để trống');
        }
    }
    close(): void {
        this._partNo = '';
        this._qtyRequest = '';
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
