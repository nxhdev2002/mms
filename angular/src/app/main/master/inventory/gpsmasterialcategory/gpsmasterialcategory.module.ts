import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsMasterialCategoryComponent } from './gpsmasterialcategory.component';
import {  GpsMasterialCategoryRoutingModule } from './gpsmasterialcategory-routing.module';
import { CreateOrEditGpsMasterialCategoryModalComponent } from './create-or-edit-gpsmasterialcategory-modal.component';



@NgModule({
    declarations: [
        GpsMasterialCategoryComponent,
        CreateOrEditGpsMasterialCategoryModalComponent

    ],
    imports: [
        AppSharedModule,  GpsMasterialCategoryRoutingModule],
        schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsMasterialCategoryModule {}
