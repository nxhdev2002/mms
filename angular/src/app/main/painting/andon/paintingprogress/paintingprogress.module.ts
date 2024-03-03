import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PaintingProgressRoutingModule } from './paintingprogress-routing.module';
import { PaintingProgressComponent } from './paintingprogress.component';
import { CreateOrEditPaintingProgressModalComponent } from './create-or-edit-paintingprogress-modal.component';

const tabcode_component_dict = {
    [TABS.PTA_ADO_PAINTINGPROGRESS]: PaintingProgressComponent
}


@NgModule({
    declarations: [
       PaintingProgressComponent,
        CreateOrEditPaintingProgressModalComponent
        ],
    imports: [
        AppSharedModule, PaintingProgressRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PaintingProgressModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
