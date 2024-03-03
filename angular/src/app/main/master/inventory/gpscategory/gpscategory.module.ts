import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsCategoryRoutingModule } from './gpscategory-routing.module';
import { GpsCategoryComponent } from './gpscategory.component';
import { NgModule } from '@angular/core';


@NgModule({
    declarations: [
        GpsCategoryComponent,

    ],
    imports: [
        AppSharedModule,  GpsCategoryRoutingModule]
})
export class GpsCategoryModule {}
