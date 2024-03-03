import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { TaxRoutingModule } from './tax-routing.module';
import { TaxComponent } from './tax.component';

@NgModule({
    declarations: [
       TaxComponent
    ],
    imports: [
        AppSharedModule, TaxRoutingModule]
})
export class TaxModule {}
