import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { LineRealTimeControlComponent } from './linerealtimecontrol.component';
import { LineRealTimeControlRoutingModule } from './linerealtimecontrol-routing.module';


const tabcode_component_dict = {
    [TABS.PTA_ADO_LINEREALTIMECONTROL]: LineRealTimeControlComponent
}


@NgModule({
    declarations: [
        LineRealTimeControlComponent
    ],
    imports: [
        LineRealTimeControlRoutingModule,
        AppSharedModule
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LineRealTimeControlModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
