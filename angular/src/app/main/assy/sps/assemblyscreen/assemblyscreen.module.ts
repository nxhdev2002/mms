import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';

//Component
import { AssemblyScreenRoutingModule } from './assemblyscreen-routing.module';
import { AssemblyScreenComponent } from './assemblyscreen.component';


//import customer


@NgModule({
    declarations: [
        AssemblyScreenComponent,
    ],
    imports: [
        AssemblyScreenRoutingModule,
        CommonModule,
    ],
    exports:[
        AssemblyScreenComponent,
    ],
    schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class AssemblyScreenModule { }
