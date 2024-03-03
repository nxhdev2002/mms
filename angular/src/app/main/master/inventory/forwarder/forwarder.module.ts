import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ForwarderRoutingModule } from './forwarder-routing.module';
import { ForwarderComponent } from './forwarder.component';

@NgModule({
    declarations: [
        ForwarderComponent


    ],
    imports: [
        AppSharedModule, ForwarderRoutingModule]
})
export class ForwarderModule { }
