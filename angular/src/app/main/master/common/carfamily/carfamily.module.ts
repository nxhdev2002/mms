import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CarfamilyRoutingModule } from './carfamily-routing.module';
import { CarfamilyComponent } from './carfamily.component';


@NgModule({
    declarations: [
       CarfamilyComponent, 
      
    ],
    imports: [
        AppSharedModule, CarfamilyRoutingModule]
})
export class CarfamilyModule {}
