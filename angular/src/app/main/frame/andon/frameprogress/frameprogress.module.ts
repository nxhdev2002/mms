import { CUSTOM_ELEMENTS_SCHEMA,NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FrameProgressRoutingModule } from './frameprogress-routing.module';
import { FrameProgressComponent } from './frameprogress.component';
import { TABS } from '@app/shared/constants/tab-keys';


const tabcode_component_dict = {
    [TABS.FRM_ADO_FRAMEPROGRESS]: FrameProgressComponent
};

@NgModule({
    declarations: [
        FrameProgressComponent,
    ],
    imports: [
        AppSharedModule, FrameProgressRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class FrameProgressModule { 
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
