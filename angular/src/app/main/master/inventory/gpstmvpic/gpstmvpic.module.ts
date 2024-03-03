import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsTmvPicRoutingModule } from './gpstmvpic-routing.module';
import { GpsTmvPicComponent } from './gpstmvpic.component';
import { CreateOrEditGpsTmvPicModalComponent } from './create-or-edit-gpstmvpic-modal.component';

@NgModule({
    declarations: [
       GpsTmvPicComponent,
        CreateOrEditGpsTmvPicModalComponent

    ],
    imports: [
        AppSharedModule, GpsTmvPicRoutingModule]
})
export class GpsTmvPicModule {}
