import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SignalRoutingModule } from './signal-routing.module';
import { SignalComponent } from './signal.component';


@NgModule({
    declarations: [
       SignalComponent

    ],
    imports: [
        AppSharedModule, SignalRoutingModule]
})
export class SignalModule {}
