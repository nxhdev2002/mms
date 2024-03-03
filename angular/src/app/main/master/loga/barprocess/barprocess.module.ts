import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BarProcessRoutingModule } from './barprocess-routing.module';
import { BarProcessComponent } from './barprocess.component';
import { CreateOrEditBarProcessModalComponent } from './create-or-edit-barprocess-modal.component';

@NgModule({
    declarations: [
       BarProcessComponent,
        CreateOrEditBarProcessModalComponent

    ],
    imports: [
        AppSharedModule, BarProcessRoutingModule]
})
export class BarProcessModule {}
