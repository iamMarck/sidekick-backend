import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-validity',
  templateUrl: './validity.component.html'
})
export class ValidityComponent implements OnInit {
  @Input() isValid: boolean;

  constructor() { }

  ngOnInit() {
  }

}
