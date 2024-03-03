import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { NextEdInComponent } from './nextedin.component';



const routes: Routes = [{
    path: '',
    component: NextEdInComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class NextEdInRoutingModule {}
