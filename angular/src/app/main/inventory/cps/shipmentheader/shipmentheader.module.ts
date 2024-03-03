import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ShipmentHeaderComponent } from './shipmentheader.component';
import { ShipmentHeaderRoutingModule } from './shipmentheader-routing.module';
import { FormsModule } from '@angular/forms';


// const tabcode_component_dict = {
//     [TABS.APP_MASTER_COMMON_MODEL]: ShipmentHeaderComponent
// };


@NgModule({
    declarations: [
        ShipmentHeaderComponent,
    ],
    imports: [
        AppSharedModule,
        ShipmentHeaderRoutingModule,


    ]
})
export class ShipmentHeaderModule {
    // static getComponent(tabCode: string) {
    //     return tabcode_component_dict[tabCode];
    // }
}
