import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InDetailsRoutingModule } from './indetails-routing.module';
import { InDetailsComponent } from './indetails.component';
import { RequestModalComponent } from './request-modal.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.INV_D125_INDETAILS]: InDetailsComponent
};

@NgModule({
    declarations: [
       InDetailsComponent, 
       RequestModalComponent    
    ],
    imports: [
        AppSharedModule, InDetailsRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class InDetailsModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
