import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ProcessRoutingModule } from './process-routing.module';
import { ProcessComponent } from './process.component';
import { CreateOrEditProcessModalComponent } from './create-or-edit-process-modal.component';

import { TABS } from '@app/shared/constants/tab-keys';



const tabcode_component_dict = {
    [TABS.FRM_ADO_FRAMEPROGRESS]: ProcessComponent
};


@NgModule({
    declarations: [
       ProcessComponent,
        CreateOrEditProcessModalComponent

    ],
    imports: [
        AppSharedModule, ProcessRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ProcessModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
