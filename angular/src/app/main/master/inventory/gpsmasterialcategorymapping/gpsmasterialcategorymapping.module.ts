import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsMasterialCategoryMappingComponent } from './gpsmasterialcategorymapping.component';
import {  GpsMasterialCategoryMappingRoutingModule } from './gpsmasterialcategorymapping-routing.module';
import { CreateOrEditGpsMasterialCategoryMappingModalComponent } from './create-or-edit-gpsmasterialcategorymapping-modal.component';
import { ImportGpsMaterialCategoryMappingComponent } from './import-gpsmasterialcategorymapping.component';
import { ViewHistoryMaterialCategoryMappingModalComponent } from './history-masterialcategorymapping-modal.component';



@NgModule({
    declarations: [
        GpsMasterialCategoryMappingComponent,
        CreateOrEditGpsMasterialCategoryMappingModalComponent,
        ImportGpsMaterialCategoryMappingComponent,
        ViewHistoryMaterialCategoryMappingModalComponent
    ],
    imports: [
        AppSharedModule,  GpsMasterialCategoryMappingRoutingModule],
        schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsMasterialCategoryMappingModule {}
