import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { LotPartComponent } from './lotpart.component';
import { LotPartRoutingModule } from './lotpart-routing.module';

@NgModule({
    declarations: [
       LotPartComponent

    ],
    imports: [
        AppSharedModule, LotPartRoutingModule]
})
export class LotPartModule {}
