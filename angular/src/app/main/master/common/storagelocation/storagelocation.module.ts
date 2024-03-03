import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StorageLocationRoutingModule } from './storagelocation-routing.module';
import { StorageLocationComponent } from './storagelocation.component';


@NgModule({
    declarations: [
       StorageLocationComponent, 
       
      
    ],
    imports: [
        AppSharedModule, StorageLocationRoutingModule]
})
export class StorageLocationModule {}
