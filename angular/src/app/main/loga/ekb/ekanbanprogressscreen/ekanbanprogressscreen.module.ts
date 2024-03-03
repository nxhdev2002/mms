import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkanbanProgressScreenRoutingModule } from './ekanbanprogressscreen-routing.module';
import { EkanbanProgressScreenComponent } from './ekanbanprogressscreen.component';

@NgModule({
    declarations: [
        EkanbanProgressScreenComponent,
    ],
    imports: [
        EkanbanProgressScreenRoutingModule,AppSharedModule]
})
export class  EkanbanProgressScreenModule {}



