import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CpsSuppliersRoutingModule } from './cpssuppliers-routing.module';
import { CpsSuppliersComponent } from './cpssuppliers.component';

@NgModule({
    declarations: [
        CpsSuppliersComponent

    ],
    imports: [
        AppSharedModule, CpsSuppliersRoutingModule]
})
export class CpsSuppliersModule { }
