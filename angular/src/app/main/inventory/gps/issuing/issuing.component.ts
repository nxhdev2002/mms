import { CellClickedEvent, GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent, AgCellEditorParams } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsIssuingDto, InvGpsIssuingServiceProxy, OnlineBudgetCheckRequest, SapIFServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditIssuingModalComponent } from './create-or-edit-issuing-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportInvGpsIssuingComponent } from './import-issuing-modal.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { AgCheckboxRendererComponent } from '@app/shared/common/grid/ag-checkbox-renderer/ag-checkbox-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { PermissionCheckerService } from 'abp-ng2-module';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';



@Component({
    templateUrl: './issuing.component.html',
    styleUrls: ['./issuing.component.less'],
})
export class IssuingComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalStockIssuing', { static: true }) createOrEditModalStockIssuing: | CreateOrEditIssuingModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importInvGpsStockIssuing', { static: true }) importInvGpsStockIssuing: | ImportInvGpsIssuingComponent | undefined;
    permission: PermissionCheckerService;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    listSelectedId;
    listDisplayGroup: any[] = []
    selectedRow: InvGpsIssuingDto = new InvGpsIssuingDto();
    saveSelectedRow: InvGpsIssuingDto = new InvGpsIssuingDto();
    datas: InvGpsIssuingDto = new InvGpsIssuingDto();
    isLoading: boolean = false;
    isBudgetSuccess: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataCheck: string = '';
    isCheckBudget: string = '';
    isIssuingBiger: string = '';
    partNo: string = '';
    partName: string = '';
    oum: string = '';
    boxqty
    box
    qty
    dateCurent = new Date();
    todayfilter: any;
    lotNo: string = '';
    prodDate
    expDate
    requestDate
    supplier: string = '';
    costCenter
    qtyIssue
    isIssue: string = '';
    status: string = '';
    isGentani: boolean = false;
    isGentaniHeader: boolean = true;
    selectedValue;
    reqDateFrom: any;
    reqDateTo: any;
    issueDateFrom: any;
    issueDateTo: any;
    statusFilter: any;
    statuslist = [
        { value: 'REQUESTED', label: "REQUESTED" },
        { value: 'ISSUED', label: "ISSUED" },
    ];
    checkbox: boolean = false;
    isConfirmAll: boolean = false;
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

    date = new Date();
    _permiss_GPS: any;
    _permiss_SHOP: any;
    selectReq;
    listRequest = [];
    dataParamsLotCodeGrade: GridParams | undefined;
    // RestrictList = [
    //     { key: 'Y', value: "REQUESTED" },
    //     { key: 'N', value: "ISSUED" },
    // ];
    listIdStatus = '';
    arrIdStatus: InvGpsIssuingDto[] = [];
    constructor(
        injector: Injector,
        private _service: InvGpsIssuingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private dataFormatService: DataFormatService,
        private _sapService: SapIFServiceProxy

    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                flex: 1
            },
            {
                headerName: this.l('Uom'),
                headerTooltip: this.l('Uom'),
                field: 'uom',
                flex: 1
            },
            {
                headerName: this.l('Box Qty'),
                headerTooltip: this.l('Box Qty'),
                field: 'boxqty',
                flex: 1
            },
            {
                headerName: this.l('Box'),
                headerTooltip: this.l('Box'),
                field: 'box',
                flex: 1
            },

            {
                headerName: this.l('Request Date'),
                headerTooltip: this.l('Request Date'),
                field: 'receivedDate',
                flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.receivedDate, 'dd/MM/YYYY')
            },
            {
                headerName: this.l('Qty Request'),
                headerTooltip: this.l('Qty  Request'),
                field: 'qtyRequest',
                editable: (params) => {
                    if (params.data.status == 'REQUESTED' && params.data.isGentani == 'N' && this.permission.isGranted('Pages.GPS.Issuing.ImportShop')) return true;
                    else return false;
                },
                flex: 1
            },
            {
                headerName: this.l('Qty Issue'),
                headerTooltip: this.l('Qty Issue'),
                field: 'qtyIssue',
                editable: this.permission.isGranted('Pages.GPS.Issuing.ConfirmGps'),
                cellStyle: params => {
                    if (params.data.status == 'ISSUED' && params.data.qtyIssue > params.data.qtyRequest) {
                        return { 'background-color': "yellow", 'border-bottom-color': "#CCCFD1" };
                    }
                },
                flex: 1
            },
            {
                headerName: this.l('Issue Date'),
                headerTooltip: this.l('Issue Date'),
                field: 'issueDate',
                width: 140,
                editable: (params) => {
                    if (params.data.status == 'ISSUED' && params.data.isGentani == 'N' && this.permission.isGranted('Pages.GPS.Issuing.ConfirmGps')) return true;
                    else return false;
                },
                valueGetter: params => this.dataFormatService.dateFormat(params.data.issueDate),
                // valueGetter: (params) => this.pipe.transform(params.data?.issueDate, 'dd/MM/YYYY'),
                cellClass: ['cell-clickable', 'cell-border'],
                cellRenderer: 'agDatepickerRendererComponent',

            },

            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1,
            },
            {
                headerName: "",
                headerTooltip: "",
                field: 'status',
                headerClass: ["align-checkbox-header"],
                headerCheckboxSelection: this.permission.isGranted('Pages.GPS.Issuing.ConfirmGps'),
                headerCheckboxSelectionFilteredOnly: true,
                disableCheckbox: (params) => {
                    if (params.data.isGentani == 'Y' || this.permission.isGranted('Pages.GPS.Issuing.ImportShop')) return true;
                    else return false;
                },
                cellRenderer: 'agCheckboxRendererComponent',
                data: ['ISSUED', 'REQUESTED'],
                cellClass: ["text-center"],
                width: 40,
                // onCellClicked: (event: CellClickedEvent) => this.onCellClick(event),
            },
            {
                headerName: this.l('Cost Center'),
                headerTooltip: this.l('Cost Center'),
                field: 'costCenter',
                flex: 1
            },
            {
                headerName: this.l('Is Gentani'),
                headerTooltip: this.l('Is Gentani'),
                field: 'isGentani',
                flex: 1

            },
            {
                headerName: this.l('Supplier'),
                headerTooltip: this.l('Supplier'),
                field: 'supplier',
                flex: 1,
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex: 1,
            },
            {
                headerName: this.l('Prod Date'),
                headerTooltip: this.l('Prod Date'),
                field: 'prodDate',
                flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.prodDate, 'dd/MM/YYYY')
            },
            {
                headerName: this.l('Exp Date'),
                headerTooltip: this.l('Exp Date'),
                field: 'expDate',
                flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.expDate, 'dd/MM/YYYY')
            },
     
        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
            agCheckboxRendererComponent: AgCheckboxRendererComponent,
            agDatepickerRendererComponent: AgDatepickerRendererComponent,
        };

        this.resetGridView();

    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 100, totalCount: 0 };
        this._permiss_GPS = this.permission.isGranted('Pages.GPS.Issuing.ConfirmGps');
        this._permiss_SHOP = this.permission.isGranted('Pages.GPS.Issuing.ImportShop')
        this._service.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG').subscribe((e) => {
            this.dataCheck = e.itemValue;
        });
        this._service.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG_2').subscribe((e) => {
            this.isCheckBudget = e.itemValue;
        });
        this._service.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG_1').subscribe((e) => {
            this.isIssuingBiger = e.itemValue;
        });
        this.todayfilter = null;
    }

    searchbyStatus(s) {
        this.statusFilter = (s === 'R') ? 'REQUESTED' : 'ISSUED'
        this.searchDatas();
    }

    searchToDayDate() {
        this.todayfilter = this.dateCurent;
        this.searchDatas();
    }
    onCellValueChanged(event) {
        if (event.newValue != event.oldValue) {
            this.updateCellData(event)
            this.resetGridView()
        }
    }

    // onCellClick(params) {
    //     console.log('cellclick');
    //     this.updateCellData(params.data)
    // }

    // cellEditingStopped(params: AgCellEditorParams) {
    //     console.log('cellEditing');
    //     this.updateCellData(params.data)

    // }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        const allColumnIds2: string[] = [];
        this.dataParamsLotCodeGrade.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });

        this.dataParamsLotCodeGrade.columnApi!.autoSizeColumns(allColumnIds);
        this.resetGridView();
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsLotCodeGrade.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },100)
    }

    updateCellData(event) {
        var cellParams = event.data;

        var month = Number(new Date().getMonth() + 1);
        var year = Number(new Date().getFullYear());
        var _fiscalYear = month > 4 ? (year + 1) : year;





        if (event.column.colId == 'status' && event.data.status == 'REQUESTED') {
            var listIssuing = [];
            listIssuing = this.listRequest
            this.listRequest = [];
            listIssuing.filter(x => x.id != event.data.id).map(res => {
                this.listRequest.push(res);

            });
            if (listIssuing.filter(x => x.id == event.data.id).length > 0) {
                // this.saveSelectedRow.status = "REQUESTED";
                // this.saveSelectedRow.issueDate = null;
                // this.saveSelectedRow.qtyIssue = null;
                // console.log(event.data.id + '---' + this.saveSelectedRow.issueDate);

                const rowData1 = this.rowData.map(res => {
                    this.saveSelectedRow.status = "REQUESTED";
                    this.saveSelectedRow.issueDate = null;
                    this.saveSelectedRow.qtyIssue = null;
                    return res;
                });
                this.dataParams.api.setRowData(rowData1)
            }
            else {
                this.saveSelectedRow.status = "ISSUED";
            }

        }
        else {
            this.message.confirm("", "Are you sure the selected item can be delivered ?",
                (isConfirmed) => {
                    if (isConfirmed) {

                        if (this.isGentani != true && this._permiss_GPS) {
                            var issueDate = this.pipe.transform(cellParams.issueDate, 'yyyy-MM-dd');
                            var today = this.pipe.transform(this.dateCurent, 'yyyy-MM-dd');

                            if ((issueDate >= today || issueDate == null)) {
                                if (cellParams.qtyIssue != null) {
                                    if (this.isIssuingBiger != 'Y' && (Number(cellParams.qtyIssue) <= cellParams.qtyRequest)) {
                                        this.saveSelectedRow.qtyIssue = Number(cellParams.qtyIssue)
                                    }
                                    else if (this.isIssuingBiger != 'Y' && (Number(cellParams.qtyIssue) > cellParams.qtyRequest)) {
                                        this.message.info(this.l('IssuingQty cannot be greater than RequestQty!'));
                                        this.searchDatas();
                                        return
                                    }
                                    else if (this.isIssuingBiger === 'Y') {
                                        this.saveSelectedRow.qtyIssue = Number(cellParams.qtyIssue)
                                    }

                                }
                                else {
                                    this.saveSelectedRow.qtyIssue = cellParams.qtyRequest
                                }

                                this.saveSelectedRow.status = "ISSUED";
                                this.saveSelectedRow.issueDate = (issueDate == null)
                                    ? this._dateTimeService.convertToDatetime(this.dateCurent)
                                    : this._dateTimeService.convertToDatetime(cellParams.issueDate);

                                //check budget 
                                if (this.isCheckBudget === 'Y') {
                                    console.log('check budget');

                                    const dataCheckBudget = [];
                                    let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                                        wbsMasterData: 'C6.IA005.HSED.16000.STWP',
                                        fiscalYear: _fiscalYear.toString(),
                                        costCenter: 'S1PA10',
                                        fixedAssetNo: '',
                                        system: 'MMS',
                                    })
                                    dataCheckBudget.push(obj);

                                    //this._sapService.sapOnlineBudgetCheck(dataCheckBudget[0])
                                    //    .subscribe((budget) => {
                                       //     console.log(budget);
                                         //   this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false 
                                            this.isBudgetSuccess = true;
                                            if (this.isBudgetSuccess) { // BudgetSuccess 
                                                this._service.createOrEdit(this.saveSelectedRow)
                                                    .pipe(finalize(() => this.isLoading = false))
                                                    .subscribe((result) => {
                                                        console.log(result);
                                                        
                                                        var listId = [];
                                                        listId.push(event.data.id);
                                                        console.log(listId);
                                                       // this._sapService.submitFundCommitmentToSap('100',event.data.id,true,listId)
                                                       // .subscribe((sub) => {
                                                      //      console.log('submitFundCommitment');                           
                                                      //      console.log(sub);
                                                            
                                                      //  })
                                                        this.notify.info(this.l('SavedSuccessfully'));
                                                        this.searchDatas();
                                                    });
                                            }
                                            else { // Budget not Success 
                                                this.notify.info(this.l('Budget is not enough'));
                                                this.searchDatas();
                                                return
                                            }
                                      //  })
                                }
                                else { //not check budget
                                    console.log('not check budget');
                                    this._service.createOrEdit(this.saveSelectedRow)
                                        .pipe(finalize(() => this.isLoading = false))
                                        .subscribe(() => {
                                            this.notify.info(this.l('SavedSuccessfully'));
                                            this.searchDatas();
                                        });
                                }



                            } else if (issueDate < today) {
                                this.saveSelectedRow.issueDate = this._dateTimeService.convertToDatetime(cellParams.issueDate);

                                if (this.dataCheck == 'Y') {
                                    this.saveSelectedRow.qtyIssue = Number(cellParams.qtyIssue);
                                    this.saveSelectedRow.status = "ISSUED";

                                    if (this.isCheckBudget === 'Y') {  //check budget 
                                        console.log('check budget');
                                        const dataCheckBudget = [];
                                        let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                                            wbsMasterData: 'C6.IA005.HSED.16000.STWP',
                                            fiscalYear: _fiscalYear.toString(),
                                            costCenter: 'S1PA10',
                                            fixedAssetNo: '',
                                            system: 'MMS',
                                        })
                                        dataCheckBudget.push(obj);

                                      //  this._sapService.sapOnlineBudgetCheck(dataCheckBudget[0])
                                         //   .subscribe((budget) => {
                                             //   this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false 
                                                this.isBudgetSuccess = true;
                                                if (this.isBudgetSuccess) { // BudgetSuccess 
                                                    this._service.createOrEdit(this.saveSelectedRow)
                                                        .pipe(finalize(() => this.isLoading = false))
                                                        .subscribe(() => {
                                                            var listId = [];
                                                            listId.push(event.data.id);
                                                            console.log(listId);
                                                            
                                                           // this._sapService.submitFundCommitmentToSap('100',event.data.id,true,listId)
                                                          //  .subscribe((sub) => {
                                                                
                                                         //   })
                                                           // this.subFundCommitmentToSap('','',null)
                                                            this.notify.info(this.l('SavedSuccessfully'));
                                                            this.searchDatas();
                                                        });
                                                }
                                                else { // Budget not Success 
                                                    this.notify.info(this.l('Budget is not enough'));
                                                    this.searchDatas();
                                                    return
                                                }
                                           // })
                                    }
                                    else { //not check budget
                                        console.log('not check budget');

                                        this._service.createOrEdit(this.saveSelectedRow)
                                            .pipe(finalize(() => this.isLoading = false))
                                            .subscribe(() => {
                                                this.notify.info(this.l('SavedSuccessfully'));
                                                this.searchDatas();
                                            });
                                    }
                                } else {
                                    this.message.info(this.l('Editing is not allowed!'));
                                    this.searchDatas();
                                    return
                                }
                            } else {
                                this.message.info(this.l('Editing is not allowed!'));
                                this.searchDatas();
                                return
                            }
                        }
                        else if (this._permiss_SHOP && this.saveSelectedRow.status == 'REQUESTED') {
                            this._service.createOrEdit(this.saveSelectedRow)
                                .pipe(finalize(() => this.isLoading = false))
                                .subscribe(() => {
                                    this.notify.info(this.l('SavedSuccessfully'));
                                    this.searchDatas();
                                });
                        }
                    }
                }
            )
        }


    }

    searchDatas(): void {

        this.isGentaniHeader = this.isGentani == true ? false : true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.reqDateFrom),
            this._dateTimeService.convertToDatetime(this.reqDateTo),
            this._dateTimeService.convertToDatetime(this.issueDateFrom),
            this._dateTimeService.convertToDatetime(this.issueDateTo),
            this._dateTimeService.convertToDatetime(this.todayfilter),
            this.statusFilter,
            this.isGentani == true ? 'Y' : 'N',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe()
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });

            

    }

    clearTextSearch() {
        this.partNo = '',
            this.lotNo = '',
            this.reqDateFrom = '',
            this.reqDateTo = '',
            this.issueDateFrom = '',
            this.issueDateTo = '',
            this.statusFilter = '',
            this.todayfilter = null,
            this.isGentani = false,
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.reqDateFrom),
            this._dateTimeService.convertToDatetime(this.reqDateTo),
            this._dateTimeService.convertToDatetime(this.issueDateFrom),
            this._dateTimeService.convertToDatetime(this.issueDateTo),
            this._dateTimeService.convertToDatetime(this.todayfilter),
            this.statusFilter,
            this.isGentani == true ? 'Y' : 'N',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsIssuingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsIssuingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectReq = this.selectedRow.status == 'REQUESTED' ? true : false;
        this.arrIdStatus = params.api.getSelectedRows();

        this.checkbox = this.checkbox == false ? true : false
        if (this.checkbox == true && this.arrIdStatus.length > 1) {
            this.listRequest = [];
            this.isConfirmAll = true;
            console.log('checkedAll');

            this.status = "ISSUED";
            const rowData = this.rowData.map(res => {
                if (res.status != 'ISSUED') {
                    res.status = this.status;
                    res.qtyIssue = res.qtyRequest;
                    res.issueDate = this._dateTimeService.convertToDatetime(this.dateCurent);;
                    res.checked = 'checked';
                    this.listRequest.push(res);
                }
                return res;
            });
            this.dataParams.api.setRowData(rowData);
            // setTimeout(() => {
            //     this.dataParams.api.selectAll();
            // }, 500);
            this.resetGridView();

        }
        else if (this.arrIdStatus.length > 1 && this.checkbox == false) {
            this.isConfirmAll = false;
            console.log('notCheckedAll');
            this.searchDatas();
            this.resetGridView();
        }


        // if(this.arrIdStatus.length == 1){
        //     this.conFirmStartusMultiChk();
        // }


    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;

        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;

        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe()
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    deleteRow(system: InvGpsIssuingDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(): void {
        this._service.getStockIssuingToExcel(
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.reqDateFrom),
            this._dateTimeService.convertToDatetime(this.reqDateTo),
            this._dateTimeService.convertToDatetime(this.issueDateFrom),
            this._dateTimeService.convertToDatetime(this.issueDateTo),
            this._dateTimeService.convertToDatetime(this.todayfilter),
            this.statusFilter,
            this.isGentani == true ? 'Y' : 'N',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    conFirmStartusMultiChk(system?: InvGpsIssuingDto): void {
        var month = Number(new Date().getMonth() + 1);
        var year = Number(new Date().getFullYear());
        var _fiscalYear = month > 4 ? (year + 1) : year;

        this.message.confirm("", "Are you sure the selected item can be delivered ?",
            (isConfirmed) => {
                if (isConfirmed) {
                    this.isLoading = true;
                    this.listRequest.forEach(e => {
                        this.listIdStatus += e.id + ',';
                    })

                    const dataCheckBudget = [];
                    let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                        wbsMasterData: '',
                        fiscalYear: _fiscalYear.toString(),
                        costCenter: '',
                        fixedAssetNo: '',
                        system: 'MMS',
                    })
                    dataCheckBudget.push(obj);

                    //check budget 
                    if (this.isCheckBudget === 'Y') {
                        console.log('check budget');
                       // this._sapService.sapOnlineBudgetCheck(dataCheckBudget[0])
                         //   .subscribe((budget) => {
                                this.isBudgetSuccess = true;

                                if (this.isBudgetSuccess) {
                                    this._service.conFirmStatusMultiCkb(this.listIdStatus, 'N')
                                        .subscribe((res) => {
                                            this.callBackDataGrid(this.dataParams!);

                                            this.notify.success(this.l('SavedSuccessfully'));
                                            this.isLoading = false;
                                        });
                                }
                                else {
                                    this.notify.info(this.l('Budget is not enough'));
                                }
                            //})
                    } else { // not check budget 
                        console.log('not check budget');
                        this._service.conFirmStatusMultiCkb(this.listIdStatus, 'N')
                            .subscribe((res) => {
                                this.callBackDataGrid(this.dataParams!);
                                this.notify.success(this.l('SavedSuccessfully'));
                                this.isLoading = false;
                            });
                    }
                }
            }
        );

    }

    subFundCommitmentToSap(type,listId,isClosed){
       // this._sapService.submitFundCommitmentToSap(type,1,listId,isClosed)
       // .subscribe((budget) => {
      //     
       // })
    }

}
