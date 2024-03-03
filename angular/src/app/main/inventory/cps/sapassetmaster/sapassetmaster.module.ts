import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import { SapAssetMasterComponent } from './sapassetmaster.component';
import { SapAssetMasterRoutingModule } from './sapassetmaster-routing.module';

@NgModule({
    declarations: [
       SapAssetMasterComponent,
    ],
    imports: [
        AppSharedModule, SapAssetMasterRoutingModule]
})
export class SapAssetMasterModule {}
