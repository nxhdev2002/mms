import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockPartServiceProxy, InvCkdStockPartDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { StockPartComponent } from './stock-part.component';
import { formatDate } from '@angular/common';
import { log } from 'console';

@Component({
    selector: 'editStockPart',
    templateUrl: './edit-stock-part-modal.component.html'
})
export class EditStockPartModalComponent extends AppComponentBase {
    @ViewChild('editStockPart', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdStockPartDto = new InvCkdStockPartDto();
    isLoading: boolean = false;
    saving: boolean = false;

    _isActive: boolean;
    _workingDate;
    _qty;
    _qtyOld;
    _isDeleted;

    constructor(
        injector: Injector,
        private _service: InvCkdStockPartServiceProxy,
        private _component: StockPartComponent
    ) {
        super(injector);

    }

    show(rowdata: InvCkdStockPartDto): void {
        this.rowdata = rowdata;
        this._workingDate = rowdata.workingDate ? formatDate(new Date(rowdata.workingDate?.toString()), 'dd/MM/yyyy', 'en-US') : undefined;

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this._qty = this.rowdata.qty;
        this._qtyOld = this.rowdata.qty;
        this.modal.show();
    }

    checkVal(ev) {
        if (ev == '') this._qty = null;
    }

    checkActive(ev){
        if(!ev){
            this._qty = 0;
        }else{
            this._qty = this._qtyOld;
        }
    }

    save(): void {
        if (this._qty == null) {
            this.notify.warn('Qty cannot null!');
            document.getElementById('qty').focus();
            return;
        }
        if (isNaN(this._qty)) {
            this.notify.warn('Qty must be a number!');
            document.getElementById('qty').focus();
            return;
        }
        if (!this._isActive) {
            this.message.confirm(this.l('Please confirm inactive (obsolete) following part?'), '', (isConfirmed) => {
                if (isConfirmed) {
                    this.isLoading = true;
                    this._service.editCKDStockPart(this.rowdata.id, this._qty, this._isActive)
                        .pipe(finalize(() => this.isLoading = false))
                        .subscribe(() => {
                            this._component.searchDatas();
                            this.notify.info(this.l('SavedSuccessfully'));
                            this.close();
                            this.modalSave.emit(this.rowdata);
                        });
                }
            })
        }
        else {
            this.isLoading = true;
            this._service.editCKDStockPart(this.rowdata.id, this._qty, this._isActive)
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this._component.searchDatas();
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(this.rowdata);
                });
        }
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
