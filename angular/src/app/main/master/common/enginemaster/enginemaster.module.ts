import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EngineMasterRoutingModule } from './enginemaster-routing.module';
import { EngineMasterComponent } from './enginemaster.component';
import { CreateOrEditEngineMasterModalComponent } from './create-or-edit-enginemaster-modal.component';
import { ViewHistoryEngineMasterModalComponent } from './history-enginemaster-modal.component';

@NgModule({
    declarations: [
       EngineMasterComponent, 
        CreateOrEditEngineMasterModalComponent,
        ViewHistoryEngineMasterModalComponent
    ],
    imports: [
        AppSharedModule, EngineMasterRoutingModule]
})
export class EngineMasterModule {}
