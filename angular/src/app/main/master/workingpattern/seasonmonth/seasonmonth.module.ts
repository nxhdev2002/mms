import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { SeasonmonthRoutingModule } from './seasonmonth-routing.module';
import { SeasonmonthComponent } from './seasonmonth.component';
import { CreateOrEditSeasonmonthModalComponent } from './create-or-edit-seasonmonth-modal.component';

const tabcode_component_dict = {
    [TABS.MASTER_WORKING_PATTERN_SEASONMONTH]: SeasonmonthComponent
}

@NgModule({
    declarations: [
        SeasonmonthComponent,
        CreateOrEditSeasonmonthModalComponent
    ],
    imports: [
        AppSharedModule, SeasonmonthRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class SeasonmonthModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
 }
