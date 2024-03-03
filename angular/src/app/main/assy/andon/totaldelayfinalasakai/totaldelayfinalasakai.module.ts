import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TotalDelayFinalAsakaiRoutingModule } from './totaldelayfinalasakai-routing.module';
import { TotalDelayFinalAsakaiComponent } from './totaldelayfinalasakai.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_TOTALDELAYFINALASAKAI]: TotalDelayFinalAsakaiComponent
};

@NgModule({
    declarations: [
       TotalDelayFinalAsakaiComponent,
    ],
    imports: [
        AppSharedModule, TotalDelayFinalAsakaiRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class TotalDelayFinalAsakaiModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
