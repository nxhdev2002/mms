import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPVServiceProxy, WldAdoWeldingPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { ImportWeldingPlanComponent } from './import-weldingplan-modal.component';


@Component({
    selector: 'list-error-import-weldingplan-modal',
    templateUrl: './list-error-import-weldingplan-modal.component.html',
})
export class ListErrorImportWeldingPlanComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: WldAdoWeldingPlanServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _frameWeldingPlanImport: ImportWeldingPlanComponent
        ) {
        super(injector);
        {}
        }
    ngOnInit() {}

    show(guid): void {
        this.guid = guid;

        this._service.getMessageErrorWeldingPlanImport(guid)
        .pipe(finalize(() => { }))
        .subscribe((result) => {
            this.data = result.items ?? [];
        });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._frameWeldingPlanImport.close();
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getListErrWeldingPlanToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }
}
