import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { EkbPartListGradeRoutingModule } from './ekbpartlistgrade-routing.module';
import { EkbPartListGradeComponent } from './ekbpartlistgrade.component';
import { CreateOrEditEkbPartListGradeModalComponent } from './create-or-edit-ekbpartlistgrade-modal.component';
import { ImportEkbPartListGradeComponent } from './import-ekbpartlistgrade-modal.component';
import { EkbPartListGradeExportComponent } from './ekbpartlistgrade-export-modal.component';

@NgModule({
    declarations: [
       EkbPartListGradeComponent,
        CreateOrEditEkbPartListGradeModalComponent,
        ImportEkbPartListGradeComponent,
        EkbPartListGradeExportComponent

    ],
    imports: [
        AppSharedModule, EkbPartListGradeRoutingModule]
})
export class EkbPartListGradeModule {}
