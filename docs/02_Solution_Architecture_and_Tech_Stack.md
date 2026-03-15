# Real-Time Chat Application — Solution Architecture and Tech Stack

## 1. Overview
This document describes the target solution architecture for a production-style real-time chat system built on Microsoft-centric technologies with an Angular front end. The architecture is optimized for:
- clean separation of responsibilities
- real-time communication
- secure API design
- SQL Server persistence
- future extensibility to cloud deployment

## 2. Architectural Style
Recommended style:
- **Frontend:** SPA using Angular
- **Backend:** Modular monolith with clean/layered architecture
- **Real-Time:** SignalR hub over WebSockets
- **Persistence:** SQL Server
- **Auth:** JWT + refresh tokens
- **Documentation:** Swagger/OpenAPI + Markdown docs
- **Deployment target:** Docker local; Azure/IIS/container ready later

This project should begin as a **modular monolith**, not microservices. That keeps the build realistic for 2 days while still showing enterprise design discipline.

## 3. Logical Architecture

```text
[ Angular SPA ]
   | REST + JWT
   | SignalR WebSocket connection
   v
[ ASP.NET Core API + SignalR Hub ]
   | Application Services
   | Auth / Validation / Authorization / Logging
   v
[ Infrastructure Layer ]
   | EF Core / Dapper
   v
[ SQL Server Database ]
```

## 4. Main Components

## 4.1 Angular SPA
Responsibilities:
- user authentication flows
- conversation list and filtering
- chat window and message rendering
- unread badges, typing indicator, presence status
- SignalR client connection management
- route guards and token handling

Suggested Angular modules/components:
- `auth`
- `layout`
- `chat`
- `shared`
- `core`

## 4.2 ASP.NET Core API
Responsibilities:
- REST endpoints for auth, users, conversations, and messages
- token issuance and refresh handling
- request validation
- authorization enforcement
- orchestration of business rules
- OpenAPI exposure

Suggested controllers/endpoints:
- `/api/auth`
- `/api/users`
- `/api/conversations`
- `/api/messages`
- `/api/search`

## 4.3 SignalR Hub
Responsibilities:
- connection registration
- user-to-connection mapping
- joining conversation groups
- broadcasting new messages
- broadcasting typing indicators
- presence notifications
- read receipt events

Recommended hub:
- `ChatHub`

## 4.4 SQL Server
Responsibilities:
- store users and auth-related entities
- store conversations and membership
- store messages and read states
- store refresh tokens
- support indexed lookups for recent chats and paginated message history

## 5. Recommended Tech Stack

### Frontend
- **Angular 18+**
- TypeScript
- RxJS
- Angular Router
- Angular HttpClient
- Angular SignalR client (`@microsoft/signalr`)
- Angular Material or Tailwind/CSS for UI polish

### Backend
- **.NET 8 ASP.NET Core Web API**
- SignalR
- FluentValidation or built-in validation
- MediatR optional, but not necessary for 2-day build
- Serilog for structured logging
- AutoMapper optional, but can be skipped if DTO mapping is simple

### Data Access
Preferred choices:
- **EF Core** for core CRUD and migrations
- optional **Dapper** for optimized read queries if desired

For a 2-day build, pure EF Core is enough unless you want to showcase mixed access patterns.

### Database
- **Microsoft SQL Server 2022 Express/Developer**
- SQL scripts for seed/demo data
- indexed tables for conversations and messages

### Auth / Security
- ASP.NET Core Identity or custom auth with BCrypt/PBKDF2
- JWT access tokens
- refresh tokens stored in database
- role-based authorization

### DevOps / Tooling
- Docker + Docker Compose
- Swagger / Swashbuckle
- xUnit for minimal tests
- Postman or Bruno collection optional
- ESLint / Prettier optional for Angular
- GitHub Actions optional follow-up

## 6. Recommended Project Structure

```text
src/
  server/
    SignalChat.Api/
    SignalChat.Application/
    SignalChat.Domain/
    SignalChat.Infrastructure/
    SignalChat.Contracts/
    SignalChat.Tests/
  client/
    signal-chat-ui/
```

## 7. Layer Responsibilities

## 7.1 Domain Layer
Contains:
- entities
- enums
- value objects where useful
- domain rules independent of framework

Examples:
- `User`
- `Conversation`
- `ConversationParticipant`
- `Message`
- `RefreshToken`

