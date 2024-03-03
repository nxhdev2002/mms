import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FramePlanRoutingModule } from './frameplan-routing.module';
import { FramePlanComponent } from './frameplan.component';
import { ImportFramePlanComponent } from './import-frameplan-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';


const tabcode_component_dict = {
    [TABS.FRM_ADO_FRAMEPLAN]: FramePlanComponent
};


@NgModule({
    declarations: [
        FramePlanComponent,
        ImportFramePlanComponent
    ],
    imports: [
        AppSharedModule, FramePlanRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class FramePlanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
