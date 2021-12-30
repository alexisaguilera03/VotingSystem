import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckVotedComponent } from './check-voted.component';

describe('CheckVotedComponent', () => {
  let component: CheckVotedComponent;
  let fixture: ComponentFixture<CheckVotedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckVotedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckVotedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
