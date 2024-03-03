import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


@Component({
    selector: 'list-error-import-part-list-modal',
    templateUrl: './list-error-import-part-list-modal.component.html',
    styleUrls: ['./list-error-import-part-list-modal.component.less'],
})
export class ListErrorImportGpsPartListComponent extends AppComponentBase {
    @ViewChild('listErorrmodal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;
    screen;

    constructor(injector: Injector,
        private _service: InvGpsPartListServiceProxy,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
        { }
    }
    ngOnInit() { }

    show(guid,screen): void {
        this.guid = guid;
        this.screen = screen
        this._service.getMessageErrorImport(guid,screen)
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
    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void { 

        this.fn.exportLoading(e, true);
		var v_guid = this.guid;
        this._service.getListErrToExcel(v_guid,this.screen)
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }
}
