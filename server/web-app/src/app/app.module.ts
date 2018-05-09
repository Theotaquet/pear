import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { SessionsListComponent } from './sessions-list/sessions-list.component';
import { SessionComponent } from './session/session.component';
import { Error404Component } from './error404/error404.component';

import { SessionService } from './session.service';
import { FormatMetricsManagerNamePipe } from './format-metrics-manager-name.pipe';
import { FormatChartNamePipe } from './format-chart-name.pipe';
import { FormatStatisticNamePipe } from './format-statistic-name.pipe';

const appRoutes: Routes = [
  { path: '', redirectTo: 'sessions', pathMatch: 'full' },
  { path: 'sessions', component: SessionsListComponent },
  { path: 'sessions/:sessionId', component: SessionComponent },
  { path: '**', component: Error404Component }
]

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SessionsListComponent,
    SessionComponent,
    Error404Component,
    FormatMetricsManagerNamePipe,
    FormatChartNamePipe,
    FormatStatisticNamePipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [
    SessionService,
    FormatMetricsManagerNamePipe,
    FormatChartNamePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
