import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SmqdOrderLeadTimeRoutingModule } from './smqdorderleadtime-routing.module';
import { SmqdOrderLeadTimeComponent } from './smqdorderleadtime.component';
import { ImportSmqdOrderLeadTimeComponent } from './import-smqdorderleadtime-modal.component';
import { ListErrorImportSmqdOrderLeadTimeComponent } from './list-error-import-smqdorderleadtime-modal.component';
import { ViewOrderLeadTimeModalComponent } from './view-modal-smqdorderleadtime';


@NgModule({
    declarations: [
       SmqdOrderLeadTimeComponent,
       ImportSmqdOrderLeadTimeComponent,
       ListErrorImportSmqdOrderLeadTimeComponent,
       ViewOrderLeadTimeModalComponent
    ],
    imports: [
        AppSharedModule, SmqdOrderLeadTimeRoutingModule]
})
export class SmqdOrderLeadTimeModule {}
