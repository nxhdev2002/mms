import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MATCustomsDeclareComponent } from './matcustomdeclare.component';
import { MATCustomsDeclareRoutingModule } from './matcustomdeclare-routing.module';


@NgModule({
    declarations: [
        MATCustomsDeclareComponent,
    ],
    imports: [
        AppSharedModule,
        MATCustomsDeclareRoutingModule
    ]
})
export class MATCustomsDeclareModule {
}


