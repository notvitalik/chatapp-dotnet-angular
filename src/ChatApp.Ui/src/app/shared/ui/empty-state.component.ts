import { Component, input } from '@angular/core';

@Component({
  selector: 'app-empty-state',
  standalone: true,
  template: `
    <section>
      <h3>{{ title() }}</h3>
      <p>{{ description() }}</p>
    </section>
  `
})
export class EmptyStateComponent {
  readonly title = input('Nothing here yet');
  readonly description = input('This area is waiting for data.');
}
