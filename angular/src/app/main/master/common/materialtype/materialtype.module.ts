import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { MaterialTypeRoutingModule } from './materialtype-routing.module';
import { MaterialTypeComponent } from './materialtype.component';

@NgModule({
    declarations: [
       MaterialTypeComponent,     
    ],
    imports: [
        AppSharedModule, MaterialTypeRoutingModule]
})
export class MaterialTypeModule {}
