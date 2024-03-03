import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GenBomDataRoutingModule } from './genbomdata-routing.module';
import { GenBomDataComponent } from './genbomdata.component';

@NgModule({
    declarations: [
        GenBomDataComponent
    ],
    imports: [
        AppSharedModule, GenBomDataRoutingModule]
})
export class GenBomDataModule {}
