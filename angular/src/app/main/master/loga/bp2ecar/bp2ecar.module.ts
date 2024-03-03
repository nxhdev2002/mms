import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { Bp2EcarRoutingModule } from './bp2ecar-routing.module';
import { Bp2EcarComponent } from './bp2ecar.component';
import { CreateOrEditBp2EcarModalComponent } from './create-or-edit-bp2ecar-modal.component';

@NgModule({
    declarations: [
        Bp2EcarComponent,
        CreateOrEditBp2EcarModalComponent

    ],
    imports: [
        AppSharedModule, Bp2EcarRoutingModule]
})
export class Bp2EcarModule {}
