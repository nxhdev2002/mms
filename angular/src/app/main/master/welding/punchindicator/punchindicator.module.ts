import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PunchIndicatorRoutingModule } from './punchindicator-routing.module';
import { PunchIndicatorComponent } from './punchindicator.component';
import { CreateOrEditPunchIndicatorModalComponent } from './create-or-edit-punchindicator-modal.component';

const tabcode_component_dict = {
    [TABS.WEL_ADO_PUNCHQUEUEINDICATOR]: PunchIndicatorComponent
};

@NgModule({
    declarations: [
       PunchIndicatorComponent,
        CreateOrEditPunchIndicatorModalComponent

    ],
    imports: [
        AppSharedModule, PunchIndicatorRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PunchIndicatorModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
