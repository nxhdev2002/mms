import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { FrameEngineRoutingModule } from './frameengine-routing.module';
import { FrameEngineComponent } from './frameengine.component';

@NgModule({
    declarations: [
       FrameEngineComponent, 
      
    ],
    imports: [
        AppSharedModule, FrameEngineRoutingModule]
})
export class FrameEngineModule {}
