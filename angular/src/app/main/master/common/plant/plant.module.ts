import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PlantRoutingModule } from './plant-routing.module';
import { PlantComponent } from './plant.component';

@NgModule({
    declarations: [
       PlantComponent
    ],
    imports: [
        AppSharedModule, PlantRoutingModule]
})
export class PlantModule {}
