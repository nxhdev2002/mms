import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomsLeadTimeComponent } from './customsleadtimemaster.component';
import { CustomsLeadTimeRoutingModule } from './customsleadtimemaster-routing.module';
import { ImportCustomsLeadTimeComponent } from './import-customsleadtimemaster.component';
import { CreateOrEditCustomsLeadTimeMasterModalComponent } from './create-or-edit-customsleadtimemaster-modal.component';



@NgModule({
    declarations: [
        CustomsLeadTimeComponent,ImportCustomsLeadTimeComponent,
        CreateOrEditCustomsLeadTimeMasterModalComponent
    ],
    imports: [
        AppSharedModule, CustomsLeadTimeRoutingModule]
})
export class CustomsLeadTimeModule { }
