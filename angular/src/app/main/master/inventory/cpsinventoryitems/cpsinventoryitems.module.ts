import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CpsInventoryItemsRoutingModule } from './cpsinventoryitems-routing.module';
import { CpsInventoryItemsComponent } from './cpsinventoryitems.component';

@NgModule({
    declarations: [
       CpsInventoryItemsComponent
    ],
    imports: [
        AppSharedModule, CpsInventoryItemsRoutingModule]
})
export class CpsInventoryItemsModule {}
