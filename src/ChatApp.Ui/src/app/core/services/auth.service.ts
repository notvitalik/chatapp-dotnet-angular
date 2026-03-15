import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenState = signal<string | null>(null);

  readonly token = this.tokenState.asReadonly();

  isAuthenticated(): boolean {
    return this.tokenState() !== null;
  }

  getAccessToken(): string | null {
    return this.tokenState();
  }

  setAccessToken(token: string | null): void {
    this.tokenState.set(token);
  }
}
