import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css']
})
@Injectable({
  providedIn: 'root'
})
export class ResultsComponent {
  public Result: IResult[];
  public Elections: IElection[];
  public selection: string = "";
  public loaded: boolean = false;

  selectedChangeHandler(event: any) {
    this.selection = event.target.value;
  }

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    this.http.get<IElection[]>(this.baseUrl + "api/election/getElections").subscribe(result => {
      this.Elections = result;
    });
  }
  public displayError() {
    alert("Please choose an election year");
  }

  public refresh() {
    this.loaded = false;
  }
  public getResult(id: any) {
    if (id === "") {
      return this.displayError();
    }
    this.http.get<IResult[]>(this.baseUrl + "api/results/get?id=" + id).subscribe(result => {
      this.Result = result;
      this.loaded = true;
    });
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


interface IResult {
  election: IElection;
  issues:  Array<IIssue>;
  winner: ICandidate;
}
