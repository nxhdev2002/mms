import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM05RoutingModule } from './fqf3mm05-routing.module';
import { FQF3MM05Component } from './fqf3mm05.component';
import { ViewFqf3mm05ValidateModalComponent } from './view-fqf3mm05-validate-modal.component';
import { ViewFqf3mm05ValidateResultModalComponent } from './view-fqf3mm05-validate-result-modal.component';

@NgModule({
    declarations: [
        FQF3MM05Component,
        ViewFqf3mm05ValidateModalComponent,
        ViewFqf3mm05ValidateResultModalComponent
    ],
    imports: [
        AppSharedModule, FQF3MM05RoutingModule]
})
export class FQF3MM05Module { }
