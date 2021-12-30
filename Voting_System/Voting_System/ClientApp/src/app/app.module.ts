import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home-login/home-login.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { ResultsComponent } from './results/results.component';
import { BallotReviewPageComponent } from './ballot-review-page/ballot-review-page.component';
import { BallotComponent } from './ballot/ballot.component';
import { PastBallotsComponent } from './past-ballots/past-ballots.component';
import { AdminComponent } from './admin/admin.component';
import { CheckVotedComponent } from './check-voted/check-voted.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ResultsComponent,
    BallotReviewPageComponent,
    BallotComponent,
    PastBallotsComponent,
    AdminComponent,
    CheckVotedComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'results', component: ResultsComponent},
      { path: 'ballot-review', component: BallotReviewPageComponent},
      { path: 'results', component: ResultsComponent },
      { path: 'ballot', component: BallotComponent },
      { path: 'past-ballots', component: PastBallotsComponent },
      { path: 'admin', component: AdminComponent},
      { path: 'check-voted', component: CheckVotedComponent},
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
