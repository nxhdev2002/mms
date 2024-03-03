// import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
// import { AppComponentBase } from '@shared/common/app-component-base';
// import { InvCkdSmqdServiceProxy } from '@shared/service-proxies/service-proxies';
// import { FileDownloadService } from '@shared/utils/file-download.service';
// import { ModalDirective } from 'ngx-bootstrap/modal';
// import { finalize } from 'rxjs/operators';
// import { ImportInvCkdSmqdComponent } from './import-smqdmanagement.component';


// @Component({
//     selector: 'list-error-import-smqdmanagement',
//     templateUrl: './list-error-import-smqdmanagement-modal.component.html',
//     styleUrls: ['./list-error-import-smqdmanagement-modal.component.less'],
// })
// export class ListErrorImportInvCkdSmqdComponent extends AppComponentBase {
//     @ViewChild('errorListModal', { static: true }) modal: ModalDirective;
//     @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
//     @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

//     pending: string = '';
//     disable: boolean = false;

//     data: any[] = [];
//     guid;
//     type;

//     constructor(injector: Injector,
//         private _service: InvCkdSmqdServiceProxy,
//         private _fileDownloadService: FileDownloadService,
//         private _import: ImportInvCkdSmqdComponent
//     ) {
//         super(injector);
//         { }
//     }
//     ngOnInit() { }

//     show(guid,type): void {
//         this.guid = guid;
//         this.type = type;
//         this._service.getMessageErrorImport(guid)
//             .pipe(finalize(() => { }))
//             .subscribe((result) => {
//                 console.log(result);
                
//                 this.data = result.items ?? [];
//             });
//         this.modal.show();
//     }

//     close(): void {
//         this.modal.hide();
//         this.modalClose.emit(null);
//         this._import.close();
//     }

//     exportToExcel(): void {
//         this.pending = 'pending';
//         this.disable = true;
//         var v_guid =this.guid 
//         this._service.getListErrToExcel(v_guid,this.type)
//             .subscribe((result) => {
//                 this._fileDownloadService.downloadTempFile(result);
//                 this.pending = '';
//                 this.disable = false;
//             });
//     }
// }
