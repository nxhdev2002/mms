import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerTransitPortPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ImportContainerTransitPortPlanComponent } from './import-containertransitportplan-modal.component';
import * as moment from 'moment';


@Component({
    selector: 'list-error-import-containertransitportplan-modal',
    templateUrl: './list-error-import-containertransitportplan-modal.component.html',
    styleUrls: ['./list-error-import-containertransitportplan-modal.component.less'],
})
export class ListErrorImportContainerRentalWHPlanComponent extends AppComponentBase {
    @ViewChild('modal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pending: string = '';
    disable: boolean = false;
    errCollection: any[] = [];

    data: any[] = [];
    guid;

    constructor(injector: Injector,
        private _service: InvCkdContainerTransitPortPlanServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _containerTransitPortPlanImport: ImportContainerTransitPortPlanComponent
    ) {
        super(injector);
        { }
    }
    ngOnInit() { }

    show(guid): void {
        this.guid = guid;
        this._service.getMessageErrorImportPortPlan(guid)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                result.items.forEach((item : any) => {
                    if (item.requestDate) {
                        item.requestDate = moment(item.requestDate).format('YYYY-MM-DD');
                    }
                    if (item.errorDescription.includes('!')) {
                     const formatResult = item.errorDescription.split('!')
                        .map(part => part.trim());
                     this.errCollection.push(formatResult.pop());
                     item.errorDescription = formatResult;
                    }
                    this.data = result.items ?? [];
                }) 
            });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._containerTransitPortPlanImport.close();
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getListErrContainerTransitPortPlanToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }
}
