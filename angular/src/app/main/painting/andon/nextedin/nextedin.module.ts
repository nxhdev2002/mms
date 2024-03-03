import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { NextEdInRoutingModule } from './nextedin-routing.module';
import { NextEdInComponent } from './nextedin.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_NEXTEDIN]: NextEdInComponent
}

@NgModule({
  imports: [
    NextEdInRoutingModule,
    AppSharedModule,
  ],
  declarations: [
    NextEdInComponent
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class NextEdInModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
