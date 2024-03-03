import { AgFilterGridComponent } from './../grid/ag-filter-grid/ag-filter-grid.component';
import { AgFloatingFilterGridComponent } from '../grid/ag-floating-filter-grid/ag-floating-filter-grid.component';
import { AgCheckboxRendererComponent } from './../grid/ag-checkbox-renderer/ag-checkbox-renderer.component';
import { ColDef, Column, ColumnApi, GridApi, ICellEditorParams, IsColumnFunc } from '@ag-grid-enterprise/all-modules';
import { AgCellButtonRendererComponent } from '../grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgCellTextRendererComponent } from '../grid/ag-cell-text-renderer/ag-cell-text-renderer.component';
import { AgDropdownRendererComponent } from '../grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { AgDatepickerRendererComponent } from '../grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { AgCellButtonTransparentRendererComponent } from '../grid/ag-cell-button-transparent-renderer/ag-cell-button-transparent-renderer.component';

export interface PaginationParamsModel {
  totalCount?: number | undefined;
  totalPage?: number | undefined;
  sorting?: string | undefined;
  skipCount?: number | undefined;
  pageSize?: number | undefined;
  pageNum?: number | undefined;
}

export interface GridParams {
  api: GridApi,
  columnApi: ColumnApi,
  editingStartedValue: string,
  invalidRcCode?: boolean,
  invalidPartCode?: boolean,
}

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface GridFrameworkComponent {

}

export interface RowSelectionParams {
    api: GridApi,
    columnApi: ColumnApi
}

export interface CustomColDef extends ColDef {
    cellRendererParamsCustom?: {
        code?: string;
        disabled?: boolean | Function;
        valueDate?: string | Function;
    },
    buttonDef?: {
        text?: string | Function;
        useRowData?: boolean;
        disabled?: boolean | Function;
        function?: (params: any) => void;
        iconName?: string;
        className?: string | Function;
        message?: string;
    },
    buttonDefTwo?: {
        text?: string | Function;
        useRowData?: boolean;
        disabled?: boolean | Function;
        function?: (params: any) => void;
        iconName?: string;
        className?: string | Function;
        message?: string;
    },
    textCellDef?: {
        text?: string | Function;
        useRowData?: boolean;
        disabled?: boolean | Function;
        function?: (params: any) => void;
        iconName?: string;
        className?: string | Function;
        message?: string;
    },
    disableSelect?: boolean | IsColumnFunc,
    list?: {
        key?: string | number,
        value?: string | number,
    }[] | Function,
    disabled?: boolean | IsColumnFunc,
    disableCheckbox?: boolean | IsColumnFunc,
    data?: string[] | boolean[] | number[],
    children?: CustomColDef[],
    validators?: string[],
    textFormatter?: IsColumnFunc | Function | any,
    property?: { key: string, value: string },
    listName?: string,
    api?: any,
    cellClass?: any,
    maxLength?: number,
}

export interface AgCellEditorParams extends ICellEditorParams {
    column: AgColumn,
    oldValue: string | number | undefined,
    newValue: string | number | undefined,
    key?: string | number,
    event?: KeyboardEvent,
    colDef: CustomColDef,
    api: GridApi,
    columnApi: ColumnApi,
    data: any
}

export interface AgColumn extends Column {
    editingStartedValue: string | number | undefined
}

export interface FrameworkComponent {
    agSelectRendererComponent?: typeof AgDropdownRendererComponent,
    agCellButtonComponent?: typeof AgCellButtonRendererComponent,
    AgCellTextComponent?: typeof AgCellTextRendererComponent,
    agCellButtonTransparentComponent?: typeof AgCellButtonTransparentRendererComponent,
    agCheckboxRendererComponent?: typeof AgCheckboxRendererComponent,
    agDatepickerRendererComponent?: typeof AgDatepickerRendererComponent,
    agFloatingFilterGridComponent?: typeof AgFloatingFilterGridComponent,
    agFilterGridComponent?: typeof AgFilterGridComponent
}

export interface AgCellPositionParams {
    column?: Column,
    rowIndex?: number,
    rowPinned: string | undefined,
    floating: string | undefined
}
