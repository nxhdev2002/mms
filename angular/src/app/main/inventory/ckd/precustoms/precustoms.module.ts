import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PreCustomsRoutingModule } from './precustoms-routing.module';
import { PreCustomsComponent } from './precustoms.component';

@NgModule({
    declarations: [
       PreCustomsComponent

    ],
    imports: [
        AppSharedModule, PreCustomsRoutingModule]
})
export class PreCustomsModule {}
