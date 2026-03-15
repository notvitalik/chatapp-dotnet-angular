# Real-Time Chat Application — Requirements Document

## 1. Document Control
- **Project Name:** Real-Time Chat Application
- **Working Title:** SignalChat
- **Prepared For:** Portfolio / production-style full-stack build
- **Primary Stack:** Angular + ASP.NET Core + SignalR + SQL Server
- **Version:** 1.0
- **Date:** 2026-03-15

## 2. Purpose
This document defines the business, functional, non-functional, security, and operational requirements for a production-style real-time chat application built with Angular, ASP.NET Core Web API, SignalR, and Microsoft SQL Server.

The goal is to build an end-to-end application that demonstrates:
- secure user authentication and authorization
- real-time messaging over WebSockets via SignalR
- persistent message history in SQL Server
- scalable API and database design
- a responsive Angular user interface
- production-ready engineering practices suitable for GitHub and portfolio use

## 3. Business Goals
The solution should:
1. Allow authenticated users to exchange messages in real time.
2. Support direct messaging and group conversations.
3. Persist chat history for later retrieval.
4. Expose a maintainable architecture suitable for iterative enhancement.
5. Demonstrate professional engineering practices, including logging, validation, error handling, security, and deployment readiness.

## 4. Scope

### 4.1 In Scope
- User registration, login, logout, and token refresh
- JWT-based authentication
- User profile basics
- One-to-one chat
- Group chat / rooms
- Real-time message delivery using SignalR
- Message history persisted in SQL Server
- Read/unread tracking
- Typing indicators
- Presence / online status
- Conversation list and recent activity
- Search conversations and messages (basic)
- Administrative room creation (optional MVP+)
- Dockerized local development setup
- API documentation with Swagger/OpenAPI
- Basic monitoring/logging hooks

### 4.2 Out of Scope for Initial 2-Day Build
- End-to-end encryption
- Voice/video calling
- Rich file preview pipeline
- AI moderation
- Multi-tenant enterprise partitioning
- Mobile application
- Kubernetes production deployment
- Offline-first sync
- Push notifications to native devices

## 5. User Roles

### 5.1 Standard User
Can:
- register and log in
- create or join allowed conversations
- send and receive messages
- view message history
- view presence, typing, and read status where permitted

### 5.2 Administrator
Can:
- manage users at a basic level
- create group rooms
- assign members to certain rooms
- deactivate abusive accounts or rooms

## 6. Assumptions
- Application will initially target web users.
- SQL Server will be the system of record.
- SignalR will be used instead of raw WebSocket implementation.
- Authentication will use ASP.NET Core Identity or a custom JWT flow.
- The first release prioritizes correctness and structure over advanced UX polish.

## 7. Functional Requirements

## 7.1 Authentication and Identity
**FR-001** Users shall be able to register with username, email, and password.

**FR-002** Users shall be able to log in using email/username and password.

**FR-003** The system shall issue access tokens and refresh tokens.

**FR-004** The system shall support logout and refresh-token revocation.

**FR-005** Passwords shall be stored using strong one-way hashing.

**FR-006** The system shall support role-based authorization for user and admin roles.

## 7.2 User Profile
**FR-010** Users shall be able to view and update limited profile data such as display name and avatar URL.

**FR-011** The system shall track `LastSeenAt` for presence and audit needs.

## 7.3 Conversations
**FR-020** The system shall support one-to-one conversations.

**FR-021** The system shall support group conversations.

**FR-022** Users shall be able to list all conversations they belong to.

**FR-023** Users shall be able to create a new direct or group conversation subject to permissions.

**FR-024** Users shall not be able to view conversations they are not authorized to access.

**FR-025** Conversation list shall display latest message preview and last activity time.

## 7.4 Messaging
**FR-030** Users shall be able to send messages in an authorized conversation.

**FR-031** Sent messages shall be persisted in SQL Server.

**FR-032** Messages shall be delivered in real time to active connected participants.

**FR-033** Users shall be able to retrieve older messages using paginated history queries.

**FR-034** Users shall be able to edit their own messages within configurable rules.

**FR-035** Users shall be able to soft-delete their own messages within configurable rules.

**FR-036** The system shall store timestamps for sent and edited messages.

**FR-037** The system shall support basic text content and extensibility for future attachments.

## 7.5 Presence, Typing, and Read State
**FR-040** The system shall show online/offline presence for users when possible.

**FR-041** The system shall broadcast typing indicators to current conversation participants.

**FR-042** The system shall track when a message is read by a participant.

**FR-043** The UI shall show unread counts per conversation.

