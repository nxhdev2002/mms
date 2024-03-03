import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JiririkokuScreenComponent } from './jiririkokuscreen.component';
import { JiririkokuScreenRoutingModule } from './jiririkokuscreen-routing.module';


@NgModule({
  imports: [
    CommonModule,
    JiririkokuScreenRoutingModule
  ],
  declarations: [JiririkokuScreenComponent]
})
export class JiririkokuScreenModule { }
