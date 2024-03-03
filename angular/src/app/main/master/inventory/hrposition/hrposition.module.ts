import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { HrPositionRoutingModule } from './hrposition-routing.module';
import { HrPositionComponent } from './hrposition.component';
import { ViewHrPositionModalComponent } from './view-hrposition-modal.component';

@NgModule({
    declarations: [
       HrPositionComponent, 
       ViewHrPositionModalComponent
    ],
    imports: [
        AppSharedModule, HrPositionRoutingModule]
})
export class HrPositionModule {}
