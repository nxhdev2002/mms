import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { CommonModule } from '@angular/common';
import { WdPunchIndicatorRoutingModule } from './delaycontrolscreen-routing.module';
import { DelaycontrolscreenComponent } from './delaycontrolscreen.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_DELAYCONTROLSCREEN]: DelaycontrolscreenComponent
}


@NgModule({
  imports: [
    WdPunchIndicatorRoutingModule,
    CommonModule
  ],
  declarations: [DelaycontrolscreenComponent],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class DelaycontrolscreenModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
