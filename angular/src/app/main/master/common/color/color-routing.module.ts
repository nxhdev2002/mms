import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { ColorComponent } from './color.component';
import { CreateOrEditColorModalComponent } from './create-or-edit-color-modal.component';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [{
    path: '',
    component: ColorComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ColorRoutingModule {}
