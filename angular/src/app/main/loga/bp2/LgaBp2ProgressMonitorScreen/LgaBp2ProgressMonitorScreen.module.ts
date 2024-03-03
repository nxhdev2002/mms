import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { LgaBp2ProgressMonitorScreenRoutingModule } from './LgaBp2ProgressMonitorScreen-routing.module';
import { LgaBp2ProgressMonitorScreenComponent } from './LgaBp2ProgressMonitorScreen.component';

//import customer

@NgModule({
    declarations: [
        LgaBp2ProgressMonitorScreenComponent,
    ],
    imports: [
        LgaBp2ProgressMonitorScreenRoutingModule,
        CommonModule,
    ],
    exports:[
        LgaBp2ProgressMonitorScreenComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LgaBp2ProgressMonitorScreenModule { }
