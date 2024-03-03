import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SmqdOrderRoutingModule } from './smqdorder-routing.module';
import { SmqdOrderComponent } from './smqdorder.component';
import { ImportSmqdOrderComponent } from './import-smqdorder-modal.component';
import { ListErrorImportSmqdOrderComponent } from './list-error-import-smqdorder-modal.component';
import { CreateOrEditInvCkdSmqdOderModalComponent } from './create-or-edit-smqdorder-modal.component';
@NgModule({
    declarations: [
       SmqdOrderComponent,
       ImportSmqdOrderComponent,
       ListErrorImportSmqdOrderComponent,
       CreateOrEditInvCkdSmqdOderModalComponent
    ],
    imports: [
        AppSharedModule, SmqdOrderRoutingModule]
})
export class SmqdOrderModule {}
