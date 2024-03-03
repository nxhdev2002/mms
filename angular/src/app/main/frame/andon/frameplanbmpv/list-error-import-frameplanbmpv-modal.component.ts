import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPVServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { FramePlanBMPVComponent } from './frameplanbmpv.component';
import { ImportFramePlanBMPVComponent } from './import-frameplanbmpv-modal.component';

@Component({
    selector: 'list-error-import-frameplanbmpv-modal',
    templateUrl: './list-error-import-frameplanbmpv-modal.component.html',
    styleUrls: ['./list-error-import-frameplanbmpv-modal.component.less'],
})
export class ListErrorImportFramePlanBMPVComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: FrmAdoFramePlanBMPVServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _framePlanBMPVImport: ImportFramePlanBMPVComponent
        ) {
        super(injector);
        {}
        }
    ngOnInit() {}

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
        this._framePlanBMPVImport.close();
    }

    exportToExcel(): void {
        var v_guid = this.guid
        this._service.getListErrFramePlanBMPVToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
