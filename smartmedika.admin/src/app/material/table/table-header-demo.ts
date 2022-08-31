import {Component, EventEmitter, Output} from '@angular/core';

@Component({
  moduleId: module.id,
  selector: 'app-table-header-demo',
  templateUrl: 'table-header-demo.html',
  styleUrls: ['table-header-demo.scss'],
})
export class TableHeaderDemoComponent {
  @Output() shiftColumns = new EventEmitter<void>();
  @Output() toggleColorColumn = new EventEmitter<void>();
}
