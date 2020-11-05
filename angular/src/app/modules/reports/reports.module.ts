import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '@shared/shared.module';

import { ReportsRoutingModule } from './reports-routing.module';
import { ReservationComponent } from './components/reservation/reservation.component';
import { TopclientsComponent } from './components/topclients/topclients.component';
import { TopdriversComponent } from './components/topdrivers/topdrivers.component';
import { ReportsComponent } from './components/reports/reports.component';


@NgModule({
  declarations: [ReservationComponent,TopclientsComponent, TopdriversComponent, ReportsComponent],
  imports: [
    CommonModule,
    ReportsRoutingModule,
    HttpClientModule,
    SharedModule
  ]
})
export class ReportsModule { }
