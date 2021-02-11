import { Component, OnInit, inject } from '@angular/core';
import { Observable, from } from 'rxjs';
import { BlogPostsQuery, BlogPostsGQL } from './graphql/graphql';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  blogPosts!: Observable<BlogPostsQuery>;
  public responsive: boolean = true;
  public config = {
    itemsPerPage: 1,
    // tslint:disable-next-line: radix
    currentPage: parseInt(sessionStorage.getItem('blogPage') || '') || 0
  };




  constructor(
    private router: Router,
    private allBlogPostGQL: BlogPostsGQL
  ) { }

  ngOnInit() {

    this.blogPosts = this.allBlogPostGQL.watch().valueChanges.pipe(map(blogs => blogs.data))

  }

  onPageChange(number: number) {
    sessionStorage.setItem('blogPage', JSON.stringify(number));
    this.config.currentPage = number;
    console.log('eeeee', number);

  }

  showMore(name: string) {
    this.router.navigate(['/blog', name]);
    console.log('aaaaaaaa', this.config.currentPage)
  }
}
