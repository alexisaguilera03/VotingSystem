import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

/* Deletable: Helper Function for welcome {{display user-firstname}}
getFname(){
    return this.user.fname;
}
*/

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

@Injectable({
  providedIn: 'root'
})

export class NavMenuComponent implements OnInit {
  isExpanded = false;
  username: string;
  password: string;
  user: IVoter;
  success: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnInit() {
    this.checkLogin();
    console.log("starting")
  }

  checkLogin() {
    let x = document.cookie;
    if (x === "") {
      return;
    }
    this.username = this.getCookie("username");
    this.password = this.getCookie("password");
    if (this.username !== "" && this.password !== "") {
      this.getLogin();
    }
  }

  getLogin() {
    this.http.get<any>(this.baseUrl + "api/login/Login?username=" + this.username + "&password=" + this.password).subscribe(result => {
      this.user = result;
      this.user.id = result.voter_Id;
      this.user.fname = result.first_Name;
      this.user.lname = result.last_Name;
      this.user.address = result.address;
      this.user.username = result.username;
      this.user.password = result.password;

    });
  }
  getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(";");
    for (let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) == " ") {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }
  logout() {
    document.cookie = "username=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    document.cookie = "password=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    document.cookie = "logout=true; path=/;";
    window.location.replace(this.baseUrl);
  }

}


interface IVoter {
  id: number;
  fname: string;
  lname: string;
  address: string;
  username: string;
  password: string;

}
