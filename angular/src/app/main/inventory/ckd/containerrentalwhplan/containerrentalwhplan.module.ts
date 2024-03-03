import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerRentalWHPlanRoutingModule } from './containerrentalwhplan-routing.module';
import { ContainerRentalWHPlanComponent } from './containerrentalwhplan.component';
import { CreateOrEditContainerRentalWHPlanModalComponent } from './create-or-edit-containerrentalwhplan-modal.component';
import { ImportContainerRentalWHPlanComponent } from './import-containerrentalwhplan-modal.component';
import { ListErrorImportContainerRentalWHPlanComponent } from './list-error-import-containerrentalwhplan-modal.component';
import { ImportRepackComponent } from './import-repack-modal.component';

@NgModule({
    declarations: [
       ContainerRentalWHPlanComponent,
       CreateOrEditContainerRentalWHPlanModalComponent,
       ImportContainerRentalWHPlanComponent,
       ImportRepackComponent,
       ListErrorImportContainerRentalWHPlanComponent
    ],
    imports: [
        AppSharedModule,
        ContainerRentalWHPlanRoutingModule,
        ]
})
export class ContainerRentalWHPlanModule {}
