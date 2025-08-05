// import { TestBed } from '@angular/core/testing';
// import { NotificationService } from '../services/notification.service';
// import { NgZone } from '@angular/core';
// import * as signalR from '@microsoft/signalr';

// describe('NotificationService', () => {
//   let service: NotificationService;
//   let zone: NgZone;
//   let mockHubConnection: jasmine.SpyObj<signalR.HubConnection>;

//   beforeEach(() => {
//     const mockNgZone = new NgZone({ enableLongStackTrace: false });

//     // Mock SignalR HubConnection methods
//     mockHubConnection = jasmine.createSpyObj('HubConnection', [
//       'start',
//       'stop',
//       'on',
//       'off',
//       'invoke'
//     ]);
//     mockHubConnection.start.and.returnValue(Promise.resolve());
//     mockHubConnection.stop.and.returnValue(Promise.resolve());

//     spyOn(signalR, 'HubConnectionBuilder').and.returnValue({
//       withUrl: () => ({
//         withAutomaticReconnect: () => ({
//           configureLogging: () => ({
//             build: () => mockHubConnection
//           })
//         })
//       })
//     } as any);

//     TestBed.configureTestingModule({
//       providers: [
//         NotificationService,
//         { provide: NgZone, useValue: mockNgZone }
//       ]
//     });

//     service = TestBed.inject(NotificationService);
//     zone = TestBed.inject(NgZone);
//   });

//   it('should start connection and register handlers', async () => {
//     service.startConnection('user123', 'admin');
//     expect(mockHubConnection.start).toHaveBeenCalled();
//     expect(service.isConnectionActive()).toBeTrue();
//   });


//   it('should stop connection', async () => {
//     service['hubConnection'] = mockHubConnection;
//     service['isStarted'] = true;

//     await service.stopConnection();

//     expect(mockHubConnection.stop).toHaveBeenCalled();
//     expect(service.isConnectionActive()).toBeFalse();
//   });
// });
