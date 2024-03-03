import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';import { GpsWbsCCMappingComponent } from './gpswbsccmapping.component';
import { GpsWbsCCMappingRoutingModule } from './gpswbsccmapping-routing.module';
import { CreateOrEditGpsWbsCCMappingModalComponent } from './create-or-edit-gpswbsccmapping-modal.component';
import { ImportGpsWbsCCMappingComponent } from './import-gpswbsccmapping.component';
import { ViewHistoryGpsWbsCCMappingModalComponent } from './history-gpswbsccmapping-modal.component'; 



@NgModule({
    declarations: [
        GpsWbsCCMappingComponent,
        CreateOrEditGpsWbsCCMappingModalComponent,
        ImportGpsWbsCCMappingComponent,
        ViewHistoryGpsWbsCCMappingModalComponent,
         
    ],
    imports: [
        AppSharedModule,  GpsWbsCCMappingRoutingModule],
        schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsWbsCCMappingModule {}
