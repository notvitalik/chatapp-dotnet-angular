import { Component } from '@angular/core';

import { ConversationListComponent } from '../conversation-list/conversation-list.component';
import { MessageInputComponent } from '../message-input/message-input.component';
import { MessageListComponent } from '../message-list/message-list.component';

@Component({
  selector: 'app-chat-layout',
  standalone: true,
  imports: [ConversationListComponent, MessageInputComponent, MessageListComponent],
  templateUrl: './chat-layout.component.html',
  styleUrl: './chat-layout.component.css'
})
export class ChatLayoutComponent {
}
