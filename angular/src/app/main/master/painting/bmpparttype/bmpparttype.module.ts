import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { BmpPartTypeRoutingModule } from './bmpparttype-routing.module';
import { BmpPartTypeComponent } from './bmpparttype.component';
import { CreateOrEditBmpPartTypeModalComponent } from './create-or-edit-bmpparttype-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_PAINTING_BMPPARTTYPE]: BmpPartTypeComponent
};


@NgModule({
    declarations: [
       BmpPartTypeComponent,
        CreateOrEditBmpPartTypeModalComponent
    ],
    imports: [
        AppSharedModule, BmpPartTypeRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class BmpPartTypeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
