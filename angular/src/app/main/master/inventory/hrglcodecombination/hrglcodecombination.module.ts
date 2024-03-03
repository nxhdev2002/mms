import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { HrGlCodeCombinationRoutingModule } from './hrglcodecombination-routing.module';
import { HrGlCodeCombinationComponent } from './hrglcodecombination.component';

@NgModule({
    declarations: [
       HrGlCodeCombinationComponent,
    ],
    imports: [
        AppSharedModule, HrGlCodeCombinationRoutingModule]
})
export class HrGlCodeCombinationModule {}
