import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PaintingProcessRoutingModule } from './paintingprocess-routing.module';
import { PaintingProcessComponent } from './paintingprocess.component';
import { CreateOrEditPaintingProcessModalComponent } from './create-or-edit-paintingprocess-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_PAINTING_PAINTINGPROCESS]: PaintingProcessComponent
};

@NgModule({
    declarations: [
       PaintingProcessComponent,
       CreateOrEditPaintingProcessModalComponent
    ],
    imports: [
        AppSharedModule, PaintingProcessRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PaintingProcessModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
