import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StockRoutingModule } from './stock-routing.module';
import { StockComponent } from './stock.component';

@NgModule({
    declarations: [
       StockComponent, 
    ],
    imports: [
        AppSharedModule, StockRoutingModule]
})
export class StockModule {}
