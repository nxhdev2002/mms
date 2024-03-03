import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StockRundownRoutingModule } from './stockrundown-routing.module';
import { StockRundownComponent } from './stockrundown.component';

@NgModule({
    declarations: [
        StockRundownComponent

    ],
    imports: [
        AppSharedModule, StockRundownRoutingModule]
})
export class StockRundownModule { }
