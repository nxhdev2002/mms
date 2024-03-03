import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EngineModelRoutingModule } from './enginemodel-routing.module';
import { EngineModelComponent } from './enginemodel.component';

@NgModule({
    declarations: [
       EngineModelComponent
    ],
    imports: [
        AppSharedModule, EngineModelRoutingModule]
})
export class EngineModelModule {}
