import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-page2',
  templateUrl: './page2.component.html',
  styleUrls: ['./page2.component.css']
})
export class Page2Component implements OnInit {

  serverList: Array<string>;

  constructor(private http: HttpClient) {
    const csrfToken = (<HTMLInputElement>document.getElementsByName('__RequestVerificationToken')[0]).value

    let headers = new HttpHeaders();
    headers.set('X-XSRF-Token', csrfToken);

    http.get(`/api/angular`, {
      headers: { 'X-XSRF-Token': csrfToken }
    })
      .subscribe(
        (response: Array<string>) => {
          this.serverList = response;
        },
        (err) => { console.log('There is an error:', err) });
  }

  ngOnInit() {
  }

}
