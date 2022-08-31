import { Component, Input } from '@angular/core';

@Component({
    selector: 'login-layout-3',
    templateUrl: 'login.html'
})

export class LoginLayout3 {
  @Input() data: any;
  @Input() events: any;

  public email: string;
  public password: string;

  constructor() {}

  onEvent = (event: string): void => {
    if (this.events[event]) {
        this.events[event]({
            'email' : this.email,
            'password' : this.password
        });
    }
  }
}
