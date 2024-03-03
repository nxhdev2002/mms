import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ExchangeRateRoutingModule } from './exchangerate-routing.module';
import { ExchangeRateComponent } from './exchangerate.component';
import { ViewDiffExchangeRateComponent } from './view-diffexchangerate-modal.component';
import { ViewHistoryExchangeRateModalComponent } from './history-exchangerate-modal.component';

@NgModule({
    declarations: [
       ExchangeRateComponent,
       ViewDiffExchangeRateComponent,
       ViewHistoryExchangeRateModalComponent
      
    ],
    imports: [
        AppSharedModule, ExchangeRateRoutingModule]
})
export class ExchangeRateModule {}
