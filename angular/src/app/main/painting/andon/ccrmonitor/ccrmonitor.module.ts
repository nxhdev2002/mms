import { CommonModule } from '@angular/common';
import { NgModule} from '@angular/core';

import {CCRMonitorRoutingModule} from './ccrmonitor-routing.module';
import {CCRMonitorComponent} from './ccrmonitor.component';

//import customer
import { AppCommonModule } from "@app/shared/common/app-common.module";
// import { CreateOrEditBankComponent } from './create-or-edit-bank.component';



@NgModule({
    declarations: [
        CCRMonitorComponent,
        // CreateOrEditBankComponent,
    ],
    imports: [
        CCRMonitorRoutingModule,
        AppCommonModule,
        CommonModule,
    ]
})
export class CCRMonitorModule { }
