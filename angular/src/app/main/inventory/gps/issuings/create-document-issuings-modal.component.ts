import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetIssuingImportView, InvGpsIssuingsDetails, InvGpsIssuingsServiceProxy, OnlineBudgetCheckRequest, SapIFServiceProxy } from '@shared/service-proxies/service-proxies';
import { IssuingsComponent } from './issuings.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { Tree } from 'primeng/tree';


@Component({
    selector: 'create-document-issuings-modal',
    templateUrl: './create-document-issuings-modal.component.html',
    styleUrls: ['./create-document-issuings-modal.component.less']
})
export class CreateDocumentIssuingsModalComponent extends AppComponentBase {
    @ViewChild('createDocumentIssuing', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('confirmDialog', { static: true }) confirmDialog: | ConfirmDialogComponent | undefined;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    active: boolean = false;
    saving: boolean = false;
    isCheckBudget: string = '';
    isBudgetSuccess: boolean = false;
    documentNo;
    documentDate;
    shop;
    status;
    costCenter;
    _project;
    listCostCenter = [{ value: '', label: '' }];;
    rowData: GetIssuingImportView = new GetIssuingImportView();
    constructor(
        injector: Injector,
        private _gpsIssuing: InvGpsIssuingsServiceProxy,
        private _sapService: SapIFServiceProxy,
        private _service: IssuingsComponent
    ) {
        super(injector);
    }

    ngOnInit() {
        this.listCostCenter = [];
        this._gpsIssuing.getListCostCenter().subscribe((result) => {
            result.map(e => {
                this.listCostCenter.push({ value: e.costCenter, label: e.costCenter })
            })
        });

    }

    show(rowdata?: GetIssuingImportView): void {
        this._gpsIssuing.getIssuingImportView('').subscribe((e) => {
            this.rowData = e;
        })
        this.active = true;
        this.modal.show();
    }

    onChangeCostCenter(v_costCenter) {
        this._gpsIssuing.getIssuingImportView(v_costCenter).subscribe((e) => {
            this.rowData.documentNo = e.documentNo;
        })
    }


    fn: CommonFunction = new CommonFunction(); 
    isSubmut:boolean = false;
    submit(e) {

        // if(this.isSubmut) return;
        // this.isSubmut = true;
        this.fn.exportLoading(e, true); //disable nút create
             

        if (this._project) {
            if (this._project.toString().length != 24) {
                this.confirmDialog.show("Waring", "Wbs không chính xác");
                this.fn.exportLoading(e); //Show nút create
                // this.isSubmut = false;
                return;
            }
            else {
                var month = Number(new Date().getMonth() + 1);
                var year = Number(new Date().getFullYear());
                var _fiscalYear = month > 4 ? (year + 1) : year;

                let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                    wbsMasterData: this._project,
                    fiscalYear: _fiscalYear.toString(),
                    costCenter: this.rowData.costCenter,
                    fixedAssetNo: '', // fix 
                    system: 'MMS', // fix cứng
                    listItemId: [] // chỉ 1 item
                })
 
                this._sapService.sapOnlineBudgetCheck(obj)
                    .pipe(finalize(() => { }))
                    .subscribe((budget) => { 
                        if (budget.response?.dataValidation.dataValidationResult == 'T') {
                            this.save(e);
                        }
                        else {
                            this.confirmDialog.show("Waring", "Wbs hoặc Cost Center không chính xác"); 
                            this.fn.exportLoading(e); //Show nút create
                            // this.isSubmut = false;
                        } 
                    })
            }
        } else { 
            this.save(e);
        }
    }

    save(e): void {
 console.log(this.rowData.shop);
        var v_shop = this.rowData.shop == undefined ? '' : this.rowData.shop
 
        this._gpsIssuing.createDocumentRequest(v_shop, this.rowData.costCenter, this._project)
            .pipe(finalize(() => {
                this.fn.exportLoading(e); //Show nút create
                // this.isSubmut = false;
            }))
            .subscribe((res) => {
                this.notify.success(this.l('Create Request Successfully'));
                this.fn.exportLoading(e); //Show nút create
                // this.isSubmut = false;
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowData);
                this._service.searchDatas();
            });
        
    }

    close(): void {
        this.active = false;
        this._project = '';
        this.confirmDialog.Cancel();
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
