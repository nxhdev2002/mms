import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CostRoutingModule } from './cost-routing.module';
import { CostComponent } from './cost.component';


@NgModule({
    declarations: [
        CostComponent
    ],
    imports: [
        AppSharedModule,
        CostRoutingModule
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CostModule { }
