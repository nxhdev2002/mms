import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvOutLineOffRoutingModule } from './invoutlineoff-routing.module';
import { InvOutLineOffComponent } from './invoutlineoff.component';
import { RequestModalComponent } from './request-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';


const tabcode_component_dict = {
    [TABS.INV_D125_INVOUTLINEOFF]: InvOutLineOffComponent
};
@NgModule({
    declarations: [
       InvOutLineOffComponent, 
       RequestModalComponent   
    ],
    imports: [
        AppSharedModule, InvOutLineOffRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class InvOutLineOffModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
