import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SignalRoutingModule } from './signal-routing.module';
import { LogAPlcSignalComponent } from './signal.component';



@NgModule({
    declarations: [
        LogAPlcSignalComponent,

    ],
    imports: [
        AppSharedModule, SignalRoutingModule]
})
export class SignalModule {}