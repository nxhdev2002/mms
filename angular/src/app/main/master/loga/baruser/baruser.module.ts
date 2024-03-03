import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BarUserRoutingModule } from './baruser-routing.module';
import { BarUserComponent } from './baruser.component';
import { CreateOrEditBarUserModalComponent } from './create-or-edit-baruser-modal.component';

@NgModule({
    declarations: [
       BarUserComponent,
        CreateOrEditBarUserModalComponent

    ],
    imports: [
        AppSharedModule, BarUserRoutingModule]
})
export class BarUserModule {}
