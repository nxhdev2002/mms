import { log } from 'console';
import { FileDownloadService } from './../../../../../shared/utils/file-download.service';
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaBp2PartListGradeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'bp2partlistgrade-export-modal',
    templateUrl: './bp2partlistgrade-export-modal.component.html',
    styleUrls: ['./bp2partlistgrade-export-modal.component.less'],
})
export class Bp2PartListGradeExportComponent extends AppComponentBase {
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('exportExcelModal', { static: true }) modal: ModalDirective;
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    states: string[] = [];
    selectedOption: string;
    formData: FormData = new FormData();
    fileName = '';
    urlFile = '';
    urlBase: string = AppConsts.remoteServiceBaseUrl;

    constructor(injector: Injector,
        private _service: MstLgaBp2PartListGradeServiceProxy,
        private _http: HttpClient,
        private _fileDownloadService: FileDownloadService
        ) {
        super(injector);
    }
    ngOnInit() {}
    ngAfterViewInit() {}

    closeModal(): void {
        this.modal?.hide();
    }
    show(): void {
        this.modal.show();
    }
    downLoadbyModel() {
        this._service.exportPartListGrade(this.selectedOption)
        .subscribe(blob => {
            this._fileDownloadService.downloadTempFile(blob);
        })
    }
}