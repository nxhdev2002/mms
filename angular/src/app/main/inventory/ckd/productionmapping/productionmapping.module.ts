import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ProductionMappingRoutingModule } from './productionmapping-routing.module';
import { ProductionMappingComponent } from './productionmapping.component';
import { ImportProductionMappingComponent } from './import-production-mapping.component';
import { ListErrorImportProductionMappingComponent } from './list-error-import-production-mapping-modal.component';
import { ViewHistoryProductionMappingModalComponent } from './history-production-mapping-modal.component';

@NgModule({
    declarations: [
       ProductionMappingComponent,
        ImportProductionMappingComponent,
        ListErrorImportProductionMappingComponent,
        ViewHistoryProductionMappingModalComponent
    ],
    imports: [
        AppSharedModule, ProductionMappingRoutingModule]
})
export class ProductionMappingModule {}
