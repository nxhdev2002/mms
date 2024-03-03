import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BumperInRoutingModule } from './bumperin-routing.module';
import { BumperInComponent } from './bumperin.component';
import { ConfirmPartModalComponent } from "./confirmpart-modal.component";

@NgModule({
    declarations: [
        BumperInComponent,
        ConfirmPartModalComponent
    ],
    imports: [
        BumperInRoutingModule,
        AppSharedModule,
    ]
})
export class BumperInModule {}




