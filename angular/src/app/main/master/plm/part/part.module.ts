import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PartRoutingModule } from './part-routing.module';
import { PartComponent } from './part.component';
import { CreateOrEditPartModalComponent } from './create-or-edit-part-modal.component';

@NgModule({
    declarations: [
       PartComponent, 
        CreateOrEditPartModalComponent
      
    ],
    imports: [AppSharedModule, PartRoutingModule]
})
export class PartModule {}
