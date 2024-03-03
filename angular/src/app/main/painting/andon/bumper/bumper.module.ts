import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BumperRoutingModule } from './bumper-routing.module';
import { BumperComponent } from './bumper.component';


@NgModule({
    declarations: [
       BumperComponent,

    ],
    imports: [
        AppSharedModule, BumperRoutingModule]
})
export class BumperModule {}
