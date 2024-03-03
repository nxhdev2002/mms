import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';

import { Bp2PartListGradeExportComponent } from './bp2partlistgrade-export-modal.component';
import { Bp2PartListGradeRoutingModule } from './bp2partlistgrade-routing.module';
import { Bp2PartListGradeComponent } from './bp2partlistgrade.component';
import { CreateOrEditBp2PartListGradeModalComponent } from './create-or-edit-bp2partlistgrade-modal.component';
import { ImportBp2PartListGradeComponent } from './import-bp2partlistgrade-modal.component';


const tabcode_component_dict = {
    [TABS.MASTER_LOGA_BP2PARTLISTGRADE]: Bp2PartListGradeComponent
};


@NgModule({
    declarations: [
       Bp2PartListGradeComponent,
       CreateOrEditBp2PartListGradeModalComponent,
       ImportBp2PartListGradeComponent,
       Bp2PartListGradeExportComponent

    ],
    imports: [
        AppSharedModule, Bp2PartListGradeRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class Bp2PartListGradeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
