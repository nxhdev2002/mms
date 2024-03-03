import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { CreateOrEditPatternDModalComponent } from './create-or-edit-patternd-modal.component';
import { PatternDRoutingModule } from './patternd-routing.module';
import { PatternDComponent } from './patternd.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_PATTERND]: PatternDComponent
}


@NgModule({
    declarations: [
        PatternDComponent,
        CreateOrEditPatternDModalComponent
    ],
    imports: [
        AppSharedModule, PatternDRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class PatternDModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
