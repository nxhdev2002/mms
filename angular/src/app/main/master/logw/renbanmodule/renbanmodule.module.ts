import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { RenbanModuleRoutingModule } from './renbanmodule-routing.module';
import { RenbanModuleComponent } from './renbanmodule.component';
import { CreateOrEditRenbanModuleModalComponent } from './create-or-edit-renbanmodule-modal.component';
import { ImportRenbanModuleComponent } from './import-renbanmodule-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_RENBANMODULE]: RenbanModuleComponent
};


@NgModule({
    declarations: [
        RenbanModuleComponent,
        CreateOrEditRenbanModuleModalComponent,
        ImportRenbanModuleComponent
    ],
    imports: [
        AppSharedModule, RenbanModuleRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class RenbanModuleModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
