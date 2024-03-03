import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkbPartListRoutingModule } from './ekbpartlist-routing.module';
import { EkbPartListComponent } from './ekbpartlist.component';
import { CreateOrEditEkbPartListModalComponent } from './create-or-edit-ekbpartlist-modal.component';

@NgModule({
    declarations: [
       EkbPartListComponent,
        CreateOrEditEkbPartListModalComponent

    ],
    imports: [
        AppSharedModule, EkbPartListRoutingModule]
})
export class EkbPartListModule {}
