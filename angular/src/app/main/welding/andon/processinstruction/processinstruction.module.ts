import { CommonModule } from '@angular/common';
import { NgModule} from '@angular/core';

import {ProcessInstructionRoutingModule} from './processinstruction-routing.module';
import {ProcessInstructionComponent} from './processinstruction.component';

//import customer
import { AppCommonModule } from "@app/shared/common/app-common.module";
// import { CreateOrEditBankComponent } from './create-or-edit-bank.component';



@NgModule({
    declarations: [
        ProcessInstructionComponent,
    ],
    imports: [
        ProcessInstructionRoutingModule,
        AppCommonModule,
        CommonModule,
    ]
})
export class ProcessInstructionModule { }
