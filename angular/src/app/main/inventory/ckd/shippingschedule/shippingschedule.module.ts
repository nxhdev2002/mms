import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ShippingScheduleComponent } from './shippingschedule.component';
import { ShippingScheduleRoutingModule } from './shippingschedule-routing.module';
import { ImportShippingScheduleComponent } from './import-shippingschedule.component';
import { ButtonRendererComponent } from './renderer/button-renderer.component';
import { AgGridModule } from '@ag-grid-community/angular';


@NgModule({
    declarations: [
        ShippingScheduleComponent,
        ImportShippingScheduleComponent,
        ButtonRendererComponent
    ],
    imports: [
        AppSharedModule,
        ShippingScheduleRoutingModule,
        AgGridModule.withComponents([ButtonRendererComponent])
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ShippingScheduleModule {
}


