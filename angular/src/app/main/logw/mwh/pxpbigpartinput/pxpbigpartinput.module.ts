import { CommonModule } from '@angular/common';
import {NgModule} from '@angular/core';

import { PxpbigpartinputRoutingModule } from './pxpbigpartinput-routing.module';
import { PxpbigpartinputComponent } from './pxpbigpartinput.component';


//import customer
// import { AppCommonModule } from "@app/shared/common/app-common.module";
import { PopupBigComponent } from './popup-big.component';
import { AppSharedModule } from '@app/shared/app-shared.module';


@NgModule({
    declarations: [
        PxpbigpartinputComponent,
        PopupBigComponent
    ],
    imports: [
        PxpbigpartinputRoutingModule,
        // AppCommonModule,
        // CommonModule,
        AppSharedModule,
    ],
    exports:[
        PxpbigpartinputComponent,
    ]
})
export class PxpbigpartinputModule {}
