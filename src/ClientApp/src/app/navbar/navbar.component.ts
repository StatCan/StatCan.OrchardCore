import { Component, OnInit } from '@angular/core';
import { SearchQueryGQL, SearchQueryQuery, SearchQueryDocument } from 'src/app/graphql/graphql';
import { Observable, BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  isActive: boolean;
  add!: boolean;
  searchPosts!: Observable<SearchQueryQuery>
  
  unSubscribe!: Subscription;
  constructor(private searchBlog: SearchQueryGQL, private router: Router) { }

  ngOnInit(): void {
  }

  openSearch() {
    this.add = !this.add
  }

  // toggle navbar on mobile view
  toggleNavbar() {
    this.isActive = !this.isActive;
  }
  search(event: string) {
    const searchParameter = `{\"Term\": \"${event}\"}`
    this.searchPosts = this.searchBlog.watch({
      parameters: searchParameter
    }).valueChanges.pipe(map(blogs => blogs.data))
  }
  navigate(name: string) {
    this.router.navigate(['/blog', name], { replaceUrl: true });
    this.openSearch()
  }
  ngOnDestroy() {
    this.unSubscribe.unsubscribe();
  }
}
