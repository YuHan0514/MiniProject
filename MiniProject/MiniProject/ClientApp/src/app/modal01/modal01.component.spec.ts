import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Modal01Component } from './modal01.component';

describe('Modal01Component', () => {
  let component: Modal01Component;
  let fixture: ComponentFixture<Modal01Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [Modal01Component]
    });
    fixture = TestBed.createComponent(Modal01Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
