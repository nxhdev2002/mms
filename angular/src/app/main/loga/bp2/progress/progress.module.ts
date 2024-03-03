import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProgressRoutingModule } from './progress-routing.module';
import { ProgressComponent } from './progress.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    //[TABS.MASTER_COMMON_LOOKUP]: ProgressComponent
};

@NgModule({
    declarations: [
       ProgressComponent    
    ],
    imports: [
        AppSharedModule, ProgressRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ProgressModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}

