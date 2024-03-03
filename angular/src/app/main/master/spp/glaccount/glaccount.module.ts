import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { GLAccountRoutingModule } from './glaccount-routing.module';
import { GLAccountComponent } from './glaccount.component';
// import { CreateOrEditGLAccountModalComponent } from './create-or-edit-glaccount-modal.component';

@NgModule({
    declarations: [
       GLAccountComponent, 
        // CreateOrEditGLAccountModalComponent
      
    ],
    imports: [
        AppSharedModule, GLAccountRoutingModule]
})
export class GLAccountModule {}
