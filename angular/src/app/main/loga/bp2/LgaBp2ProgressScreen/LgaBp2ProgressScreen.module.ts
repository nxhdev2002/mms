import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

import { LgaBp2ProgressScreenRoutingModule } from './LgaBp2ProgressScreen-routing.module';
import { LgaBp2ProgressScreenComponent } from './LgaBp2ProgressScreen.component';

//import customer

@NgModule({
    declarations: [
        LgaBp2ProgressScreenComponent,
    ],
    imports: [
        LgaBp2ProgressScreenRoutingModule,
        CommonModule,
    ],
    exports:[
        LgaBp2ProgressScreenComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class LgaBp2ProgressScreenModule { }
