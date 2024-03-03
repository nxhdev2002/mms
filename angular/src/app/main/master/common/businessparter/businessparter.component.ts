import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmBusinessParterDto, MstCmmBusinessParterServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBusinessParterModalComponent } from './create-or-edit-businessparter-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './businessparter.component.html'
})
export class BusinessParterComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBusinessParter', { static: true }) createOrEditModalBusinessParter:| CreateOrEditBusinessParterModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstCmmBusinessParterDto = new MstCmmBusinessParterDto();
    saveSelectedRow: MstCmmBusinessParterDto = new MstCmmBusinessParterDto();
    datas: MstCmmBusinessParterDto = new MstCmmBusinessParterDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter= '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    businessPartnerGroup = '' ;
    businessPartnerCategory = '' ;
	businessPartnerRole = '' ;
	businessPartnerCd = '' ;
	emailAddress1 = '' ;
	suppSearcgTerm = '' ;
	businessPartnerName1 = '' ;
	businessPartnerName2 = '' ;
	businessPartnerName3 = '' ;
	businessPartnerName4 = '' ;
	address1 = '' ;
	address2 = '' ;
	address3 = '' ;
	district = '' ;
	city = '' ;
	postalCd = '' ;
	country = '' ;
	phoneNo = '' ;
	faxNo = '' ;
	taxNo = '' ;
	taxCate = '' ;
	companyCode = '' ;
	paymentMethodCd = '' ;
	paymentMethodNm = '' ;
	paymentTermCd = '' ;
	paymentTermNm = '' ;
	orderCurrency = '' ;
	typeOfIndustry = '' ;
	previousMasterRecordNumber = '' ;
	textIdTitle = '' ;
	uniqueBankId = '' ;
	suppBankKey = '' ;
	suppBankCountry = '' ;
	suppAccount = '' ;
	accountHolder = '' ;
	accname = '' ;
	partnerBankName = '' ;
	externalId = '' ;


    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
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
        private _service: MstCmmBusinessParterServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
