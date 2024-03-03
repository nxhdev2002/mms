import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { FQF3MM04RoutingModule } from './fqf3mm04-routing.module';
import { FQF3MM04Component } from './fqf3mm04.component';
import { ViewFqf3mm04ValidateModalComponent } from './view-fqf3mm04-validate-modal.component';
import { ViewFqf3mm04ValidateResultModalComponent } from './view-fqf3mm04-validate-result-modal.component';


@NgModule({
    declarations: [
       FQF3MM04Component,
       ViewFqf3mm04ValidateModalComponent,
       ViewFqf3mm04ValidateResultModalComponent
      
    ],
    imports: [
        AppSharedModule, FQF3MM04RoutingModule]
})
export class FQF3MM04Module {}
