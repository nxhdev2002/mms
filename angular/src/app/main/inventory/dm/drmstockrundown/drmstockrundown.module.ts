import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { DrmStockRundownRoutingModule } from './drmstockrundown-routing.module';
import { DrmStockRundownComponent } from './drmstockrundown.component';

@NgModule({
    declarations: [
       DrmStockRundownComponent  
    ],
    imports: [
        AppSharedModule, DrmStockRundownRoutingModule]
})
export class DrmStockRundownModule {}
