import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PxPUpPlanRoutingModule } from './pxpupplan-routing.module';
import { PxPUpPlanComponent } from './pxpupplan.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
   // [TABS.MASTER_COMMON_LOOKUP]: PxPUpPlanComponent
};

@NgModule({
    declarations: [
        PxPUpPlanComponent
    ],
    imports: [
        PxPUpPlanRoutingModule,AppSharedModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PxPUpPlanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}



