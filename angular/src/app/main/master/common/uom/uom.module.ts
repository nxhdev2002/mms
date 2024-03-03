import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { UomRoutingModule } from './uom-routing.module';
import { UomComponent } from './uom.component';
import { CreateOrEditUomModalComponent } from './create-or-edit-uom-modal.component';

@NgModule({
    declarations: [
       UomComponent, 
        CreateOrEditUomModalComponent
      
    ],
    imports: [
        AppSharedModule, UomRoutingModule]
})
export class UomModule {}
