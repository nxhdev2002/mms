import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MaterialGroupRoutingModule } from './materialgroup-routing.module';
import { MaterialGroupComponent } from './materialgroup.component';
import { viewMaterialGroupModalComponent } from './view-materialgroup-modal.component';


@NgModule({
    declarations: [
        viewMaterialGroupModalComponent,
       MaterialGroupComponent, 
            
    ],
    imports: [
        AppSharedModule, MaterialGroupRoutingModule]
})
export class MaterialGroupModule {}
