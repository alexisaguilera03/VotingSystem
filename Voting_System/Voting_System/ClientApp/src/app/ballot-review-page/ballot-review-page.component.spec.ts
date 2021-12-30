import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BallotReviewPageComponent } from './ballot-review-page.component';

describe('BallotReviewPageComponent', () => {
  let component: BallotReviewPageComponent;
  let fixture: ComponentFixture<BallotReviewPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BallotReviewPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BallotReviewPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
