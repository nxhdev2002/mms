import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportSmqdOrderComponent } from './import-smqdorder-modal.component';

@Component({
    selector: 'list-error-import-smqdorder-modal',
    templateUrl: './list-error-import-smqdorder-modal.component.html',
    styleUrls: ['./list-error-import-smqdorder-modal.component.less'],
})
export class ListErrorImportSmqdOrderComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    guid;
    orderType: number;

    constructor(injector: Injector,
        private _service: InvCkdSmqdOrderServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _smqdOrder: ImportSmqdOrderComponent) {
        super(injector);
        }
    ngOnInit() {}

    show(guid): void {
        this.guid = guid;
        this._service.getMessageErrorImport(guid)
        .pipe(finalize(() => { }))
        .subscribe((result) => {
            this.data = result.items ?? [];
            console.log(this.data);
            
        });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._smqdOrder.close();
    }

    exportToExcel(): void {
        this._service.getSmqdOrderListErrToExcel(this.guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
