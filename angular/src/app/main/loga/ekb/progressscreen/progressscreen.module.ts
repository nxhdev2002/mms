import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { ProgressScreenRoutingModule } from './progressscreen-routing.module';
import { ProgressScreenComponent } from './progressscreen.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { AppSharedModule } from '@app/shared/app-shared.module';

const tabcode_component_dict = {
   // [TABS.MASTER_COMMON_LOOKUP]: ProgressScreenComponent
};

@NgModule({
    declarations: [
        ProgressScreenComponent,
    ],
    imports: [
        ProgressScreenRoutingModule,
        CommonModule,
    ],
    exports:[
        ProgressScreenComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ProgressScreenModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
