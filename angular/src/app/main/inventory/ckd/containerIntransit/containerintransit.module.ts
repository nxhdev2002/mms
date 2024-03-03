import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ContainerIntransitRoutingModule } from './containerintransit-routing.module';
import { ContainerIntransitComponent } from './containerintransit.component';

@NgModule({
    declarations: [
       ContainerIntransitComponent,
    ],
    imports: [
        AppSharedModule, ContainerIntransitRoutingModule]
})
export class ContainerIntransitModule {}
