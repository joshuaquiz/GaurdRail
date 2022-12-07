import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { PanComponent } from './pan/pan.component';
import { ZoomComponent } from './zoom/zoom.component';
import { MapComponent } from './map.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
  declarations: [MenuComponent, PanComponent, ZoomComponent, MapComponent],
  exports: [MenuComponent, PanComponent, ZoomComponent, MapComponent]
})
export class MapModule { }
