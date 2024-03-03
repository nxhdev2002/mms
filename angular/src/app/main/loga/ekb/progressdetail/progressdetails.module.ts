import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProgressDetailsRoutingModule } from './progressdetails-routing.module';
import { ProgressDetailsComponent } from './progressdetails.component';
import { CreateOrEditProgressDetailsModalComponent } from './create-or-edit-progressdetails-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.LOGA_EKB_PROGRESSDETAILS]: ProgressDetailsComponent
};

@NgModule({
    declarations: [
       ProgressDetailsComponent,
        CreateOrEditProgressDetailsModalComponent
    ],
    imports: [
        AppSharedModule, ProgressDetailsRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ProgressDetailsModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
