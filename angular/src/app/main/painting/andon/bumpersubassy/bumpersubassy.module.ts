import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BumperSubAssyRoutingModule } from './bumpersubassy-routing.module';
import { BumperSubAssyComponent } from './bumpersubassy.component';
import { ConfirmPartAssyModalComponent } from './confirmpart-modal.component';


@NgModule({
    declarations: [
        BumperSubAssyComponent,ConfirmPartAssyModalComponent,
    ],
    imports: [
        BumperSubAssyRoutingModule,AppSharedModule]
})
export class BumperSubAssyModule {}



