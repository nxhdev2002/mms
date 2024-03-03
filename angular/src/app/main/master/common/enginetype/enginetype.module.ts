import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EngineTypeRoutingModule } from './enginetype-routing.module';
import { EngineTypeComponent } from './enginetype.component';

@NgModule({
    declarations: [
       EngineTypeComponent
    ],
    imports: [
        AppSharedModule, EngineTypeRoutingModule]
})
export class EngineTypeModule {}
