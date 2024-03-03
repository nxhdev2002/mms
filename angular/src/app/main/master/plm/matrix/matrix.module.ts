import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MatrixRoutingModule } from './matrix-routing.module';
import { MatrixComponent } from './matrix.component';
import { CreateOrEditMatrixModalComponent } from './create-or-edit-matrix-modal.component';

@NgModule({
    declarations: [
       MatrixComponent, 
        CreateOrEditMatrixModalComponent
    ],
    imports: [
        AppSharedModule, MatrixRoutingModule]
})
export class MatrixModule {}
