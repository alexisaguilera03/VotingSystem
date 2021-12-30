import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-ballot',
  templateUrl: './ballot.component.html',
  styleUrls: ['./ballot.component.css']
})
@Injectable({
  providedIn: 'root'
})
export class BallotComponent implements OnInit {
  public ballot: IBallot;
  public selection: string = "";
  public loaded: boolean = false;
  public voter: IVoter;
  public voted: boolean = false;
  public loggedIn: boolean = true;

  selectedChangeHandler(event: any) {
    this.selection = event.target.value;
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.getBallot();
  }
  getBallot() {
    this.checkLogin();
    if (!this.loggedIn) {
      return;
    }
    this.http.get<any>(this.baseUrl + "api/ballot/GetBallot?username= " + this.getCookie("username") + "&password=" + this.getCookie("password")).subscribe(result => {
      this.ballot = result;
      this.ballot.status = result.voter_Status;
      this.ballot.candidateResults = result.candidate_Results;
      this.ballot.issueResults = result.issues_Results;
      for (let i = 0; i < this.ballot.candidates.length; i++) {
        this.ballot.candidates[i].id = result.candidates[i].canidate_Id; //this misspelling drives me crazy 
      }
      for (let i = 0; i < this.ballot.issues.length; i++) {
        this.ballot.issues[i].id = result.issues[i].issue_Id;
      }
      this.checkIfVoted();
    });
    
  }
  displayError(message: string) {
    alert(message);
  }
  checkLogin() {
    if (this.getCookie("username") == "" || this.getCookie("password") == "") {
      this.displayError("User is not logged in");
      this.loggedIn = false;
      return window.location.replace(this.baseUrl);
    }else if (this.getCookie("username") == "admin" || this.getCookie("username") == "company") {
      this.loggedIn = false;
      this.loaded = false;
      return window.location.replace(this.baseUrl);
    }
  }

  checkIfVoted() {
    if (this.ballot.status) {
      this.loaded = false;
      return window.location.replace(this.baseUrl + "past-ballots");
    } else {
      this.loaded = true;
    }
  }

 recordResponse() {
   this.getResponses();
   let data = JSON.stringify(this.ballot);
    this.http.get(this.baseUrl + "api/ballot/StoreResults?jsonBallot=" + data).subscribe(result => {
      if (result) {
        this.http.get(this.baseUrl + "api/ballot/StoreResultsJSON?jsonBallot=" + data).subscribe(json => {
          document.cookie = "ballot=" + JSON.stringify(json)+ "; path=/";
          return window.location.replace(this.baseUrl + "ballot-review");
        });
        
      }
    });

 }

  getResponses() {
    for (let i = 0; i < this.ballot.candidates.length; i++) {
      var value = document.getElementById("elec_" + this.ballot.candidates[i].id) as HTMLInputElement;
      if (value.checked) {
        this.ballot.candidateResults[i] = this.ballot.candidates[i].id;
      } else {
        this.ballot.candidateResults[i] = -1;
      }
    }
    for (let i = 0; i < this.ballot.issues.length; i++) {
      let value = document.getElementById("issue_" + this.ballot.issues[i].id) as HTMLInputElement;
      if (value.checked) {
        this.ballot.issueResults[i] = this.ballot.issues[i].id;
      } else {
        this.ballot.issueResults[i] = -1;
      }
    }
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


interface IBallot {
  voter: IVoter;
  election: IElection;
  issues: Array<IIssue>;
  candidates: Array<ICandidate>;
  status: boolean;
  issueResults: Array<number>;
  candidateResults: Array<number>;
}

interface IVoter {
  id: number;
  fname: string;
  lname: string;
  address: string;
  username: string;
  password: string;

}

interface IIssue {
  description: string;
  election_id: number;
  id: number;
  title: string;
  passed: boolean;
  votes: number;
}
interface IElection {
  id: number;
  year: number;
  active: boolean;
}
interface ICandidate {
  id: number;
  fname: string;
  lname: string;
  affiliation: string;
  election_id: number;
  votes: number;
}

