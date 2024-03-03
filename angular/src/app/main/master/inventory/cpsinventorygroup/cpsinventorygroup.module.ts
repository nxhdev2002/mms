import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CpsInventoryGroupRoutingModule } from './cpsinventorygroup-routing.module';
import { CpsInventoryGroupComponent } from './cpsinventorygroup.component';

@NgModule({
    declarations: [
       CpsInventoryGroupComponent, 
        
      
    ],
    imports: [
        AppSharedModule, CpsInventoryGroupRoutingModule]
})
export class CpsInventoryGroupModule {}
