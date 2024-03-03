import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { StorageLocationCategoryRoutingModule } from './storagelocationcategory-routing.module';
import { StorageLocationCategoryComponent } from './storagelocationcategory.component';

@NgModule({
    declarations: [
       StorageLocationCategoryComponent
    ],
    imports: [
        AppSharedModule, StorageLocationCategoryRoutingModule]
})
export class StorageLocationCategoryModule {}
