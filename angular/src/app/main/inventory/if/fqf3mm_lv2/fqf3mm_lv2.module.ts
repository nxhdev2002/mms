import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { FQF3MM_LV2RoutingModule } from './fqf3mm_lv2-routing.module';
import { FQF3MM_LV2Component } from './fqf3mm_lv2.component';
import { ViewDetailLV2ModalComponent } from './view-fqf3mm_lv2-modal.component';

@NgModule({
    declarations: [
       FQF3MM_LV2Component ,
       ViewDetailLV2ModalComponent    
    ],
    imports: [
        AppSharedModule, FQF3MM_LV2RoutingModule]
})
export class FQF3MM_LV2Module {}
