import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TABS } from '@app/shared/constants/tab-keys';
import { ProductionPlanRoutingModule } from './productionplan-routing.module';
import { ProductionPlanComponent } from './productionplan.component';
import { CreateOrEditProductionPlanModalComponent } from './create-or-edit-productionplan-modal.component';
import { ImportPlnCcrProductionPlanComponent } from './import-productionplan-modal.component';

const tabcode_component_dict = {
    [TABS.PLA_CCR_PRODUCTIONPLAN]: ProductionPlanComponent
};


@NgModule({
    declarations: [
        ProductionPlanComponent,
        CreateOrEditProductionPlanModalComponent,
        ImportPlnCcrProductionPlanComponent
    ],
    imports: [
        AppSharedModule, ProductionPlanRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class ProductionPlanModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
