import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PickingmonitoringScreenComponent } from './pickingmonitoringscreen.component';
import { PickingmonitoringScreenRoutingModule } from './pickingmonitoringscreen-routing.module';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
   // [TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW1]: PickingmonitoringScreenComponent
};
@NgModule({
  imports: [
    CommonModule,
    PickingmonitoringScreenRoutingModule
  ],
  declarations: [PickingmonitoringScreenComponent],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PickingmonitoringScreenModule {
  static getComponent(tabCode: string) {
      return tabcode_component_dict[tabCode];
  }
}
