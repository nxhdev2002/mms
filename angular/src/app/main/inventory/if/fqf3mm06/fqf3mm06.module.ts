import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM06RoutingModule } from './fqf3mm06-routing.module';
import { FQF3MM06Component } from './fqf3mm06.component';
import { ViewFqf3mm06ValidateModalComponent } from './view-fqf3mm06-validate-modal.component';
import { ViewFqf3mm06ValidateResultModalComponent } from './view-fqf3mm06-validate-result-modal.component';

@NgModule({
    declarations: [
        FQF3MM06Component,
        ViewFqf3mm06ValidateModalComponent,
        ViewFqf3mm06ValidateResultModalComponent
    ],
    imports: [
        AppSharedModule, FQF3MM06RoutingModule]
})
export class FQF3MM06Module { }
