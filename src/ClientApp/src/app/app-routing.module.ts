import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlogContentComponent } from './blog-content/blog-content.component';
import { BlogComponent } from './blog/blog.component';


const routes: Routes = [
  {path: '', component: BlogComponent},
  {path: 'blog/:path', component: BlogContentComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
