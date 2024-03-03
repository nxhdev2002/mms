import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ValuationClassRoutingModule } from './valuationclass-routing.module';
import { ValuationClassComponent } from './valuationclass.component';
import { CreateOrEditValuationClassModalComponent } from './create-or-edit-valuationclass-modal.component';

@NgModule({
    declarations: [
       ValuationClassComponent, 
        CreateOrEditValuationClassModalComponent
      
    ],
    imports: [
        AppSharedModule, ValuationClassRoutingModule]
})
export class ValuationClassModule {}
