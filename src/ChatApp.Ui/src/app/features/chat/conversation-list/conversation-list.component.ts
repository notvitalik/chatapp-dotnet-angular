import { Component } from '@angular/core';

@Component({
  selector: 'app-conversation-list',
  standalone: true,
  templateUrl: './conversation-list.component.html',
  styleUrl: './conversation-list.component.css'
})
export class ConversationListComponent {
  protected readonly conversations = [
    { name: 'General', preview: 'Project kickoff thread' },
    { name: 'Design', preview: 'Wireframes shared' }
  ];
}
