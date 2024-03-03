import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFramePlanA1ServiceProxy, FrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPVServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportFramePlanA1Component } from './import-frameplana1-modal.component';

@Component({
    selector: 'list-error-import-frameplana1-modal',
    templateUrl: './list-error-import-frameplana1-modal.component.html',
    styleUrls: ['./list-error-import-frameplana1-modal.component.less'],
})
export class ListErrorImportFramePlanA1Component extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: FrmAdoFramePlanA1ServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _framePlanA1Import: ImportFramePlanA1Component
        ) {
        super(injector);
        {}
        }
    ngOnInit() {}

    show(guid): void {
        this.guid = guid;

        this._service.getMessageErrorFramePlanA1Import(guid)
        .pipe(finalize(() => { }))
        .subscribe((result) => {
            this.data = result.items ?? [];
        });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._framePlanA1Import.close();
    }

    exportToExcel(): void {
        var v_guid = this.guid
        this._service.getListErrFramePlanA1ToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
