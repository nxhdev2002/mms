import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { HrOrgStructureRoutingModule } from './hrorgstructure-routing.module';
import { HrOrgStructureComponent } from './hrorgstructure.component';

@NgModule({
    declarations: [
       HrOrgStructureComponent,

    ],
    imports: [
        AppSharedModule, HrOrgStructureRoutingModule]
})
export class HrOrgStructureModule {}
