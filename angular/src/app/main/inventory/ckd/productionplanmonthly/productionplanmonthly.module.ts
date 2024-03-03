import { NgModule } from '@angular/core';
import { ProductionPlanMonthlyComponent } from './productionplanmonthly.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ProductionPlanMonthlyRoutingModule } from './productionplanmonthly-routing.component';
import { ImportProductionPlanMonthlyComponent } from './import-productionplanmonthly.component';
import { ListErrorImportProductionPlanMonthlyComponent } from './list-error-import-productionplanmonthly-modal.component';

@NgModule({
  imports: [
    AppSharedModule,
    ProductionPlanMonthlyRoutingModule
  ],
  declarations: [ProductionPlanMonthlyComponent,
    ImportProductionPlanMonthlyComponent,
    ListErrorImportProductionPlanMonthlyComponent]
})
export class ProductionPlanMonthlyModule { }
