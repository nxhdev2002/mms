import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomsPortRoutingModule } from './customsport-routing.module';
import { CustomsPortComponent } from './customsport.component';

@NgModule({
    declarations: [
        CustomsPortComponent


    ],
    imports: [
        AppSharedModule, CustomsPortRoutingModule]
})
export class CustomsPortModule { }
