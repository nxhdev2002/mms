import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkbProcessRoutingModule } from './ekbprocess-routing.module';
import { EkbProcessComponent } from './ekbprocess.component';
import { CreateOrEditEkbProcessModalComponent } from './create-or-edit-ekbprocess-modal.component';

@NgModule({
    declarations: [
       EkbProcessComponent,
        CreateOrEditEkbProcessModalComponent

    ],
    imports: [
        AppSharedModule, EkbProcessRoutingModule]
})
export class EkbProcessModule {}
