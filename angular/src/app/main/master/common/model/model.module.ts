import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ModelRoutingModule } from './model-routing.module';
import { ModelComponent } from './model.component';
import { CreateOrEditModelModalComponent } from './create-or-edit-model-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';


// const tabcode_component_dict = {
//     [TABS.APP_MASTER_COMMON_MODEL]: ModelComponent
// };


@NgModule({
    declarations: [
       ModelComponent,
        CreateOrEditModelModalComponent,
    ],
    imports: [
        AppSharedModule,
        ModelRoutingModule
    ]
})
export class ModelModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
