
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { CustomColDef } from '@app/shared/common/models/base.model';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-request-modal',
    templateUrl: './request-modal.component.html',
    styleUrls: ['./request-modal.component.less']
})
export class RequestModalComponent   extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild("reportRequest2Modal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    cbbPeriod: any[] = [];
    periodColumdef: CustomColDef[] = [];
    ckbImportbyCont: boolean = false;
    ckbStock: boolean = false;
    ckbOutWipStock: boolean = false;
    ckbOutLineOff: boolean = false;
    ckbDetails: boolean = false;
    ckbNotificationEmail: boolean = false;
    txtPid: number = 0;
    txtPid_d125: number = 0;
    txtGrade: string = "";
    txtFrame: string = "";


    constructor(
      injector: Injector,
      private _httpClient: HttpClient,
      private _serviceStock: InvStockServiceProxy,
    ) {
      super(injector);
      this.periodColumdef = [
            {
                headerName: this.l('Id'),
                headerTooltip: this.l('Id'),
                field: 'id',
                width: 50,
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
        ]
      {}
    }
    ngOnInit() {
        this._serviceStock.getIdInvPeriod().subscribe((result) => {
            this.cbbPeriod = result.items;
        });
    }

    show(): void {
        this.modal.show();
    }

    close(): void {
        this.clearFormRequest();
        this.modal.hide();
        this.modalClose.emit(null);
    }

    clearFormRequest() {
        this.ckbImportbyCont = false,
        this.ckbStock = false,
        this.ckbOutWipStock = false,
        this.ckbOutLineOff = false,
        this.ckbDetails = false,
        this.ckbNotificationEmail = false,
		this.txtPid = 0,
		this.txtPid_d125 = 0,
		this.txtGrade = '',
		this.txtFrame = ''
    }


    onSubCkd(){
        this._serviceStock.reportRequest(
            String(this.ckbImportbyCont),
            String(this.ckbStock),
            String(this.ckbOutWipStock),
            String(this.ckbOutLineOff),
            String(this.ckbDetails),
            String(this.ckbNotificationEmail),
            this.txtPid,
            this.txtPid_d125,
            this.txtGrade,
            this.txtFrame
            ).pipe(finalize(() => {})).subscribe(() => {
                this.notify.info('Hệ thống sẽ xử lý và gửi lại kết quả');
                this.close();
            });

    }
  }



