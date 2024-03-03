import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PcRackRoutingModule } from './pcrack-routing.module';
import { PcRackComponent } from './pcrack.component';
import { CreateOrEditPcRackModalComponent } from './create-or-edit-pcrack-modal.component';

@NgModule({
    declarations: [
       PcRackComponent,
        CreateOrEditPcRackModalComponent

    ],
    imports: [
        AppSharedModule, PcRackRoutingModule]
})
export class PcRackModule {}
