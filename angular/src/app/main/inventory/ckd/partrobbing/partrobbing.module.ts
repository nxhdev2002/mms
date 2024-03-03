import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { PartRobbingRoutingModule } from './partrobbing-routing.module';
import { PartRobbingComponent } from './partrobbing.component';
import { ImportPartRobbingComponent } from './import-partrobbing-modal.component';
import { ListErrorImportPartRobbingComponent } from './list-error-import-partrobbing-modal.component';

@NgModule({
    declarations: [
       PartRobbingComponent,
       ImportPartRobbingComponent,
       ListErrorImportPartRobbingComponent

    ],
    imports: [
        AppSharedModule, PartRobbingRoutingModule]
})
export class PartRobbingModule {}
