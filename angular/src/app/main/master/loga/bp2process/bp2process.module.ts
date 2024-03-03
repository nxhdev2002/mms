import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { Bp2ProcessRoutingModule } from './bp2process-routing.module';
import { Bp2ProcessComponent } from './bp2process.component';
import { CreateOrEditBp2ProcessModalComponent } from './create-or-edit-bp2process-modal.component';
import { ImportBp2ProcessComponent } from './import-bp2process-modal.component';

@NgModule({
    declarations: [
       Bp2ProcessComponent, 
        CreateOrEditBp2ProcessModalComponent,
        ImportBp2ProcessComponent
      
    ],
    imports: [
        AppSharedModule, Bp2ProcessRoutingModule]
})
export class Bp2ProcessModule {}
