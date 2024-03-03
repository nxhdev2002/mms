import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { PxpsmallpartinputRoutingModule } from './pxpsmallpartinput-routing.module';
import { PxpsmallpartinputComponent } from './pxpsmallpartinput.component';


//import customer
// import { AppCommonModule } from "@app/shared/common/app-common.module";
import { PopuprobbingModalComponent } from './popuprobbing-modal.component';
import { PopupSmallComponent } from './popup-small.component';
import { AppSharedModule } from '@app/shared/app-shared.module';

@NgModule({
    declarations: [
        PxpsmallpartinputComponent,
        PopuprobbingModalComponent,
        PopupSmallComponent,
    ],
    imports: [
        PxpsmallpartinputRoutingModule,
        // AppCommonModule,
        // CommonModule,
        AppSharedModule
    ],
    exports:[
        PxpsmallpartinputComponent,
    ]
})
export class PxpsmallpartinputModule {}
