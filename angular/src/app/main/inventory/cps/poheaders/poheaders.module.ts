import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PoHeadersRoutingModule } from './poheaders-routing.module';
import { PoHeadersComponent } from './poheaders.component';

@NgModule({
    declarations: [
       PoHeadersComponent,

    ],
    imports: [
        AppSharedModule, PoHeadersRoutingModule]
})
export class PoHeadersModule {}
