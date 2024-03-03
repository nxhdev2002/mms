import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ImportInvCkdPhysicalStockPartS4Component } from './import-physical-stock-part-s4.component';
import { ListErrorImportInvCkdPhysicalStockPartS4Component } from './list-error-import-physical-stock-part-s4-modal.component';
import { PhysicalStockPartS4RoutingModule } from './physical-stock-part-s4-routing.module';
import { PhysicalStockPartS4Component } from './physical-stock-part-s4.component';


@NgModule({
    declarations: [
       PhysicalStockPartS4Component,
        ImportInvCkdPhysicalStockPartS4Component,
        ListErrorImportInvCkdPhysicalStockPartS4Component

    ],
    imports: [
        AppSharedModule, PhysicalStockPartS4RoutingModule]
})
export class PhysicalStockPartModule {
}
