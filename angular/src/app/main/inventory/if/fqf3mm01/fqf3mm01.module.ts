import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM01RoutingModule } from './fqf3mm01-routing.module';
import { FQF3MM01Component } from './fqf3mm01.component';
import { ViewFqf3mm01ValidateModalComponent } from './view-fqf3mm01-validate-modal.component';
import { ViewFqf3mm01ValidateResultModalComponent } from './view-fqf3mm01-validate-result-modal.component';


@NgModule({
    declarations: [
        FQF3MM01Component,
        ViewFqf3mm01ValidateModalComponent,
        ViewFqf3mm01ValidateResultModalComponent

    ],
    imports: [
        AppSharedModule, FQF3MM01RoutingModule]
})
export class FQF3MM01Module { }