//             			{headerName: this.l('Nation'),headerTooltip: this.l('Nation'),field:  'nation', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
// buttonDefTwo: { text: params => (params.data?.'nation' == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.'nation' == 'Y') ? 'btnActive' : 'btnInActive',},}
// 			{headerName: this.l('Business Partner Category'),headerTooltip: this.l('Business Partner Category'),field:  'businessPartnerCategory', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
// buttonDefTwo: { text: params => (params.data?.'businessPartnerCategory' == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.'businessPartnerCategory' == 'Y') ? 'btnActive' : 'btnInActive',},}
			{headerName: this.l('Nation'),headerTooltip: this.l('Nation'),field: 'nation',flex: 1},
			{headerName: this.l('Business Partner Group'),headerTooltip: this.l('Business Partner Group'),field: 'businessPartnerGroup',flex: 1},
			{headerName: this.l('Business Partner Category'),headerTooltip: this.l('Business Partner Category'),field: 'businessPartnerCategory',flex: 1},
			{headerName: this.l('Business Partner Role'),headerTooltip: this.l('Business Partner Role'),field: 'businessPartnerRole',flex: 1},
			{headerName: this.l('Business Partner Cd'),headerTooltip: this.l('Business Partner Cd'),field: 'businessPartnerCd',flex: 1},
			{headerName: this.l('Email Address 1'),headerTooltip: this.l('Email Address 1'),field: 'emailAddress1',flex: 1},
			{headerName: this.l('Supp Searcg Term'),headerTooltip: this.l('Supp Searcg Term'),field: 'suppSearcgTerm',flex: 1},
			{headerName: this.l('Business Partner Name 1'),headerTooltip: this.l('Business Partner Name 1'),field: 'businessPartnerName1',flex: 1},
			{headerName: this.l('Business Partner Name 2'),headerTooltip: this.l('Business Partner Name 2'),field: 'businessPartnerName2',flex: 1},
			{headerName: this.l('Business Partner Name 3'),headerTooltip: this.l('Business Partner Name 3'),field: 'businessPartnerName3',flex: 1},
			{headerName: this.l('Business Partner Name 4'),headerTooltip: this.l('Business Partner Name 4'),field: 'businessPartnerName4',flex: 1},
			{headerName: this.l('Address 1'),headerTooltip: this.l('Address 1'),field: 'address1',flex: 1},
			{headerName: this.l('Address 2'),headerTooltip: this.l('Address 2'),field: 'address2',flex: 1},
			{headerName: this.l('Address 3'),headerTooltip: this.l('Address 3'),field: 'address3',flex: 1},
			{headerName: this.l('District'),headerTooltip: this.l('District'),field: 'district',flex: 1},
			{headerName: this.l('City'),headerTooltip: this.l('City'),field: 'city',flex: 1},
			{headerName: this.l('Postal Cd'),headerTooltip: this.l('Postal Cd'),field: 'postalCd',flex: 1},
			{headerName: this.l('Country'),headerTooltip: this.l('Country'),field: 'country',flex: 1},
			{headerName: this.l('Phone No'),headerTooltip: this.l('Phone No'),field: 'phoneNo',flex: 1},
			{headerName: this.l('Fax No'),headerTooltip: this.l('Fax No'),field: 'faxNo',flex: 1},
			{headerName: this.l('Tax No'),headerTooltip: this.l('Tax No'),field: 'taxNo',flex: 1},
			{headerName: this.l('Tax Cate'),headerTooltip: this.l('Tax Cate'),field: 'taxCate',flex: 1},
			{headerName: this.l('Company Code'),headerTooltip: this.l('Company Code'),field: 'companyCode',flex: 1},
			{headerName: this.l('Payment Method Cd'),headerTooltip: this.l('Payment Method Cd'),field: 'paymentMethodCd',flex: 1},
			{headerName: this.l('Payment Method Nm'),headerTooltip: this.l('Payment Method Nm'),field: 'paymentMethodNm',flex: 1},
			{headerName: this.l('Payment Term Cd'),headerTooltip: this.l('Payment Term Cd'),field: 'paymentTermCd',flex: 1},
			{headerName: this.l('Payment Term Nm'),headerTooltip: this.l('Payment Term Nm'),field: 'paymentTermNm',flex: 1},
			{headerName: this.l('Order Currency'),headerTooltip: this.l('Order Currency'),field: 'orderCurrency',flex: 1},
			{headerName: this.l('Type Of Industry'),headerTooltip: this.l('Type Of Industry'),field: 'typeOfIndustry',flex: 1},
			{headerName: this.l('Previous Master Record Number'),headerTooltip: this.l('Previous Master Record Number'),field: 'previousMasterRecordNumber',flex: 1},
			{headerName: this.l('Text Id Title'),headerTooltip: this.l('Text Id Title'),field: 'textIdTitle',flex: 1},
			{headerName: this.l('Unique Bank Id'),headerTooltip: this.l('Unique Bank Id'),field: 'uniqueBankId',flex: 1},
			{headerName: this.l('Supp Bank Key'),headerTooltip: this.l('Supp Bank Key'),field: 'suppBankKey',flex: 1},
			{headerName: this.l('Supp Bank Country'),headerTooltip: this.l('Supp Bank Country'),field: 'suppBankCountry',flex: 1},
			{headerName: this.l('Supp Account'),headerTooltip: this.l('Supp Account'),field: 'suppAccount',flex: 1},
			{headerName: this.l('Account Holder'),headerTooltip: this.l('Account Holder'),field: 'accountHolder',flex: 1},
			{headerName: this.l('Accname'),headerTooltip: this.l('Accname'),field: 'accname',flex: 1},
			{headerName: this.l('Partner Bank Name'),headerTooltip: this.l('Partner Bank Name'),field: 'partnerBankName',flex: 1},
			{headerName: this.l('External Id'),headerTooltip: this.l('External Id'),field: 'externalId',flex: 1},
			{headerName: this.l('Status Flag Ab'),headerTooltip: this.l('Status Flag Ab'),field: 'statusFlagAb',flex: 1},
			{headerName: this.l('Status Flag Cb'),headerTooltip: this.l('Status Flag Cb'),field: 'statusFlagCb',flex: 1},
			{headerName: this.l('Status Flag Ad'),headerTooltip: this.l('Status Flag Ad'),field: 'statusFlagAd',flex: 1},
			{headerName: this.l('Status Flag Cd'),headerTooltip: this.l('Status Flag Cd'),field: 'statusFlagCd',flex: 1},
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.businessPartnerCategory,
			this.city,
			this.phoneNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        });
    }

    clearTextSearch() {
        this.businessPartnerCategory = '' ;
		this.city = '' ;
		this.phoneNo = '' ;
        this.searchDatas();
    }
	autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){

        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },1000)
    }
    onSelectionMulti(params) {
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRow, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRow.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
			this.businessPartnerCategory,
			this.city,
			this.phoneNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmBusinessParterDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmBusinessParterDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
		this.resetGridView();
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
				this.resetGridView();

            });
    }

    // exportToExcel(): void {
    //     this._service.getBusinessParterToExcel(
	// 		'',
	// 		'',
    //         this.businessPartnerGroup,
    //         this.businessPartnerRole,
    //         this.businessPartnerCd,
    //         this.emailAddress1,
    //         this.suppSearcgTerm,
    //         this.businessPartnerName1,
    //         this.businessPartnerName2,
    //         this.businessPartnerName3,
    //         this.businessPartnerName4,
    //         this.address1,
    //         this.address2,
    //         this.address3,
    //         this.district,
    //         this.city,
    //         this.postalCd,
    //         this.country,
    //         this.phoneNo,
    //         this.faxNo,
    //         this.taxNo,
    //         this.taxCate,
    //         this.companyCode,
    //         this.paymentMethodCd,
    //         this.paymentMethodNm,
    //         this.paymentTermCd,
    //         this.paymentTermNm,
    //         this.orderCurrency,
    //         this.typeOfIndustry,
    //         this.previousMasterRecordNumber,
    //         this.textIdTitle,
    //         this.uniqueBankId,
    //         this.suppBankKey,
    //         this.suppBankCountry,
    //         this.suppAccount,
    //         this.accountHolder,
    //         this.accname,
    //         this.partnerBankName,
    //         this.externalId,
    //         '',
	// 		'',
	// 		'',
	// 		'',
    //     	)
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //     });
    // }
}
