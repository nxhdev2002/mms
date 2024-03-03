import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LotDirectSupplyAndonComponent } from './lotdirectsupplyandon.component';
import { LotDirectSupplyAndonRoutingModule } from './lotdirectsupplyandon-routing.module';

@NgModule({
  imports: [
    CommonModule,
    LotDirectSupplyAndonRoutingModule
  ],
  declarations: [LotDirectSupplyAndonComponent]
})
export class LotDirectSupplyAndonModule { }

