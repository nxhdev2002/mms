import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { GpsMaterialRegisterByShopComponent } from './gpsmaterialregisterbyshop.component';

const routes: Routes = [{
    path: '',
    component: GpsMaterialRegisterByShopComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class GpsMaterialRegisterByShopRoutingModule {}
