import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GpsSupplierInfoRoutingModule } from './gpssupplierinfo-routing.module';
import { GpsSupplierInfoComponent } from './gpssupplierinfo.component';
import { CreateOrEditGpsSupplierInfoModalComponent } from './create-or-edit-gpssupplierinfo-modal.component';
import { CreateOrEditGpsSupplierOrderTimeModalComponent } from './create-or-edit-gpssupplierordertime-modal.component';

@NgModule({
    declarations: [
        GpsSupplierInfoComponent,
        CreateOrEditGpsSupplierInfoModalComponent,
        CreateOrEditGpsSupplierOrderTimeModalComponent
    ],
    imports: [
        AppSharedModule, GpsSupplierInfoRoutingModule]
})
export class GpsSupplierInfoModule {}
