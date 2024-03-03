import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdOrderLeadTimeServiceProxy, InvCkdSmqdOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { SmqdOrderLeadTimeComponent } from './smqdorderleadtime.component';
import { ImportSmqdOrderLeadTimeComponent } from './import-smqdorderleadtime-modal.component';

@Component({
    selector: 'list-error-import-smqdorder-modal',
    templateUrl: './list-error-import-smqdorderleadtime-modal.component.html',
    styleUrls: ['./list-error-import-smqdorderleadtime-modal.component.less'],
})
export class ListErrorImportSmqdOrderLeadTimeComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    guid;
    orderType: number;


    constructor(injector: Injector,
        private _service: InvCkdSmqdOrderLeadTimeServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _smqdOrder: ImportSmqdOrderLeadTimeComponent) {
        super(injector);
        }
    ngOnInit() {}

    show(guid): void {
        this.guid = guid;
        this._service.getMessageErrorImportOrderLeadTime(guid)
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
        this._service.getSmqdOrderLeadTimeListErrToExcel(this.guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
