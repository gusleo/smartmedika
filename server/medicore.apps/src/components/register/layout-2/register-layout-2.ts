import { Component, Input } from '@angular/core';

@Component({
    selector: 'register-layout-2',
    templateUrl: 'register.html'
})

export class RegisterLayout2 {

    @Input() data: any;
    @Input() events: any;

    name: string;
    phone: string;

    constructor() { }

    onEvent = (event: string): void => {
        if (this.events[event]) {
            this.events[event]({
                'name': this.name,
                'phone': this.phone
            });
        }
    }
}
