import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { InventoryStdComponent } from './inventorystd.component';

const routes: Routes = [{
    path: '',
    component: InventoryStdComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InventoryStdRoutingModule {}
