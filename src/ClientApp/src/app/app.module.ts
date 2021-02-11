import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GraphQLModule } from './graphql.module';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { MarkdownModule, MarkedOptions } from 'ngx-markdown';
import { NgxPaginationModule } from 'ngx-pagination';
import { BlogContentComponent } from './blog-content/blog-content.component';
import { BlogComponent } from './blog/blog.component';
@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    BlogContentComponent,
    BlogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule,
    GraphQLModule,
    HttpClientModule,
    NgxPaginationModule,
    HttpClientModule,
    MarkdownModule.forRoot({
      loader: HttpClient,
      markedOptions: {
        provide: MarkedOptions,
        useValue: {
          gfm: true,
          tables: true,
          breaks: true,
          pedantic: false,
          sanitize: true,
          smartLists: true,
          smartypants: true,
        }
      }
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
