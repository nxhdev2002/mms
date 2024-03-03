import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CarSeriesRoutingModule } from './carseries-routing.module';
import { CarSeriesComponent } from './carseries.component';


@NgModule({
    declarations: [
        CarSeriesComponent,

    ],
    imports: [
        AppSharedModule,  CarSeriesRoutingModule]
})
export class CarSeriesModule {}
