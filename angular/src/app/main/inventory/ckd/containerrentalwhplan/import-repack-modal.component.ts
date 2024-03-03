
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { InvCkdContainerRentalWHPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { ContainerRentalWHPlanComponent } from './containerrentalwhplan.component';
import { ListErrorImportContainerRentalWHPlanComponent } from './list-error-import-containerrentalwhplan-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
  selector: 'import-repack-modal',
  templateUrl: './import-repack-modal.component.html',
  styleUrls: ['../../../import-modal.less'],
})
export class ImportRepackComponent   extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importRepackModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorRepackListModal', { static: true }) errorRepackListModal: ListErrorImportContainerRentalWHPlanComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    fn: CommonFunction = new CommonFunction();

    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];

    vissibleProgess;
    vissbleInputName;

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;

    constructor(
      injector: Injector,
      private _httpClient: HttpClient,
      private _service: InvCkdContainerRentalWHPlanServiceProxy,
      private _contRental: ContainerRentalWHPlanComponent,
      private _fileDownloadService: FileDownloadService,
    ) {
      super(injector);
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportInvCkdContainerRepackTransferFromExcel';
      {}
    }
    ngOnInit() {

    }

    show(): void {
        this.progressBarHidden();
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this.modal.show();
    }

    close(): void {
        this.InputVar.nativeElement.value = '';
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImport');
        if (viewName != null) {
                viewName.innerHTML = '';
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.fileName = '';

    }

    onUpload(data: { target: { files: Array<any> } }): void {
        if (data?.target?.files.length > 0) {
            this.formData = new FormData();
            const formData: FormData = new FormData();
            const file = data?.target?.files[0];
            this.fileName = file?.name;
            formData.append('file', file, file.name);
            this.formData = formData;


            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }
    }


    upload() {

        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    console.log(response);
                    if (response.success) {
                        this.notify.info('Lưu thành công');
                        this.modal.hide();
                        this.modalClose.emit(null);
                        this._contRental.searchDatasRepack();
                        this.close();
                    } else {
                        this.exeptionDataImport();
                    }
                },
                (error) => {
                    this.exeptionDataImport();
                });
        }else{
            this.notify.warn('Vui lòng chọn file');
        }

    }

    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close();
    }

    // mergeContainerRentalWHPlan(guid) {
    //     this.isLoading = true;
    //     this._service
    //         .mergeDataContainerRentalWHPlan(guid)
    //         .pipe(
    //             finalize(() => {
    //                 this.isLoading = false;
    //             })
    //         )
    //         .subscribe(() => {
    //             this.showMessImport(guid);
    //         });
    // }


    showMessImport(guid){
        this._service.getMessageErrorImport(guid)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorRepackListModal.show(guid)
            }
            else{
                this.notify.info('Lưu thành công');
                this.modal.hide();
                this.modalClose.emit(null);
                this._contRental.searchDatas();
                this.close();
            }
        });
    }

    progressBarVisible() {

        this.vissbleInputName = 'vissible';
        this.vissibleProgess = 'vissible active';

    }
    progressBarHidden() {

        this.vissibleProgess = '';
        this.vissbleInputName = '';
    }

    exportRepackTemplate(e) {
        this.fn.exportLoading(e, true);
        this._service.getAllRepackToExcel()
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
        }

  }
