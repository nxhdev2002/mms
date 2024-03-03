import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { InvHrEmployeeRoutingModule } from './invhremployee-routing.module';
import { InvHrEmployeeComponent } from './invhremployee.component';

@NgModule({
    declarations: [
        InvHrEmployeeComponent,    
    ],
    imports: [
        AppSharedModule, InvHrEmployeeRoutingModule]
})
export class InvHrEmployeeModule {}
