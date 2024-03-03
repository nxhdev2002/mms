import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { AssemblyDataRoutingModule } from './assemblydata-routing.module';
import { AssemblyDataComponent } from './assemblydata.component';
import { TABS } from '@app/shared/constants/tab-keys';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_ASSEMBLYDATA]: AssemblyDataComponent
};

@NgModule({
    declarations: [
       AssemblyDataComponent
    ],
    imports: [
        AppSharedModule, AssemblyDataRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class AssemblyDataModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
