import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { CpsInventoryItemsComponent } from './cpsinventoryitems.component';


const routes: Routes = [{
    path: '',
    component: CpsInventoryItemsComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CpsInventoryItemsRoutingModule {}
