import { Component, Inject, Injectable, OnInit} from '@angular/core';
import { HttpClient } from '@angular/common/http';



@Component({
  selector: 'app-home-login',
  templateUrl: './home-login.component.html',
  styleUrls: ['./home-login.component.css']
})

@Injectable({
  providedIn: 'root'
})

export class HomeComponent implements OnInit {
  Voter: IVoter;
  public notValidUser: boolean = true;
  public notValidPass: boolean = true;
  public stopExecution: boolean = false;
  public username: string;
  public password: string;
  public loaded: boolean = false;
  public success: boolean = false;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }
  ngOnInit() {
    this.checkUser();
  }



  public login() {
    this.http.get<any>(this.baseUrl + "api/login/Login?username=" + this.username + "&password=" + this.password).subscribe(result => {
      this.Voter = result; 
      this.Voter.id = result.voter_Id;
      this.Voter.fname = result.first_Name;
      this.Voter.lname = result.last_Name;
      this.Voter.address = result.address;
      this.Voter.username = result.username;
      this.Voter.password = result.password;
        if (this.Voter.username === "missing") {
          return this.displayError("Username cannot be empty");
        }
        else if (this.Voter.password === "missing") {
          return this.displayError("Password cannot be empty");
        }
        else if (this.Voter.password === "incorrect") {
          return this.displayError("Username or password was not correct");
        }
        else if (this.Voter.username === "null" || this.Voter.password === "null") { //this statement should only be true if the function is called without any parameters or if the user deletes both the username and password field
          return this.displayError("an unexpected error occurred. Please try again"); 
      }
        if (this.Voter.id != -1) {
          document.cookie = "username=" + this.Voter.username + "; path=/";
          document.cookie = "password=" + this.Voter.password + "; path=/";
          return this.getPage();
        }
      });
    return;
  }

  checkUser() {
    if (this.getCookie("username") != "") {
      return this.getPage();
    } else {
      this.loaded = true;
      this.checkLogout();
    }
  }
  checkLogout() {
    if (this.getCookie("logout") != "") {
      this.success = true;
      document.cookie = "logout=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
      setTimeout(() => { //this line waits 5 seconds and then executes code below
          this.success = false;
        },
        5000);
    }
  }

  getPage() {
    let username = this.getCookie("username");
    if (username == "admin") {
      return window.location.replace(this.baseUrl + "admin");
    }
    else if (username == "company") {
      return window.location.replace(this.baseUrl + "check-voted");
    } else {
      return window.location.replace(this.baseUrl + "ballot");
    }
  }


  getUsername(item) {
    this.username = item.target.value;
  }
  getPassword(item) {
    this.password = item.target.value;
  }
  refresh() {
    location.reload();
  }

  public displayError(error: string) {
    alert(error);
    return;
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
}

interface IVoter {
  id: number;
  fname: string;
  lname: string;
  address: string;
  username: string;
  password: string;

}
