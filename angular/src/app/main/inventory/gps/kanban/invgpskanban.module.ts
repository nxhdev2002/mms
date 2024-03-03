import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvGpsKanbanRoutingModule } from './invgpskanban-routing.module';
import { InvGpsKanbanComponent } from './invgpskanban.component';
import { CreateOrEditInvGpsKanbanModalComponent } from './create-or-edit-invgpskanban-modal.component';

@NgModule({
    declarations: [
       InvGpsKanbanComponent, 
        CreateOrEditInvGpsKanbanModalComponent
      
    ],
    imports: [
        AppSharedModule, InvGpsKanbanRoutingModule]
})
export class InvGpsKanbanModule {}