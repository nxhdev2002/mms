import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PIOEmailRoutingModule } from './pioemail-routing.module';
import { PIOEmailComponent } from './pioemail.component';
import { CreateOrEditPIOEmailModalComponent } from './create-or-edit-pioemail-modal.component';

@NgModule({
    declarations: [
       PIOEmailComponent, 
        CreateOrEditPIOEmailModalComponent
      
    ],
    imports: [
        AppSharedModule, PIOEmailRoutingModule]
})
export class PIOEmailModule {}
