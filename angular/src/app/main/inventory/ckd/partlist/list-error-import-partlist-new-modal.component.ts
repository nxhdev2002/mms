import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import {  InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


@Component({
    selector: 'list-error-import-partlist-new-modal',
    templateUrl: './list-error-import-partlist-new-modal.component.html',
    styleUrls: ['./list-error-import-partlist-new-modal.component.less'],
})
export class ListErrorImportInvCkdPartlistNewComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;
    screen;

    constructor(injector: Injector,
        private _service: InvCkdPartListServiceProxy,
        
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
        { }
    }
    ngOnInit() { }

    show(guid): void {
        this.guid = guid;
        //this.screen = screen
        this._service.getMessageErrorImport(guid,'')
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.data = result.items ?? [];
            });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getListErrPartListNormalToExcel(v_guid, '')
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });

    }
}
