import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LotMakingTabletServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


@Component({
    selector: 'app-lotmakingtablet-modal',
    templateUrl: './lotmakingtablet-modal.component.html',
    styleUrls: ['./lotmakingtablet-modal.component.less']
})
export class LotMakingTabletModalComponent extends AppComponentBase {
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() close = new EventEmitter();
    @Output() end = new EventEmitter();

    isLoading: boolean = false;
    dataMkLotMaking: any[] = [];
    prod_line: string;
    lot_no: string;
    lot_id: number;
    loading: boolean;

    constructor(
        injector: Injector,
        private _service: LotMakingTabletServiceProxy,
    ) {super(injector);}
    ngOnInit() {
    }
    ngAfterViewInit() {
    }

    show(prodLine:string, lotNo:string,id:number) {
        this.prod_line = prodLine;
        this.lot_no = lotNo;
        this.lot_id = id;
        this.modal.show();
        this._service.getMkModuleListDataLotUpPlan(prodLine,lotNo,id,).pipe(finalize(() => {
            setTimeout(() => {
                this.loadData();
            }, 1000);
        })).subscribe((result) => {
            this.dataMkLotMaking = result.items ?? [];
        })
    }
    closeModal(): void {
        this.modal?.hide();
    }

    FinishLotMk(p_lot_id) {
        console.log(p_lot_id);

        this.isLoading = true;
        this._service.finishLotMk(p_lot_id).pipe(finalize(() => {
            this.isLoading = false;
        })).subscribe(() => {
            alert('Finnish Making Lot Up Plan Success');
            this.notify.info('Finnish Making Lot Up Plan Success');
            this.closeModal();
        })
    }

    loadData() {
        let htmListModule = document.querySelector('.modal-dialog .modal-content .modal-body .list-module');
        var data = this.dataMkLotMaking;
        var countLotMk = data.length;

        if (countLotMk > 0) {
            var htmModule = "";
            htmModule = '<div onclick="fun"  style="display: flex;font-weight: 900;"><div class="col-md-2" style="border-right:1px solid black;border-bottom:1px solid black">#</div><div class="col-md-5" style="border-right:1px solid black;border-bottom:1px solid black">Module</div><div class="col-md-5" style="border-bottom:1px solid black">Source</div></div>';
            for (var i = 0; i < countLotMk; i++) {
                htmModule = htmModule + '<div style="display: flex;"><div class="col-md-2" style="height: 25px;border-right:1px solid black;border-bottom:1px solid black">' + (i + 1) + '</div><div class="col-md-5" style="height: 25px;border-right:1px solid black;border-bottom:1px solid black">' + data[i].caseNo + '</div><div class="col-md-5" style="height: 25px;border-bottom:1px solid black">' + data[i].supplierNo + '</div></div>';
            }
            htmListModule.innerHTML = htmModule;
        }
    }
}
