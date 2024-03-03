import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ModelGradeRoutingModule } from './modelgrade-routing.module';
import { ModelGradeComponent } from './modelgrade.component';
import { CreateOrEditModelGradeModalComponent } from './create-or-edit-modelgrade-modal.component';
import { ImportModelGradeComponent } from './import-modelgrade-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_LOGW_MODELGRADE]: ModelGradeComponent
};


@NgModule({
    declarations: [
       ModelGradeComponent,
        CreateOrEditModelGradeModalComponent,
        ImportModelGradeComponent
    ],
    imports: [
        AppSharedModule, ModelGradeRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ModelGradeModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
