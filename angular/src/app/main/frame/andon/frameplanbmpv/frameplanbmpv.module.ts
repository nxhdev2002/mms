import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { FramePlanBMPVRoutingModule } from './frameplanbmpv-routing.module';
import { FramePlanBMPVComponent } from './frameplanbmpv.component';
import { ImportFramePlanBMPVComponent } from './import-frameplanbmpv-modal.component';
import { ListErrorImportFramePlanBMPVComponent } from './list-error-import-frameplanbmpv-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.FRM_ADO_FRAMEPLANBMPV]: FramePlanBMPVComponent
};

@NgModule({
    declarations: [
       FramePlanBMPVComponent,
        ImportFramePlanBMPVComponent,
        ListErrorImportFramePlanBMPVComponent
    ],
    imports: [
        AppSharedModule, FramePlanBMPVRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class FramePlanBMPVModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
