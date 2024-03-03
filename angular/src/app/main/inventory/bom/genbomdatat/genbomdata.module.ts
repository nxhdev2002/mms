import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { GenBomDataComponent } from './genbomdata.component';
import { GenBomDataRoutingModule } from './genbomdata-routing.module';
import { ViewGenBomDataDetailModalComponent } from './view-genbomdata-detail-modal.component';

@NgModule({
    declarations: [
        GenBomDataComponent,
        ViewGenBomDataDetailModalComponent
    ],
    imports: [
        AppSharedModule, GenBomDataRoutingModule]
})
export class GenBomDataModule { }
