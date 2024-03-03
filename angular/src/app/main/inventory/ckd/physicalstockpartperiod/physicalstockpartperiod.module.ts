import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PhysicalStockPartPeriodRoutingModule } from './physicalstockpartperiod-routing.module';
import { PhysicalStockPartPeriodComponent } from './physicalstockpartperiod.component';
import { CreateOrEditPhysicalStockPartPeriodModalComponent } from './create-or-edit-physicalstockpartperiod-modal.component';

@NgModule({
    declarations: [
       PhysicalStockPartPeriodComponent, 
        CreateOrEditPhysicalStockPartPeriodModalComponent
      
    ],
    imports: [
        AppSharedModule, PhysicalStockPartPeriodRoutingModule]
})
export class PhysicalStockPartPeriodModule {
}