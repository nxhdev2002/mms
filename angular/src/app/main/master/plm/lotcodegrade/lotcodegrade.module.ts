import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { LotCodeGradeRoutingModule } from './lotcodegrade-routing.module';

import { LotCodeGradeComponent } from './lotcodegrade.component';
import { CreateOrEditLotCodeGradeModalComponent } from './create-or-edit-lotcodegrade-modal.component';

@NgModule({
    declarations: [
       LotCodeGradeComponent,
        CreateOrEditLotCodeGradeModalComponent

    ],
    imports: [
        AppSharedModule, LotCodeGradeRoutingModule]
})
export class LotCodeGradeModule {}
