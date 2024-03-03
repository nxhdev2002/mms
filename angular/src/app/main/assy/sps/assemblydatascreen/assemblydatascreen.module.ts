import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AssemblyDataScreenRoutingModule } from './assemblydatascreen-routing.module';
import { AssemblyDataScreenComponent } from './assemblydatascreen.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
   // [TABS.ASSY_ADO_VEHICLEDETAILS]: AssemblyDataScreenComponent
};
@NgModule({
  declarations: [AssemblyDataScreenComponent],
  imports: [
    CommonModule,
    AssemblyDataScreenRoutingModule
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]

})
export class AssemblyDataScreenModule { 
  static getComponent(tabCode: string) {
    return tabcode_component_dict[tabCode];
}
}

