import { Component, OnInit, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
@Injectable({
  providedIn: 'root'
})
export class AdminComponent implements OnInit {
  electionYear: number;
  electionActive: boolean = false;
  activeElectionId: number;
  candidateId: number;
  candidateFname: string;
  candidateLname: string;
  candidateAffiliation: string;
  candidateAssignedElection: number;
  issueId: number;
  issueDescription: string;
  issueTitle: string;
  issueElectionId: number;
  success: boolean = false;
  Elections: IElection[];
  Candidates: ICandidate[];
  selectedElection: string = "";
  selectedCandidate: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  ngOnInit() {
    this.addListeners();
    this.hideDivs();
    this.getElections();
  }

  getElections() {
    this.http.get<IElection[]>(this.baseUrl + "api/election/getElections").subscribe(result => {
      this.Elections = result;
    });
  }

  hideDivs() {
    document.getElementById("candidates").style.display = "none";
    document.getElementById("issues").style.display = "none";
    document.getElementById("elections").style.display = "none";
    document.getElementById("addElection").style.display = "none";
    document.getElementById("editElection").style.display = "none";
    document.getElementById("addCandidate").style.display = "none";
    document.getElementById("editCandidate").style.display = "none";
    document.getElementById("addIssue").style.display = "none";
    document.getElementById("editIssue").style.display = "none";
    return;
  }

  addListeners() {
    document.getElementById("candidate").addEventListener("click", this.openCandidate);
    document.getElementById("issue").addEventListener("click", this.openIssue);
    document.getElementById("election").addEventListener("click", this.openElection);
    document.getElementById("addElectionbtn").addEventListener("click", this.openAddElection);
    document.getElementById("editElectionbtn").addEventListener("click", this.openEditElection);
    document.getElementById("addCandidatebtn").addEventListener("click", this.openAddCandidate);
    document.getElementById("editCandidatebtn").addEventListener("click", this.openEditCandidate);
    document.getElementById("addIssuebtn").addEventListener("click", this.openAddIssue);
    document.getElementById("editIssuebtn").addEventListener("click", this.openEditIssue);
  }
  getChosenElection(event: any) {
    this.selectedElection = event.target.value;
  }
  openCandidate() {
    document.getElementById("candidate").style.background = "#1b6ec2";
    document.getElementById("candidate").style.border = "1px solid #1861ac";

    document.getElementById("issue").style.background = "#1b6ec2";
    document.getElementById("issue").style.border = "1px solid #1861ac";

    document.getElementById("election").style.background = "#1b6ec2";
    document.getElementById("election").style.border = "1px solid #1861ac";

    document.getElementById("candidates").style.display = "none";
    document.getElementById("issues").style.display = "none";
    document.getElementById("elections").style.display = "none";
    document.getElementById("addElection").style.display = "none";
    document.getElementById("editElection").style.display = "none";
    document.getElementById("addCandidate").style.display = "none";
    document.getElementById("editCandidate").style.display = "none";
    document.getElementById("addIssue").style.display = "none";
    document.getElementById("editIssue").style.display = "none";

    document.getElementById("candidate").style.background = "#1861ac";
    document.getElementById("candidate").style.border = "2px solid black";
    setTimeout(() => {
      document.getElementById("candidates").style.display = "block";
    },
      200);
  }
  openIssue() {
    document.getElementById("candidate").style.background = "#1b6ec2";
    document.getElementById("candidate").style.border = "1px solid #1861ac";

    document.getElementById("issue").style.background = "#1b6ec2";
    document.getElementById("issue").style.border = "1px solid #1861ac";

    document.getElementById("election").style.background = "#1b6ec2";
    document.getElementById("election").style.border = "1px solid #1861ac";

    document.getElementById("candidates").style.display = "none";
    document.getElementById("issues").style.display = "none";
    document.getElementById("elections").style.display = "none";
    document.getElementById("addElection").style.display = "none";
    document.getElementById("editElection").style.display = "none";
    document.getElementById("addCandidate").style.display = "none";
    document.getElementById("editCandidate").style.display = "none";
    document.getElementById("addIssue").style.display = "none";
    document.getElementById("editIssue").style.display = "none";

    document.getElementById("issue").style.background = "#1861ac";
    document.getElementById("issue").style.border = "2px solid black";
    setTimeout(() => {
      document.getElementById("issues").style.display = "block";
    },
      200);
  }
  openElection() {
    document.getElementById("candidate").style.background = "#1b6ec2";
    document.getElementById("candidate").style.border = "1px solid #1861ac";

    document.getElementById("issue").style.background = "#1b6ec2";
    document.getElementById("issue").style.border = "1px solid #1861ac";

    document.getElementById("election").style.background = "#1b6ec2";
    document.getElementById("election").style.border = "1px solid #1861ac";

    document.getElementById("candidates").style.display = "none";
    document.getElementById("issues").style.display = "none";
    document.getElementById("elections").style.display = "none";
    document.getElementById("addElection").style.display = "none";
    document.getElementById("editElection").style.display = "none";
    document.getElementById("addCandidate").style.display = "none";
    document.getElementById("editCandidate").style.display = "none";
    document.getElementById("addIssue").style.display = "none";
    document.getElementById("editIssue").style.display = "none";

    document.getElementById("election").style.background = "#1861ac";
    document.getElementById("election").style.border = "2px solid black";
    setTimeout(() => {
      document.getElementById("elections").style.display = "block";
    },
      200);
  }
  openAddElection() {
    document.getElementById("editElection").style.display = "none";
    document.getElementById("addElection").style.display = "block";
  }
  openEditElection() {
    document.getElementById("addElection").style.display = "none";
    document.getElementById("editElection").style.display = "block";
  }
  openAddCandidate() {
    document.getElementById("editCandidate").style.display = "none";
    document.getElementById("addCandidate").style.display = "block";
  }
  openEditCandidate() {
    document.getElementById("addCandidate").style.display = "none";
    document.getElementById("editCandidate").style.display = "block";
  }
  openAddIssue() {
    document.getElementById("editIssue").style.display = "none";
    document.getElementById("addIssue").style.display = "block";
  }
  openEditIssue() {
    document.getElementById("addIssue").style.display = "none";
    document.getElementById("editIssue").style.display = "block";
  }

  getData(item, mode) {
    switch (mode) {
      case "electionYear":
        //todo: check if it is an integer
        this.electionYear = item.target.value;
        break;
      case "candidateFname":
        this.candidateFname = item.target.value;
        break;
      case "candidateLname":
        this.candidateLname = item.target.value;
        break;
      case "candidateAffiliation":
        this.candidateAffiliation = item.target.value;
        break;
      case "candidateAssignedElection":
        //todo: check if it is an integer
        this.candidateAssignedElection = item.target.value;
        break;
      case "issueDescription":
        this.issueDescription = item.target.value;
        break;
      case "issueTitle":
        this.issueTitle = item.target.value;
        break;
    }
  }

  submitCandidate() {
    this.success = false;
    const params = new HttpParams()
      .set('first_name', this.candidateFname)
      .set('last_name', this.candidateLname)
      .set('affiliation', this.candidateAffiliation)
      .set('election_id', String(this.selectedElection));

    return this.http.post<number>(this.baseUrl + "api/Admin/AddCandidate", "", { params }).subscribe(
      result => {
        if (result) {
          this.success = true;
        }
      }
    );
  }

  submitElection() {
    this.success = false;
    var element = <HTMLInputElement>document.getElementById("electionActive");
    this.electionActive = element.checked;

    const params = new HttpParams()
      .set('year', String(this.electionYear));

    return this.http.post<number>(this.baseUrl + "api/Admin/AddElection", "", { params }).subscribe(
      result => {
        if (this.electionActive) {
          this.activeElectionId = result;
          this.changeElection();
        }
        if (result) {
          this.success = true;
          this.getElections();
        }
      });
  }

  editActiveElection() {
    this.activeElectionId = parseInt(this.selectedElection);
    this.changeElection();
  }

  changeElection() {
    this.success = false;
    const params = new HttpParams()
      .set('election_id', String(this.activeElectionId));

    return this.http.put(this.baseUrl + "api/Admin/ChangeElection", "", { params }).subscribe(
      result => {
        if (result) {
          this.success = true;
          this.getElections();
        }
      }
    )
  }

  submitIssue() {
    this.success = false;
    const params = new HttpParams()
      .set("issue_description", this.issueDescription)
      .set("issue_title", this.issueTitle)
      .set("election_id", this.selectedElection);

    return this.http.post(this.baseUrl + "api/Admin/AddIssue", "", { params }).subscribe(
      result => {
        if (result) {
          this.success = true;
        }
      }
    )
  }
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
