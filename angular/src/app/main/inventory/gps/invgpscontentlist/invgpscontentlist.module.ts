import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvGpsContentListRoutingModule } from './invgpscontentlist-routing.module';
import { InvGpsContentListComponent } from './invgpscontentlist.component';
import { CreateOrEditInvGpsContentListModalComponent } from './create-or-edit-invgpscontentlist-modal.component';

@NgModule({
    declarations: [
       InvGpsContentListComponent, 
        CreateOrEditInvGpsContentListModalComponent
      
    ],
    imports: [
        AppSharedModule, InvGpsContentListRoutingModule]
})
export class InvGpsContentListModule {}
