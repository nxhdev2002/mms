import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { PatternHRoutingModule } from './patternh-routing.module';
import { PatternHComponent } from './patternh.component';
import { CreateOrEditPatternHModalComponent } from './create-or-edit-patternh-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_PATTERNH]: PatternHComponent
}

@NgModule({
    declarations: [
        PatternHComponent,
        CreateOrEditPatternHModalComponent
    ],
    imports: [
        AppSharedModule, PatternHRoutingModule],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PatternHModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
