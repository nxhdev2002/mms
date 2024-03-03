import { Component, Input } from '@angular/core';
import { ICellRendererAngularComp } from '@ag-grid-community/angular';

@Component({
    selector: 'ag-datepicker-renderer',
    templateUrl: './ag-datepicker-renderer.component.html',
    styleUrls: ['./ag-datepicker-renderer.component.less']
})
export class AgDatepickerRendererComponent implements ICellRendererAngularComp {

    hasTimePicker: boolean | Function;
    public params: any;
    field: string;
    // tslint:disable-next-line:ban-types
    disabled: boolean | Function;
    isDisabled: boolean | Function;
    
    DisabledTxt: string = 'false';
    valueDate: string;

    datePicker;
    placementDatePicker: string = 'bottom'
    cellRendererParamsCustom?: {
        code?: string;
        disabled?: boolean | Function;
        valueDate?: string | Function;
    }

    constructor() {
    }

    agInit(params: any): void {
        this.params = params;
        if (typeof this.params.colDef.disabled === 'function') {
            this.disabled = this.params.colDef.disabled(params);
        } else {
            this.disabled = this.params.colDef.disabled || false;
        }
        this.field = this.params.colDef.field;
        this.datePicker = this.params.data[this.field];
        this.hasTimePicker = this.params.colDef.hasTimePicker;
        
        this.cellRendererParamsCustom = this.params.colDef.cellRendererParamsCustom;
        if(this.cellRendererParamsCustom) { 
            this.placementDatePicker = this.cellRendererParamsCustom.code;


            if(this.cellRendererParamsCustom?.disabled) {
                if (typeof this.params.colDef.cellRendererParamsCustom.disabled === 'function') {
                    this.isDisabled = this.params.colDef.cellRendererParamsCustom.disabled(this.params);
                } else {
                    this.isDisabled = this.cellRendererParamsCustom.disabled || false;
                } 
                this.DisabledTxt = (this.isDisabled) ? 'true': 'false';
                if(this.DisabledTxt == 'true') {
                 console.log(this.params);
                }
            } 
            if(this.cellRendererParamsCustom?.valueDate) {
                if (typeof this.params.colDef.cellRendererParamsCustom.valueDate === 'function') {
                    this.valueDate = this.params.colDef.cellRendererParamsCustom.valueDate(this.params);
                } else {
                    this.valueDate = this.params.colDef.cellRendererParamsCustom.valueDate;
                }  
            } 
        }
    }

    changeDateValue(val) {
        if(val == undefined){
            this.params.data[this.field] = undefined;
            this.params.column.editingStartedValue = undefined;
            this.params.node.setDataValue(this.params.colDef.field, undefined);
        }
        if (!this.params.data[this.field] || ((this.params.data[this.field] && val) && this.params.data[this.field] !== val)) {
            this.params.column.editingStartedValue = this.params.data[this.field];
            this.params.node.setDataValue(this.params.colDef.field, val);
        }
    }

    refresh(): boolean {
        return false;
    }

}
