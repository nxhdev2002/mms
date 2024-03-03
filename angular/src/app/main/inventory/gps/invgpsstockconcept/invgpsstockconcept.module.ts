import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvGpsStockConceptRoutingModule } from './invgpsstockconcept-routing.module';
import { InvGpsStockConceptComponent } from './invgpsstockconcept.component';
import { CreateOrEditInvGpsStockConceptModalComponent } from './create-or-edit-invgpsstockconcept-modal.component';
import { ImportInvGpsStockConceptComponent } from './import-invgpsstockconcept-modal';

@NgModule({
    declarations: [
       InvGpsStockConceptComponent, 
        CreateOrEditInvGpsStockConceptModalComponent,
        ImportInvGpsStockConceptComponent   
    ],
    imports: [
        AppSharedModule, InvGpsStockConceptRoutingModule]
})
export class InvGpsStockConceptModule {}
