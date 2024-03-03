import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsMasterComponent } from './gpsmaster.component';
import { GpsMasterRoutingModule } from './gpsmaster-routing.module';
import { CreateOrEditGpsMasterModalComponent } from './create-or-edit-invgpsmaster-modal.component';
import { ImportGpsMasterComponent } from './import-gpsmaster.component';

@NgModule({
    declarations: [
       GpsMasterComponent,
       CreateOrEditGpsMasterModalComponent,
       ImportGpsMasterComponent
    ],
    imports: [
        AppSharedModule, GpsMasterRoutingModule],
        schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsMaster {}
