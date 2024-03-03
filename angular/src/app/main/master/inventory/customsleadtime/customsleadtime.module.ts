import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CustomsLeadtimeRoutingModule } from './customsleadtime-routing.module';
import { CustomsLeadtimeComponent } from './customsleadtime.component';
import { CreateOrEditCustomsLeadtimeModalComponent } from './create-or-edit-customsleadtime-modal.component';

@NgModule({
    declarations: [
       CustomsLeadtimeComponent, 
        CreateOrEditCustomsLeadtimeModalComponent
      
    ],
    imports: [
        AppSharedModule, CustomsLeadtimeRoutingModule]
})
export class CustomsLeadtimeModule {}
