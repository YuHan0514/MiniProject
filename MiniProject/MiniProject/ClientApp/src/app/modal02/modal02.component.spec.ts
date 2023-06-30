import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Modal02Component } from './modal02.component';

describe('Modal02Component', () => {
  let component: Modal02Component;
  let fixture: ComponentFixture<Modal02Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [Modal02Component]
    });
    fixture = TestBed.createComponent(Modal02Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
