import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerTransitPortPlanRoutingModule } from './containertransitportplan-routing.module';
import { ContainerTransitPortPlanComponent } from './containertransitportplan.component';
import { CreateOrEditContainerTransitPortPlanModalComponent } from './create-or-edit-containertransitportplan-modal.component';
import { ImportContainerTransitPortPlanComponent } from './import-containertransitportplan-modal.component';
import { ListErrorImportContainerRentalWHPlanComponent } from './list-error-import-containertransitportplan-modal.component';

@NgModule({
    declarations: [
       ContainerTransitPortPlanComponent, 
        CreateOrEditContainerTransitPortPlanModalComponent,
       ImportContainerTransitPortPlanComponent,
       ListErrorImportContainerRentalWHPlanComponent
    ],
    imports: [
        AppSharedModule, ContainerTransitPortPlanRoutingModule]
})
export class ContainerTransitPortPlanModule {}