## 7.2 Application Layer
Contains:
- use cases / services
- DTOs
- interfaces
- business rules
- validators

Examples:
- `AuthService`
- `ConversationService`
- `MessageService`
- `PresenceService`

## 7.3 Infrastructure Layer
Contains:
- EF Core DbContext
- repository implementations if used
- external service integrations
- JWT token service
- password hashing service
- SignalR connection mapping implementation

## 7.4 API Layer
Contains:
- controllers
- middleware
- dependency injection setup
- hub endpoints
- Swagger setup
- authentication/authorization setup

## 8. API vs SignalR Responsibility Split

### Use REST APIs for:
- register/login/refresh/logout
- create conversation
- list conversations
- get message history
- search
- update profile
- admin membership changes

### Use SignalR for:
- live message send/receive
- typing start/stop
- presence connected/disconnected
- mark-read event propagation
- new conversation notifications

This split avoids pushing historical/query operations through the hub and keeps real-time traffic focused on transient events.

## 9. Data Model Overview

### Core Tables
- `Users`
- `Roles`
- `UserRoles`
- `RefreshTokens`
- `Conversations`
- `ConversationParticipants`
- `Messages`
- `MessageReadStates`

### Optional Tables
- `MessageAttachments`
- `AuditEvents`
- `UserBlocks`

## 10. Key Design Decisions
1. **SignalR over raw WebSockets** for faster delivery and stronger .NET integration.
2. **Modular monolith** for speed, maintainability, and easier portfolio demonstration.
3. **SQL Server** to align with the Microsoft stack and your profile.
4. **JWT + refresh tokens** for realistic auth.
5. **Pagination-first message retrieval** to avoid large loads.
6. **Soft delete for messages** for auditability and future moderation support.

## 11. Sequence Flows

## 11.1 User Login
1. User submits credentials from Angular.
2. API validates credentials.
3. API generates JWT + refresh token.
4. Angular stores token securely for the chosen strategy.
5. Angular opens SignalR connection using access token.

## 11.2 Send Message
1. User types message in Angular chat component.
2. Angular sends message via SignalR hub.
3. Hub validates identity and conversation membership.
4. Hub delegates to application service.
5. Service persists message to SQL Server.
6. Hub broadcasts message event to conversation group.
7. UI updates conversation order and unread state.

## 11.3 Load History
1. Angular calls REST endpoint for message history.
2. API validates membership.
3. API queries paginated messages from SQL Server.
4. API returns DTO page to Angular.
5. UI prepends older messages.

## 12. Security Architecture
- JWT auth for APIs and hub access
- server-side authorization per conversation
- password hashing
- secret management by environment
- CORS by allowed origin
- validation on DTOs
- rate limit login path when feasible
- standard secure response headers in non-dev

## 13. Performance Considerations
- index `Messages` on `ConversationId, SentAt DESC`
- index `ConversationParticipants` by `UserId`
- fetch conversation list using last-message projection query
- use paginated history, not full history load
- optionally use Redis or Azure SignalR later for scale-out

## 14. Deployment Architecture (Target)

### Local Development
- Angular dev server
- ASP.NET Core API
- SQL Server container or local instance

### Production-Style Future State
- Angular static build behind Nginx or App Service
- ASP.NET Core API container / Azure App Service
- SQL Server or Azure SQL
- Azure SignalR Service optional
- Key Vault / secret store
- centralized logging

## 15. Observability
Recommended:
- Serilog console + rolling file sink locally
- request logging middleware
- exception middleware
- business-event logs for login, conversation creation, message send failure

## 16. Tech Stack Summary Table

| Layer | Technology |
|---|---|
| Frontend | Angular, TypeScript, RxJS |
| Realtime | SignalR (`@microsoft/signalr`, ASP.NET Core SignalR) |
| Backend API | ASP.NET Core Web API (.NET 8) |
| Auth | JWT, Refresh Tokens, ASP.NET Identity or custom auth |
| Persistence | SQL Server, EF Core |
| Logging | Serilog |
| API Docs | Swagger / OpenAPI |
| Local Dev | Docker Compose |
| Testing | xUnit, minimal Angular unit tests |

## 17. Recommended MVP Build Order
1. database schema
2. auth API
3. Angular auth screens
4. conversations API
5. message history API
6. SignalR live messaging
7. group chat
8. unread/read state
9. polish, docs, Docker, seed data
