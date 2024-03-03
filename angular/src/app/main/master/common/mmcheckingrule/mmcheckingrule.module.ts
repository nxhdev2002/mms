import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MMCheckingRuleRoutingModule } from './mmcheckingrule-routing.module';
import { MMCheckingRuleComponent } from './mmcheckingrule.component';
import { ListErrorImportMMCheckingRuleComponent } from './list-error-import-mmcheckingrule-modal.component';
import { ImportMMCheckingRuleComponent } from './import_mmcheckingrule.component';
import { ViewHistorymmcheckingruleModalComponent } from './history-mmcheckingrule-modal.component';

@NgModule({
    declarations: [
       MMCheckingRuleComponent, 
        ImportMMCheckingRuleComponent,
        ListErrorImportMMCheckingRuleComponent,
        ViewHistorymmcheckingruleModalComponent
    ],
    imports: [
        AppSharedModule, MMCheckingRuleRoutingModule]
})
export class MMCheckingRuleModule {}
