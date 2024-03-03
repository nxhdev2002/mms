import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM02RoutingModule } from './fqf3mm02-routing.module';
import { FQF3MM02Component } from './fqf3mm02.component';
import { ViewFqf3mm02ValidateModalComponent } from './view-fqf3mm02-validate-modal.component';
import { ViewFqf3mm02ValidateResultModalComponent } from './view-fqf3mm02-validate-result-modal.component';


@NgModule({
    declarations: [
        FQF3MM02Component,
        ViewFqf3mm02ValidateModalComponent,
        ViewFqf3mm02ValidateResultModalComponent

    ],
    imports: [
        AppSharedModule, FQF3MM02RoutingModule]
})
export class FQF3MM02Module { }
