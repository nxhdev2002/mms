import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { DriveTrainRoutingModule } from './drivetrain-routing.module';
import { DriveTrainComponent } from './drivetrain-routing.module';


@NgModule({
    declarations: [
        DriveTrainComponent,

    ],
    imports: [
        AppSharedModule,  DriveTrainRoutingModule]
})
export class DriveTrainModule {}
