import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FQF3MM03RoutingModule } from './fqf3mm03-routing.module';
import { FQF3MM03Component } from './fqf3mm03.component';
import { ViewBusinessDataComponent } from './view-business-data-modal.component';
import { ViewFqf3mm03ValidateModalComponent } from './view-fqf3mm03-validate-modal.component';
import { ViewFqf3mm03ValidateResultModalComponent } from './view-fqf3mm03-validate-result-modal.component';


@NgModule({
    declarations: [
        FQF3MM03Component,
        ViewBusinessDataComponent,
        ViewFqf3mm03ValidateModalComponent,
        ViewFqf3mm03ValidateResultModalComponent
    ],
    imports: [
        AppSharedModule, FQF3MM03RoutingModule]
})
export class FQF3MM03Module { }
