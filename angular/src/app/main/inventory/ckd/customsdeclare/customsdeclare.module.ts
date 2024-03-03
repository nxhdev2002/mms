import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { CustomsDeclareRoutingModule } from './customsdeclare-routing.module';
import { CustomsDeclareModalComponent } from './customsdeclare.component';

@NgModule({
    declarations: [
        CustomsDeclareModalComponent
      
    ],
    imports: [
        AppSharedModule, CustomsDeclareRoutingModule]
})
export class CustomsDeclareModule {}
