import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportDrmPartListComponent } from './import-drmpartlist.component';
import { InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'list-error-import-drmpartlist',
    templateUrl: './list-error-import-drmpartlist-modal.component.html',
    styleUrls: ['./list-error-import-drmpartlist-modal.component.less'],
})
export class ListErrorImportDrmPartListComponent extends AppComponentBase {
    @ViewChild('errorListModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    data_ihp: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: InvDrmPartListServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _import: ImportDrmPartListComponent
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
                this.data = result.items.filter(a => a.drmOrIhp == 'DRM') ?? [];
                this.data_ihp = result.items.filter(b => b.drmOrIhp == 'IHP') ?? [];
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
