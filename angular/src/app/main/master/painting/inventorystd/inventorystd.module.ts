import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { InventoryStdRoutingModule } from './inventorystd-routing.module';
import { InventoryStdComponent } from './inventorystd.component';
import { CreateOrEditInventoryStdModalComponent } from './create-or-edit-inventorystd-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_PAINTING_INVENTORYSTD]: InventoryStdComponent
};

@NgModule({
    declarations: [
       InventoryStdComponent,
        CreateOrEditInventoryStdModalComponent
    ],
    imports: [
        AppSharedModule, InventoryStdRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class InventoryStdModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
