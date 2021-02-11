import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take} from 'rxjs/operators';
import { BlogPostsQuery, BlogPostsGQL } from '../graphql/graphql';


@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.scss']
})
export class BlogComponent implements OnInit {
  public responsive: boolean = true;
  blogPosts!: Observable<BlogPostsQuery>;
  modalIsActive!: boolean;
  values!: string;
  
  
  public config = {
    itemsPerPage: 1,
    // tslint:disable-next-line: radix
    currentPage: parseInt(sessionStorage.getItem('blogPage') || '') || 0
  };

  constructor(
    private router: Router,
    private allBlogPostGQL: BlogPostsGQL
  ) {

  }

  ngOnInit() {

    this.blogPosts =  this.allBlogPostGQL.watch().valueChanges.pipe(map(blogs =>    blogs.data))

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
