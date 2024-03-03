import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockReceivingTransactionServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportGpsStockReceivingTransactionComponent } from './import-stockreceivingtransaction.component';


@Component({
    selector: 'list-error-import-stockreceivingtransaction-modal',
    templateUrl: './list-error-import-stockreceivingtransaction-modal.component.html',
    styleUrls: ['./list-error-import-stockreceivingtransaction-modal.component.less'],
})
export class ListErrorImportGpsStockReceivingTransactionComponent extends AppComponentBase {
    @ViewChild('errorListModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: InvGpsStockReceivingTransactionServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _import: ImportGpsStockReceivingTransactionComponent
    ) {
        super(injector);
        { }
    }
    ngOnInit() { }

    show(guid): void {
        this.guid = guid;
        this._service.getMessageErrorImport(guid)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.data = result.items ?? [];
            });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._import.close();
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getListErrToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }
}
