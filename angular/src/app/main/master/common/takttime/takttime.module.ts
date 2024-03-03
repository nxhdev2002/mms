import {NgModule} from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TakttimeRoutingModule } from './takttime-routing.module';
import { TakttimeComponent } from './takttime.component';
import { CreateOrEditTakttimeModalComponent } from './create-or-edit-takttime-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';


// const tabcode_component_dict = {
//     [TABS.APP_MASTER_COMMON_TAKTTIME]: TakttimeComponent
// };

@NgModule({
    declarations: [
        TakttimeComponent,
        CreateOrEditTakttimeModalComponent

    ],
    imports: [
        AppSharedModule, TakttimeRoutingModule]
})
export class TakttimeModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
