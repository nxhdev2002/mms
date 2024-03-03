import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { SmqdRoutingModule } from './smqdmanagement-routing.module';
import { SmqdComponent } from './smqdmanagement.component';
// import { ImportInvCkdSmqdComponent } from './import-smqdmanagement.component';
//import { ListErrorImportInvCkdSmqdComponent } from './list-error-import-smqdmanagement-modal.component';
import { ImportInvCkdSmqdPxpInOutComponent } from './import-smqd-pxpinout.component';
import { ListErrorImportSmqdModalComponent } from './list-error-import-smqd-modal.component';



@NgModule({
    declarations: [
        SmqdComponent,
       // ImportInvCkdSmqdComponent,
        ImportInvCkdSmqdPxpInOutComponent,
      //  ListErrorImportInvCkdSmqdComponent,
        ListErrorImportSmqdModalComponent

    ],
    imports: [
        AppSharedModule, SmqdRoutingModule]
})
export class SmqdModule { }
