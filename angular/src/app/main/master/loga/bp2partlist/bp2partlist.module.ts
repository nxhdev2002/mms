import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { Bp2PartListRoutingModule } from './bp2partlist-routing.module';
import { Bp2PartListComponent } from './bp2partlist.component';
import { CreateOrEditBp2PartListModalComponent } from './create-or-edit-bp2partlist-modal.component';
import { ImportBp2PartListComponent } from './import-bp2partlist-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.MASTER_LOGA_BP2PARTLIST]: Bp2PartListComponent
};


@NgModule({
    declarations: [
       Bp2PartListComponent,
        CreateOrEditBp2PartListModalComponent,
        ImportBp2PartListComponent

    ],
    imports: [
        AppSharedModule, Bp2PartListRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class Bp2PartListModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
