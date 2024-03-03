import {CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsCostCenterComponent } from './gpscostcenter.component';
import { GpsCostCenterRoutingModule } from './gpscostcenter-routing.module';
import { CreateOrEditGpsCostCenterModalComponent } from './create-or-edit-gpscostcenter-modal.component';
import { ImportGpsCostCenterComponent } from './import-gpscostcenter.component';


@NgModule({
    declarations: [
        GpsCostCenterComponent,
        CreateOrEditGpsCostCenterModalComponent,
        ImportGpsCostCenterComponent
    ],
    imports: [
        AppSharedModule,  GpsCostCenterRoutingModule],
        schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class GpsMaterialRegisterByShopModule {}
