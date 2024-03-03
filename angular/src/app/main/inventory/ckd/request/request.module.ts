import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { FormsModule } from '@angular/forms';
import { MultiSelectModule } from 'primeng/multiselect';
import { ListboxModule } from 'primeng/listbox';
import { RequestRoutingModule } from './request-routing.module';
import { RequestComponent } from './request.component';




const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_DAILYWORKINGTIME]: RequestComponent
}

@NgModule({
    declarations: [
       RequestComponent
    ],
    imports: [
        ListboxModule,
        AppSharedModule,
        RequestRoutingModule,
        BsDropdownModule.forRoot(),
        MultiSelectModule,
        FormsModule,],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class InvCkdRequestModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
