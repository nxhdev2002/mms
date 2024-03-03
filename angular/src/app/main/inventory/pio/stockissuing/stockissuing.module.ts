import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockIssuingRoutingModule } from './stockissuing-routing.module';
import { StockIssuingComponent } from './stockissuing.component';

@NgModule({
    declarations: [
       StockIssuingComponent, 
    ],
    imports: [
        AppSharedModule, StockIssuingRoutingModule]
})
export class StockIssuingModule {}
