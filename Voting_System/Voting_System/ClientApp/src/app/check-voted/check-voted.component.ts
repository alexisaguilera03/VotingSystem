import { Component, Inject, Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';




@Component({
  selector: 'app-check-voted',
  templateUrl: './check-voted.component.html',
  styleUrls: ['./check-voted.component.css']
})
@Injectable({
  providedIn: 'root'
})
export class CheckVotedComponent implements OnInit {
  Elections: IElection[];
  Voters: IVoter[];
  selection: string = "";
  loaded: boolean = false;
  year: number;
  

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.getElections()
  }
  selectedChangeHandler(event: any) {
    this.selection = event.target.value;
    this.getYear();
  }
  getElections() {
    this.http.get<any>(this.baseUrl + "api/election/getElections").subscribe(result => {
      this.Elections = result;
      for (let i = 0; i < this.Elections.length; i++) {
        this.Elections[i].id = result[i].electionId
      }
    });
  }
  getVoters(id: any) {
    this.http.get<IVoter[]>(this.baseUrl + "api/results/getVoters?electionId=" + this.selection).subscribe(result => {
      this.Voters = result;
      this.loaded = true;
    });
  }
   refresh() {
    this.loaded = false;
   }
  getYear() {
    for (let i = 0; i < this.Elections.length; i++) {
      if (parseInt(this.selection) == this.Elections[i].id) {
        this.year = this.Elections[i].year;
      }
    }
  }

}

interface IElection {
  id: number;
  year: number;
  active: boolean;
}
interface IVoter {
  id: number;
  fname: string;
  lname: string;
  address: string;
  username: string;
  password: string;
}
