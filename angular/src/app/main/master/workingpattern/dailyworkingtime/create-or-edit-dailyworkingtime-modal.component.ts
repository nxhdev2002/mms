import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptDailyWorkingTimeDto, MstWptDailyWorkingTimeDto, MstWptDailyWorkingTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-dailyworkingtime-modal',
    templateUrl: './create-or-edit-dailyworkingtime-modal.component.html',
})
export class CreateOrEditDailyWorkingTimeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalDailyWorkingTime', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstWptDailyWorkingTimeDto = new CreateOrEditMstWptDailyWorkingTimeDto();
    selectedRow: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    saveSelectedRow: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    active: boolean = false;
    saving: boolean = false;
    _isManual: boolean;
    _isActive: boolean;
    pDList: any[] = [];
    sList: any[] = [];
    wTList: any[] = [];
    listSelected: any[] = [];
    shopColdef: any[] = [];
    shiftColdef: any[] = [];
    wtColdef: any[] = [];
    shiftNo: number;
    shiftName: string = '';
    workingType: number;
    gridApi!: GridApi;
    shopName: string = '';
    ListSelectedshopName: any[] = [];
    listShopId: string = '';
    shiftNo_shiftName: string = '';
    wtype_des_find: string = '';
    wtype_des: string = '';
    _workingDate: any;




    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };


    constructor(
        injector: Injector,
        private _service: MstWptDailyWorkingTimeServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.shiftColdef = [
            {
                headerName: 'Shift No',
                headerTooltip: 'Shift No',
                field: 'shiftNo',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Shift Name',
                headerTooltip: 'Shift Name',
                field: 'shiftName',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Start Time',
                headerTooltip: 'Start Time',
                field: 'startTime',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'End Time',
                headerTooltip: 'End Time',
                field: 'endTime',
                cellClass: ['text-center'],
                flex: 1
            },
        ];

        this.wtColdef = [
            {
                headerName: 'Working Type',
                headerTooltip: 'Working Type',
                field: 'workingType',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Is Active',
                headerTooltip: 'Is Active',
                field: 'isActive',
                cellClass: ['text-center'],
                flex: 1
            },
        ];

    }

    show(rowdata?: CreateOrEditMstWptDailyWorkingTimeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptDailyWorkingTimeDto();
        else this.rowdata = rowdata;
        this.shiftNo_shiftName = this.rowdata.shiftNo + ' - ' + this.rowdata.shiftName;

        const dateValue = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        this._isManual = (this.rowdata.isManual == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this._service.mstWptPatternD_GetsActive()
            .subscribe((result) => {
                this.pDList = result.items;
            });

        this._service.mstWptShop_GetsActive()
            .subscribe((result) => {
                this.sList = result.items;
            });

        this._service.mstWptWorkingType_GetsForDaily()
            .subscribe((result) => {
                this.wTList = result.items;
                this.wtype_des_find = this.wTList.find(e => e.id === this.rowdata.workingType);
                this.wtype_des = this.wtype_des_find['id'] + ' - ' + this.wtype_des_find['description'];
            });

        this.active = true;
        this.modal.show();
    }

    changeManual(event) {
        this._isManual = event.target.checked;
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.listShopId = this.ListSelectedshopName.map(x => x.id).join(",");
        this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
            this.rowdata.shiftName = this.listShopId;
        this.rowdata.isManual = (this._isManual == true) ? 'Y' : 'N'
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._service.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
            });
        this.saving = false;
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);

    }
    clearText() {
        this.shiftNo = 0,
            this.workingType = 0;

    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
            this.clearText();
        }
    }
}
