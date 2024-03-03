import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsStockRundownRoutingModule } from './gpsstockrundown-routing.module';
import { GpsStockRundownComponent } from './gpsstockrundown.component';


@NgModule({
    declarations: [
       GpsStockRundownComponent,
    ],
    imports: [
        AppSharedModule, GpsStockRundownRoutingModule]
})
export class GpsStockRundownModule {}
