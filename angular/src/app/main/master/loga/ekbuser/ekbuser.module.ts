import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkbUserRoutingModule } from './ekbuser-routing.module';
import { EkbUserComponent } from './ekbuser.component';
import { CreateOrEditEkbUserModalComponent } from './create-or-edit-ekbuser-modal.component';

@NgModule({
    declarations: [
       EkbUserComponent,
        CreateOrEditEkbUserModalComponent

    ],
    imports: [
        AppSharedModule, EkbUserRoutingModule]
})
export class EkbUserModule {}
