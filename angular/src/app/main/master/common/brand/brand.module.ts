import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BrandRoutingModule } from './brand-routing.module';
import { BrandComponent } from './brand.component';
import { CreateOrEditBrandModalComponent } from './create-or-edit-brand-modal.component';

@NgModule({
    declarations: [
       BrandComponent,
        CreateOrEditBrandModalComponent
    ],
    imports: [
        AppSharedModule, BrandRoutingModule]
})
export class BrandModule {}
