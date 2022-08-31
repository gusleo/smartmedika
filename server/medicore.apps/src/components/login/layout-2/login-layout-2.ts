import { Component, Input } from '@angular/core';

@Component({
    selector: 'login-layout-2',
    templateUrl: 'login.html'
})

export class LoginLayout2 {
  @Input() data: any;
  @Input() events: any;

  public phone: string;

  constructor() {}

  onEvent = (event: string): void => {
    if (this.events[event]) {
        this.events[event]({
            'phone' : this.phone
        });
    }
  }
}
