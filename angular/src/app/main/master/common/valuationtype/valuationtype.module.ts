import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ValuationTypeRoutingModule } from './valuationtype-routing.module';
import { ValuationTypeComponent } from './valuationtype.component';
import { CreateOrEditValuationTypeModalComponent } from './create-or-edit-valuationtype-modal.component';

@NgModule({
    declarations: [
       ValuationTypeComponent, 
        CreateOrEditValuationTypeModalComponent
      
    ],
    imports: [
        AppSharedModule, ValuationTypeRoutingModule]
})
export class ValuationTypeModule {}
