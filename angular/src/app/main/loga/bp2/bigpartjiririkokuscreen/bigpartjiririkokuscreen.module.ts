import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BigPartJiririkokuScreenComponent } from './bigpartjiririkokuscreen.component';
import { BigPartJiririkokuScreenRoutingModule } from './bigpartjiririkokuscreen-routing.module';

@NgModule({
  imports: [
    CommonModule,
    BigPartJiririkokuScreenRoutingModule
  ],
  declarations: [BigPartJiririkokuScreenComponent]
})
export class BigPartJiririkokuScreenModule { }

