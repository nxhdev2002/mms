import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdProductionMappingServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportProductionMappingComponent } from './import-production-mapping.component';


@Component({
    selector: 'list-error-import-production-mapping',
    templateUrl: './list-error-import-production-mapping-modal.component.html',
    styleUrls: ['./list-error-import-production-mapping-modal.component.less'],
})
export class ListErrorImportProductionMappingComponent extends AppComponentBase {
    @ViewChild('errorListModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: InvCkdProductionMappingServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _import: ImportProductionMappingComponent
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

                console.log(this.data[0])
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
