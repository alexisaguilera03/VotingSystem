import { Component, OnInit, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-ballot-review-page',
  templateUrl: './ballot-review-page.component.html',
  styleUrls: ['./ballot-review-page.component.css']
})

@Injectable({
  providedIn: 'root'
})

export class BallotReviewPageComponent implements OnInit {
  public ballot: IBallot;
  public voter: IVoter;
  public submitted: boolean = false;
  public electionId: number;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }
  ngOnInit() {
    this.getBallot();
    this.getVoter();

  }
  getVoter() {
    this.http.get<any>(this.baseUrl + "api/login/login?username=" + this.getCookie("username") + "&password=" + this.getCookie("password")).subscribe(result => {
      this.voter = result;
      this.voter.id = result.voter_Id;
    });
  }

  getBallot() {
    console.log("nothing");
    console.log("nothing again");
    var data = JSON.parse(this.getCookie("ballot"));
    this.ballot = JSON.parse(this.getCookie("ballot")); //this causes all of the data to not match up to any of the interfaces. DO NOT MANIPULATE THIS DATA YOU WILL HAVE A BAD TIME
    this.electionId = data.Current_Election.electionId;
  }
  submit() {
    this.http.get<boolean>(this.baseUrl + "api/ballot/StoreVoted?voterId=" + this.voter.id + "&electionId=" + this.electionId).subscribe(result =>
    {
      if (result) {
        this.submitted = true;
        console.log("success");
        return window.location.replace(this.baseUrl + "past-ballots");
      }
    });
  }

  edit() {
    this.http.get<boolean>(this.baseUrl + "api/ballot/RemoveBallot?id=" + this.voter.id).subscribe(result => {
      return window.location.replace(this.baseUrl + "ballot");
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


interface IBallot {
  voter: IVoter;
  election: IElection;
  issues: Array<IIssue>;
  candidates: Array<ICandidate>;
  status: boolean;
  issueResults: Array<number>;
  candidateResults: Array<number>;
}
