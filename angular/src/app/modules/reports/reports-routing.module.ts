import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ReservationComponent } from './components/reservation/reservation.component';
import { TopclientsComponent } from './components/topclients/topclients.component';
import { TopdriversComponent } from './components/topdrivers/topdrivers.component';
import { ReportsComponent } from './components/reports/reports.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';


const routes: Routes = [
  { path: "reservation", component: ReservationComponent, canActivate: [AppRouteGuard] },
  { path: "topclients", component: TopclientsComponent, canActivate: [AppRouteGuard] },
  { path: "topdrivers", component: TopdriversComponent, canActivate: [AppRouteGuard] },
  { path: "reports", component: ReportsComponent, canActivate: [AppRouteGuard] },
  { path: "", component: ReportsComponent, canActivate: [AppRouteGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportsRoutingModule { }
