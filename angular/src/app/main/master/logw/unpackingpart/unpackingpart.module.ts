import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { UnpackingPartRoutingModule } from './unpackingpart-routing.module';
import { CreateOrEditUnpackingPartModalComponent } from './create-or-edit-unpackingpart-modal.component';
import { ImportUnpackingPartComponent } from './import-unpackingpart-modal.component';
import { UnpackingPartComponent } from './unpackingpart.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_UNPACKINGPART]: UnpackingPartComponent
};

@NgModule({
    declarations: [
       UnpackingPartComponent,
        CreateOrEditUnpackingPartModalComponent,
        ImportUnpackingPartComponent
    ],
    imports: [
        AppSharedModule, UnpackingPartRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class UnpackingPartModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