## 7.6 Search
**FR-050** Users shall be able to search their own conversations by title or participant name.

**FR-051** Users shall be able to search message history within authorized conversations.

## 7.7 Notifications
**FR-060** The UI shall update conversation ordering based on the latest message activity.

**FR-061** The system shall notify participants of new messages and room events in real time.

## 7.8 Admin
**FR-070** Admins shall be able to create moderated chat rooms.

**FR-071** Admins shall be able to add or remove participants from rooms they manage.

## 8. Non-Functional Requirements

## 8.1 Performance
**NFR-001** Real-time message propagation for connected users should generally occur within 1 second under normal development-scale load.

**NFR-002** Standard message history endpoints should return the first page of results within 2 seconds under expected local/demo load.

**NFR-003** The application shall support pagination for message history to avoid excessive payload sizes.

## 8.2 Scalability
**NFR-010** The backend shall be designed to support horizontal scaling of API nodes.

**NFR-011** The real-time layer shall allow future adoption of a SignalR backplane or Azure SignalR Service.

## 8.3 Reliability
**NFR-020** Message persistence shall occur before delivery acknowledgment is considered complete.

**NFR-021** API and hub operations shall include structured logging and error handling.

## 8.4 Security
**NFR-030** All authenticated API and hub access shall require valid tokens.

**NFR-031** Authorization checks shall be enforced server-side for every conversation operation.

**NFR-032** Sensitive secrets shall not be hardcoded in source control.

**NFR-033** Input validation and output encoding shall be used to reduce injection and XSS risks.

**NFR-034** HTTPS shall be assumed for non-local deployment.

## 8.5 Maintainability
**NFR-040** The solution shall follow clean separation of concerns.

**NFR-041** The solution shall be organized into layers/projects that are testable and replaceable.

**NFR-042** Public APIs and contracts shall be documented.

## 8.6 Observability
**NFR-050** Application shall log authentication failures, message send failures, and major system exceptions.

**NFR-051** Correlation IDs should be supported for request tracing.

## 8.7 Usability
**NFR-060** The Angular UI shall be responsive for desktop and basic tablet usage.

**NFR-061** The application shall provide meaningful validation and error messages.

## 9. Security Requirements
- Password hashing: PBKDF2 / Identity default or stronger approved algorithm
- JWT access tokens with short-lived expiry
- Refresh token persistence and revocation
- Role-based authorization
- Input validation for all DTOs
- Server-side conversation membership checks
- Rate limiting or throttling on login and message APIs where practical
- CORS policy restricted by environment
- Secure secret storage via environment variables / user secrets / Key Vault later
- Audit fields for key entities

## 10. Data Requirements
Core entities:
- Users
- Roles
- Conversations
- ConversationParticipants
- Messages
- MessageReadStates
- RefreshTokens
- UserConnections / presence cache abstraction

Key data constraints:
- A direct conversation between the same two active users should not duplicate unintentionally.
- Messages belong to exactly one conversation.
- Read-state records are unique per message/user pair.
- Soft-deleted records preserve auditability where required.

## 11. Integration Requirements
- Angular web app consumes REST APIs and SignalR hub
- API communicates with SQL Server
- Swagger/OpenAPI exposes endpoint documentation
- Optional future integration with Azure SignalR, Azure Key Vault, blob storage, or email service

## 12. Reporting / Audit Requirements
The system should allow auditability for:
- user registration
- login activity
- conversation creation
- message create/edit/delete events
- admin membership changes

## 13. Acceptance Criteria for MVP
The MVP is accepted when:
1. A user can register and log in.
2. Two users can start a direct conversation.
3. Users can exchange real-time messages.
4. Messages persist and reload from SQL Server.
5. Unread counts and read state work at a basic level.
6. A group conversation can be created and used.
7. Unauthorized users cannot access protected conversations.
8. The solution runs locally with documented setup steps.
9. Swagger documents the REST endpoints.
10. Angular UI demonstrates core workflows cleanly.

## 14. Risks
- Real-time state management can become complex quickly.
- Auth, SignalR, and Angular integration may consume more time than expected.
- Read-state and presence features can create race conditions if not simplified.
- Overengineering may slow down the 2-day build.

## 15. Recommended MVP Prioritization
### Must Have
- registration/login
- JWT auth
- conversation list
- direct chat
- group chat
- real-time messaging
- message persistence
- basic unread count

### Should Have
- typing indicators
- presence
- search
- Docker compose
- Swagger polish

### Could Have
- edit/delete message
- admin room management
- seed/demo data
- screenshots and architecture diagram
