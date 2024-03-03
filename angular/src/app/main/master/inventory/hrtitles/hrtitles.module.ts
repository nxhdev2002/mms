import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { HrTitlesComponent } from './hrtitles.component';
import { HrTitlesRoutingModule } from './hrtitles-routing.module';
import { ViewHrTitlesModalComponent } from './view-hrtitles-modal.component';

@NgModule({
    declarations: [
       HrTitlesComponent,
       ViewHrTitlesModalComponent
    ],
    imports: [
        AppSharedModule, HrTitlesRoutingModule]
})
export class HrTitlesModule {}
