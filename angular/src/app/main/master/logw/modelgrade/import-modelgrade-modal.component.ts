
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwModelGradeServiceProxy, MstLgwUnpackingPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { ModelGradeComponent } from './modelgrade.component';



@Component({
  selector: 'import-modelgrade-modal',
  templateUrl: './import-modelgrade-modal.component.html',
  styleUrls: ['./import-modelgrade-modal.component.less']
})
export class ImportModelGradeComponent   extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild("importModal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];

    formData: FormData = new FormData();
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;

    constructor(
      injector: Injector,
      private _httpClient: HttpClient,
      private _service:  MstLgwModelGradeServiceProxy,
      private _mstlgwmodelgrade: ModelGradeComponent
    ) {
      super(injector);
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportMstLgwModelGradeFromExcel';
      {}
    }
    ngOnInit() {

    }

    show(): void {
        this.fileName = '';
        this.modal.show();
    }

    close(): void {
        this.InputVar.nativeElement.value = "";
        this.fileName = '';
        this.formData = new FormData();
        let viewName = document.getElementById("viewNameFileImport");
        if ( viewName!= null ){
            viewName.innerHTML = "";
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.isLoading = false;
    }

    onUpload(data: { target: { files: Array<any> } }): void {
        if (data?.target?.files.length > 0) {
            this.formData = new FormData();
            const formData: FormData = new FormData();
            const file = data?.target?.files[0];
            this.fileName = file?.name;
            formData.append('file', file, file.name);
            this.formData = formData;

            //view name file import
            let viewName = document.getElementById("viewNameFileImport");
            if ( viewName!= null ){
                viewName.innerHTML = this.fileName.toString();
                viewName.style.color = "red";
            }
        }
    }


    upload() {
            if(this.fileName != ''){
                this.isLoading = true;
                this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                    this.isLoading = true;
                }))
                .subscribe(response => {
                    if (response.success ) {
                        this.mergeUnpackingPart(response.result.unpackingPart.result[0].guid);
                    } else if (response.error != null || !response.result.unpackingPart.result) {
                        this.notify.warn('Dữ liệu không hợp lệ');
                        this.progressBarHidden()
                    }
                });
            }
    }


    mergeUnpackingPart(guid) {
        this.isLoading = true;
        this._service.mergeDataMstLgwModel(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.notify.info('Lưu thành công');
                this.close();
                this._mstlgwmodelgrade.searchDatas();
            });
    }

    refresh() {

            this.InputVar.nativeElement.value = "";
            this.fileName = '';
            this.formData = new FormData();
            this.InputVar.nativeElement.click();
    }
    progressBarVisible(){
        var VissibleProgressBar = document.getElementById("loader-line" ).style.visibility = "visible";
        var viewName = document.getElementById('viewNameFileImport').style.visibility = 'hidden';

        var hideButtom = document.querySelectorAll<HTMLElement>('.modal-body .ui-md-12');
        (hideButtom[0] as HTMLElement).style.visibility = 'hidden';

    }
    progressBarHidden(){
        var HiddenProgressBar = document.getElementById("loader-line").style.visibility = "hidden"
        var viewName = document.getElementById('viewNameFileImport').style.visibility = 'visible'
        var hideButtom = document.querySelectorAll<HTMLElement>('.modal-body .ui-md-12');
        (hideButtom[0] as HTMLElement).style.visibility = 'visible';

    }

  }


