import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PhysicalConfirmLotRoutingModule } from './physicalconfirmlot-routing.module';
import { PhysicalConfirmLotComponent } from './physicalconfirmlot.component';
import { ImportPhysicalConfirmLotComponent } from './import-physical-confirm-lot.component';
import { ListErrorImportPhysicalConfirmLotComponent } from './list-error-import-physicalconfirmlot-modal.component';

@NgModule({
    declarations: [
       PhysicalConfirmLotComponent,
        ImportPhysicalConfirmLotComponent,
        ListErrorImportPhysicalConfirmLotComponent
    ],
    imports: [
        AppSharedModule, PhysicalConfirmLotRoutingModule]
})
export class PhysicalConfirmLotModule {}
