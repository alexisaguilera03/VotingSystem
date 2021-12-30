import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-past-ballots',
  templateUrl: './past-ballots.component.html',
  styleUrls: ['./past-ballots.component.css']
})
@Injectable({
  providedIn: 'root'
})
export class PastBallotsComponent implements OnInit {
  voter: IVoter;
  ballot: IBallot;
  Elections: IElection[];
  pastBallot: IBallot;
  selection: string = "";
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.getUser();
    this.getElections();
  }
  selectedChangeHandler(event: any) {
    this.selection = event.target.value;
  }
  getUser() {
    this.http.get<any>(this.baseUrl + "api/ballot/GetBallot?username= " + this.getCookie("username") + "&password=" + this.getCookie("password")).subscribe(result => {
      console.log("");
      this.voter = result;
      this.voter.id = result.voter.voter_Id;
      this.ballot = result;
    });
  }
  getBallot() {
    this.http.get<any>(this.baseUrl + "api/ballot/GeneratePastBallot?voterId=" + this.voter.id + "&electionId=" + this.selection).subscribe(result => {
      this.pastBallot = result;
    });
  }

  getElections() {
    this.http.get<IElection[]>(this.baseUrl + "api/election/getElections").subscribe(result => {
      this.Elections = result;
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

