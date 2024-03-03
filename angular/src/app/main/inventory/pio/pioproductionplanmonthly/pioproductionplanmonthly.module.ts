import { NgModule } from '@angular/core';
import { PioProductionPlanMonthlyComponent } from './pioproductionplanmonthly.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PioProductionPlanMonthlyRoutingModule } from './pioproductionplanmonthly-routing.component';
import { ImportPioProductionPlanMonthlyComponent } from './import-pioproductionplanmonthly.component';
import { ListErrorImportPioProductionPlanMonthlyComponent } from './list-error-import-pioproductionplanmonthly-modal.component';

@NgModule({
  imports: [
    AppSharedModule,
    PioProductionPlanMonthlyRoutingModule
  ],
  declarations: [PioProductionPlanMonthlyComponent,
    ImportPioProductionPlanMonthlyComponent,
    ListErrorImportPioProductionPlanMonthlyComponent,

]
})
export class PioProductionPlanMonthlyModule { }
