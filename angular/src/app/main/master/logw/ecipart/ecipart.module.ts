import { ImportEciPartComponent } from './import-ecipart-modal.component';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { EciPartRoutingModule } from './ecipart-routing.module';
import { EciPartComponent } from './ecipart.component';
import { CreateOrEditEciPartModalComponent } from './create-or-edit-ecipart-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_ECIPART]: EciPartComponent
};



@NgModule({
    declarations: [
        EciPartComponent,
        CreateOrEditEciPartModalComponent,
        ImportEciPartComponent

    ],
    imports: [
        AppSharedModule, EciPartRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class EciPartModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
