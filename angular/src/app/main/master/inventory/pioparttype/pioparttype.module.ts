import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PIOPartTypeRoutingModule } from './pioparttype-routing.module';
import { PIOPartTypeComponent } from './pioparttype.component';
import { CreateOrEditPIOPartTypeModalComponent } from './create-or-edit-pioparttype-modal.component';

@NgModule({
    declarations: [
       PIOPartTypeComponent, 
        CreateOrEditPIOPartTypeModalComponent
      
    ],
    imports: [
        AppSharedModule, PIOPartTypeRoutingModule]
})
export class PIOPartTypeModule {}
