import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsStockRoutingModule } from './gpsstock-routing.module';
import { GpsStockComponent } from './gpsstock.component';
import { ViewGPSStockPartDetailModalComponent } from './view-gpsstockpart-detail-modal.component';

@NgModule({
    declarations: [
       GpsStockComponent,
       ViewGPSStockPartDetailModalComponent
    ],
    imports: [
        AppSharedModule, GpsStockRoutingModule]
})
export class GpsStockModule {}
