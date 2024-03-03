import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetIF_FundCommitmentItemDMExportDto, IF_FQF3MM07ServiceProxy, InvCkdContainerListServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import ceil from 'lodash-es/ceil';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs';
import * as vkbeautify from 'vkbeautify';
import { Pipe, PipeTransform } from "@angular/core";


@Component({
    templateUrl: './view-request-budget-check-modal.component.html',
    selector: 'view-request-budget-check-modal'
})
export class ViewRequestBudgetCheckComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewFundCommitmentItem', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];

    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    request: string = '';
    parser = new DOMParser();
    logId: number;
    xmlDoc;
    response: string = '';
    creationTime;
    constructor(injector: Injector,
        private _service: IF_FQF3MM07ServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    ngOnInit(): void {}

    show(logId): void {
        if(logId){
            console.log('getViewRequestBudgetCheck')
            this._service.getViewRequestBudgetCheck(logId)
            .subscribe((result) => {
                this.request = result.request;
                this.creationTime = result.creationTime_DDMMYYYY;
                this.response = result.response;
            });
        this.modal.show();
        }
       
    }

    transform(value: string): string {
        if(value != '') return vkbeautify.xml(value);
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
