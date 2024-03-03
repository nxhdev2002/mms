import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { DevanningCaseTypeRoutingModule } from './devanningcasetype-routing.module';
import { DevanningCaseTypeComponent } from './devanningcasetype.component';
import { CreateOrEditDevanningCaseTypeModalComponent } from './create-or-edit-devanningcasetype-modal.component';
import { ViewHistoryDevanningCaseTypeModalComponent} from './history-devanningcasetype-modal.component'

@NgModule({
    declarations: [
        DevanningCaseTypeComponent, 
        CreateOrEditDevanningCaseTypeModalComponent,
        ViewHistoryDevanningCaseTypeModalComponent

    ],
    imports: [
        AppSharedModule, DevanningCaseTypeRoutingModule]
})
export class DevanningCaseTypeModule {}
