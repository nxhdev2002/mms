import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';



//import customer
import { AppCommonModule } from "@app/shared/common/app-common.module";
import { BigPartTablet2RoutingModule } from './bigparttablet2-routing.module';
import { BigPartTablet2Component } from './bigparttablet2.component';

@NgModule({
    declarations: [
        BigPartTablet2Component
    ],
    imports: [
        CommonModule,
        AppCommonModule,
        BigPartTablet2RoutingModule
    ],
    exports:[
        BigPartTablet2Component,
    ],
})
export class BigPartTablet2Module {


}
