import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvGpsDailyOrderRoutingModule } from './invgpsdailyorder-routing.module';
import { InvGpsDailyOrderComponent } from './invgpsdailyorder.component';
import { CreateOrEditInvGpsDailyOrderModalComponent } from './create-or-edit-invgpsdailyorder-modal.component';

@NgModule({
    declarations: [
        InvGpsDailyOrderComponent,
        CreateOrEditInvGpsDailyOrderModalComponent

    ],
    imports: [
        AppSharedModule, InvGpsDailyOrderRoutingModule]
})
export class InvGpsDailyOrderModule { }
