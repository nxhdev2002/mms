import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CreateOrEditSpsRackModalComponent } from './create-or-edit-spsrack-modal.component';
import { SpsRackRoutingModule } from './spsrack-routing.module';
import { SpsRackComponent } from './spsrack.component';


@NgModule({
    declarations: [
       SpsRackComponent,
        CreateOrEditSpsRackModalComponent

    ],
    imports: [
        AppSharedModule, SpsRackRoutingModule]
})
export class SpsRackModule {}
