import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { BmpPartListRoutingModule } from './bmppartlist-routing.module';
import { BmpPartListComponent } from './bmppartlist.component';
import { CreateOrEditBmpPartListModalComponent } from './create-or-edit-bmppartlist-modal.component';
import { ImportBmpPartListComponent } from './import-bmppartlist-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_PAINTING_BMPPARTLIST]: BmpPartListComponent
};

@NgModule({
    declarations: [
       BmpPartListComponent,
        CreateOrEditBmpPartListModalComponent,
        ImportBmpPartListComponent

    ],
    imports: [
        AppSharedModule, BmpPartListRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class BmpPartListModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
