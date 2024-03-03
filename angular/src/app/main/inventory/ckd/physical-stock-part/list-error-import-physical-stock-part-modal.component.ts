import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerRentalWHPlanServiceProxy, InvCkdPhysicalStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportInvCkdPhysicalStockPartComponent } from './import-physical-stock-part.component';


@Component({
    selector: 'list-error-import-physical-stock-part-modal',
    templateUrl: './list-error-import-physical-stock-part-modal.component.html',
    styleUrls: ['./list-error-import-physical-stock-part-modal.component.less'],
})
export class ListErrorImportInvCkdPhysicalStockPartComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;

    data: any[] = [];
    guid;
    screen;

    constructor(injector: Injector,
        private _service: InvCkdPhysicalStockPartServiceProxy,
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

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getListErrToExcel(v_guid,this.screen)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }
}
