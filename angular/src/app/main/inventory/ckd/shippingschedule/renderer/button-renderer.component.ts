import { ICellRendererAngularComp } from "@ag-grid-community/angular";
import { Component } from "@angular/core";

@Component({
  selector: 'app-button-renderer',
  template: `
    <button type="button" (click)="onClick($event)" class="btn btn-sm btn-primary" style="padding: 0px 5px; text-align: center;" [disabled]="disabled">Confirm</button>
    `
})

export class ButtonRendererComponent implements ICellRendererAngularComp {

  params;
  paramsValue;
  disabled: boolean;


  agInit(params): void {
    this.params = params;
    this.paramsValue = this.params.customParam(this.params);
    // this.onClick = this.paramsValue.onClick;
    this.disabled = this.paramsValue.disabled;
  }

  refresh(params?: any): boolean {
    return true;
  }

  onClick($event) {
    if (this.paramsValue.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: $event,
        rowData: this.params.node.data
        // ...something
      }
      this.paramsValue.onClick(params);

    }
  }
}
