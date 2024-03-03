import {CUSTOM_ELEMENTS_SCHEMA,NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { VehicleDetailsRoutingModule } from './vehicledetails-routing.module';
import { VehicleDetailsComponent } from './vehicledetails.component';
import { TABS } from '@app/shared/constants/tab-keys';
import { ViewVehicleDetailModalComponent } from './view-vehicle-details-modal.component';
import { ViewHistoryVehicleDetailsComponent } from './history-vehicledetails-modal.component';

const tabcode_component_dict = {
    [TABS.ASSY_ADO_VEHICLEDETAILS]: VehicleDetailsComponent
};

@NgModule({
    declarations: [
       VehicleDetailsComponent,
       ViewVehicleDetailModalComponent,
       ViewHistoryVehicleDetailsComponent
    ],
    imports: [
        AppSharedModule, VehicleDetailsRoutingModule],
        schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class VehicleDetailsModule {
    static getComponent(tabCode: string) {
        return tabcode_component_dict[tabCode];
    }
}
