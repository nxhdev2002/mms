import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsMaterialRegisterByShopComponent } from './gpsmaterialregisterbyshop.component';
import { GpsMaterialRegisterByShopRoutingModule } from './gpsmaterialregisterbyshop-routing.module';
import { CreateOrEditGpsMaterialRegisterByShopModalComponent } from './create-or-edit-gpsmaterialregisterbyshop-modal.component';
import { ImportGpsMaterialRegisterByShopComponent } from './import-gpsmaterialregisterbyshop.component';


@NgModule({
    declarations: [
        GpsMaterialRegisterByShopComponent,
        CreateOrEditGpsMaterialRegisterByShopModalComponent,
        ImportGpsMaterialRegisterByShopComponent
    ],
    imports: [
        AppSharedModule,  GpsMaterialRegisterByShopRoutingModule],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsMaterialRegisterByShopModule {}
