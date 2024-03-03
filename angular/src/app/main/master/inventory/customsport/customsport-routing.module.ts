import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CustomsPortComponent } from './customsport.component';

const routes: Routes = [{
    path: '',
    component: CustomsPortComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsPortRoutingModule {}
