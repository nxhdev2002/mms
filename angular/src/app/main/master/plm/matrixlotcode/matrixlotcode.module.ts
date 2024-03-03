import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MatrixLotCodeRoutingModule } from './matrixlotcode-routing.module';
import { MatrixLotCodeComponent } from './matrixlotcode.component';
import { CreateOrEditMatrixLotCodeModalComponent } from './create-or-edit-matrixlotcode-modal.component';

@NgModule({
    declarations: [
       MatrixLotCodeComponent, 
        CreateOrEditMatrixLotCodeModalComponent
      
    ],
    imports: [
        AppSharedModule, MatrixLotCodeRoutingModule]
})
export class MatrixLotCodeModule {}
