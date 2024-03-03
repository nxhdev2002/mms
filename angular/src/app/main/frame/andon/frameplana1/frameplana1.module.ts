import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { FramePlanA1RoutingModule } from './frameplana1-routing.module';
import { FramePlanA1Component } from './frameplana1.component';
import { CreateOrEditFramePlanA1ModalComponent } from './create-or-edit-frameplana1-modal.component';
import { ImportFramePlanA1Component } from './import-frameplana1-modal.component';
import { ListErrorImportFramePlanA1Component } from './list-error-import-frameplana1-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.FRM_ADO_FRAMEPLANA1]: FramePlanA1Component
};

@NgModule({
    declarations: [
       FramePlanA1Component,
        CreateOrEditFramePlanA1ModalComponent,
        ImportFramePlanA1Component,
        ListErrorImportFramePlanA1Component
    ],
    imports: [
        AppSharedModule, FramePlanA1RoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class FramePlanA1Module {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
