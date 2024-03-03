import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ShippingScheduleFirmRoutingModule } from './shippingschedulefirm-routing.module';
import { ShippingScheduleFirmComponent } from './shippingschedulefirm.component';
import { CreateOrEditShippingScheduleDetailsFirmModalComponent } from './create-or-edit-shippingscheduledetailsfirm-modal.component';
import { ProdTabsModule } from '@app/shared/common/production-tabs/tabs.module';
import { ButtonRendererComponent } from './renderer/button-renderer.component';
import { AgGridModule } from '@ag-grid-community/angular';

@NgModule({
    declarations: [
        ShippingScheduleFirmComponent,
        CreateOrEditShippingScheduleDetailsFirmModalComponent,
        ButtonRendererComponent
    ],
    imports: [
        AppSharedModule, ShippingScheduleFirmRoutingModule, ProdTabsModule, AgGridModule.withComponents([ButtonRendererComponent])],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ShippingScheduleFirmModule {}
