import { Component } from '@angular/core';

@Component({
  selector: 'app-message-list',
  standalone: true,
  templateUrl: './message-list.component.html',
  styleUrl: './message-list.component.css'
})
export class MessageListComponent {
  protected readonly messages = [
    { sender: 'Ava', content: 'Welcome to the chat slice.' },
    { sender: 'Noah', content: 'Next step is wiring the API.' }
  ];
}
