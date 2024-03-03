import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomsStatusRoutingModule } from './customsstatus-routing.module';
import { CustomsStatusComponent } from './customsstatus.component';

@NgModule({
    declarations: [
        CustomsStatusComponent


    ],
    imports: [
        AppSharedModule, CustomsStatusRoutingModule]
})
export class CustomsStatusModule { }
