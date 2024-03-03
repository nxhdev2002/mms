import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import { PIOEmailComponent } from './pioemail.component';

const routes: Routes = [{
    path: '',
    component: PIOEmailComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PIOEmailRoutingModule {}
