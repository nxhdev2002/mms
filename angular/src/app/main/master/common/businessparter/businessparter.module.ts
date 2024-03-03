import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { BusinessParterRoutingModule } from './businessparter-routing.module';
import { BusinessParterComponent } from './businessparter.component';
import { CreateOrEditBusinessParterModalComponent } from './create-or-edit-businessparter-modal.component';

@NgModule({
    declarations: [
       BusinessParterComponent, 
        CreateOrEditBusinessParterModalComponent    
    ],
    imports: [
        AppSharedModule, BusinessParterRoutingModule]
})
export class BusinessParterModule {}
