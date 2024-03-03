import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { IssuingRoutingModule } from './issuing-routing.module';
import { IssuingComponent } from './issuing.component';
import { CreateOrEditIssuingModalComponent } from './create-or-edit-issuing-modal.component';
import { ImportInvGpsIssuingComponent } from './import-issuing-modal.component';
import { ListErrorImportGpsIssuingComponent } from './list-error-import-issuing-modal.component';
import { AppComponentBase } from '@shared/common/app-component-base';

@NgModule({
    declarations: [
       IssuingComponent, 
        CreateOrEditIssuingModalComponent,
        ImportInvGpsIssuingComponent,
        ListErrorImportGpsIssuingComponent,
        
    ],
    imports: [
        AppSharedModule, IssuingRoutingModule]
})
export class IssuingModule {}
