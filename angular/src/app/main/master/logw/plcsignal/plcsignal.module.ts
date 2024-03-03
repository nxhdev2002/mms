import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PlcsignalRoutingModule } from './plcsignal-routing.module';
import { PlcsignalComponent } from './plcsignal.component';
//import { CreateOrEditPlcSignalModalComponent } from './create-or-edit-plcsignal-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGA_PLCSIGNAL]: PlcsignalComponent
};


@NgModule({
    declarations: [
       PlcsignalComponent,
    ],
    imports: [
        AppSharedModule, PlcsignalRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PlcsignalModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
