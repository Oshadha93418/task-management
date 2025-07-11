import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ErrorNotificationsComponent } from './components/error-notifications/error-notifications.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ErrorNotificationsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TaskManagementFrontend';
}
