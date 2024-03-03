import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CostOfSaleSummaryComponent } from './costofsalesummary.component';
import { CostOfSaleSummaryRoutingModule } from './costofsalesummary-routing.module';

@NgModule({
    declarations: [
        CostOfSaleSummaryComponent
    ],
    imports: [
        AppSharedModule,
        CostOfSaleSummaryRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CostOfSaleSummaryModule { }
