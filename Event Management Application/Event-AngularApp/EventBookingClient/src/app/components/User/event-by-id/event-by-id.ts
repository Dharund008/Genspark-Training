import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppEvent, EventResponseTicketType } from '../../../models/event.model';
import { EventService } from '../../../services/Event/event.service';
import { v4 as uuidv4 } from 'uuid';
import { EventCategory, EventStatus, EventTypeEnum, PaymentTypeEnum, TicketTypeEnum } from '../../../models/enum';
import { TicketService } from '../../../services/Ticket/ticket.service';
import { ApiResponse } from '../../../models/api-response.model';
import { CommonModule, DatePipe } from '@angular/common';
import { signal } from '@angular/core';
import { App } from '../../../app';
import { SimilarEvents } from "../../similar-events/similar-events";
import { NotificationService } from '../../../services/Notification/notification-service';
import { UserService } from '../../../services/User/user-service';
import { UserWallet } from '../../../models/userwallet.model';

@Component({
  selector: 'app-event-by-id',
  templateUrl: './event-by-id.html',
  styleUrl: './event-by-id.css',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe, CommonModule, SimilarEvents]
})
export class EventById implements OnInit {
  event = signal<AppEvent | null>(null);
  eventId!: string;
  form!: FormGroup;
  availableToBook!: Number;
  selectedTicketType = signal<EventResponseTicketType | undefined>(undefined);
  availableSeats = signal<number[]>([]);
  bookedSeatNumbers = signal<number[]>([]);
  EventTypeEnum = EventTypeEnum;
  imageid = signal<any[] | null>(null);
  PaymentTypeEnum = PaymentTypeEnum;
  paymentTypes = Object.entries(PaymentTypeEnum).filter(([k, v]) => !isNaN(Number(v)));
  similarEvents = signal<AppEvent[]>([]);
  currentImageIndex = 0;
  imageIntervalId: any;
  userWallet?: UserWallet;

  constructor(
    private route: ActivatedRoute,
    private eventService: EventService,
    private ticketService: TicketService,
    private fb: FormBuilder,
    private router: Router,
    private notify: NotificationService,
    private userService: UserService

  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const eventId = params.get('id');
      console.log(eventId)
      if (eventId) {
        this.eventId = eventId;
        this.loadEvent();
      }
    });

    // Load wallet info
    this.userService.getWallet().subscribe({
      next: (wallet) => this.userWallet = wallet,
      error: (err) => console.warn('Failed to fetch wallet:', err)
    });
  }
  getTotalBooked(event  : AppEvent){
    return event.ticketTypes.reduce((sum, ticket) => sum + (ticket.bookedQuantity), 0);
  }
  getTotalAvailable(event  : AppEvent){
    return event.ticketTypes.reduce((sum, ticket) => sum + (ticket.totalQuantity), 0);
  }
  getSimilarEvents() {
    const categoryLabel = this.event()!.category;
    const categoryValue = (EventCategory[categoryLabel] as unknown) as number;
    // let categoryValue : number = EventCategory[categoryLabel];
    // console.log(categoryValue," ",typeof(categoryValue));
    this.eventService.getFilteredEvents(categoryValue, "", "", "", 1, 4).subscribe({
      next: (res: ApiResponse) => {
        const rawItems = res.data?.items?.$values || [];
        this.similarEvents.set(rawItems.map((e: any) => new AppEvent(e)));
        // console.log(this.similarEvents());
        this.similarEvents.set(this.similarEvents().filter(e => e?.title !== this.event()?.title).slice(0,3));
        // console.log(this.similarEvents());
      }
    });
  }
  ngOnDestroy(): void {
    if (this.imageIntervalId) clearInterval(this.imageIntervalId);
  }

  loadEvent() {
    // console.log("in Load evnt")
    this.eventService.getEventById(this.eventId).subscribe({
      next: (res: ApiResponse) => {
        const evt = new AppEvent(res.data);
        // console.log(evt);
        this.event.set(evt);
        console.log(this.event());
        this.imageid.set(evt.images ?? null);

        if ((this.imageid()?.length ?? 0) > 1) {
          this.startImageSlider();
        }

        this.bookedSeatNumbers.set(
          evt.bookedSeats.filter((s) => s.bookedSeatStatus === 0).map((s) => s.seatNumber)
        );

        this.form = this.fb.group({
          ticketTypeId: [null, Validators.required],
          quantity: [1, [Validators.required, Validators.min(1)]],
          paymentType: [null, Validators.required],
          useWallet: [false],
          seatNumbers: [[]]
        });

        this.form.get('ticketTypeId')?.valueChanges.subscribe(id => {
          const ticketType = evt.ticketTypes.find(t => t.id === id);
          this.selectedTicketType.set(ticketType);
          this.form.get('seatNumbers')?.setValue([]);
          this.form.get('quantity')?.setValue(1);
          this.availableToBook = (ticketType?.totalQuantity ?? 0) - (ticketType?.bookedQuantity ?? 0);
        });

        this.form.get('ticketTypeId')?.setValue(null);
        this.getSimilarEvents();
      },
      error: (err: any) => {
        this.notify.error(err.error.errors.message);
      }
    });
  }

  startImageSlider() {
    this.imageIntervalId = setInterval(() => {
      const images = this.imageid();
      if (images && images.length > 1) {
        this.currentImageIndex = (this.currentImageIndex + 1) % images.length;
      }
    }, 2000);
  }
  isCancelled(event: AppEvent): boolean {
    return event.eventStatus.toString() == "Cancelled";
  }

  eventStatusToString(status: number): string {
    return EventStatus[status];
  }

  eventTypeToString(type: number): string {
    return EventTypeEnum[type];
  }

  ticketTypeToString(type: number): string {
    return TicketTypeEnum[type];
  }
  toggleSeat(seat: number) {
    const currentSeats = this.form.value.seatNumbers as number[];
    const index = currentSeats.indexOf(seat);

    if (index > -1) {
      currentSeats.splice(index, 1);
    } else if (currentSeats.length < this.form.value.quantity) {
      currentSeats.push(seat);
    }

    this.form.get('seatNumbers')?.setValue([...currentSeats]);
  }

  isSeatBooked(seat: number): boolean {
    return this.bookedSeatNumbers().includes(seat);
  }

  isSeatSelected(seat: number): boolean {
    return this.form.value.seatNumbers?.includes(seat);
  }

  generateSeats(total: number): number[] {
    return Array.from({ length: total }, (_, i) => i + 1);
  }

  submit() {
    if (this.form.invalid){
      // alert("Enter all required details!");
      return;
    }

    const evt = this.event();
    if (!evt) return;

    const isSeatable = this.event()?.eventType.toString() == this.eventTypeToString(0)

    if(isSeatable){
      let seats = this.form.value.seatNumbers;
      if(seats.length != this.form.value.quantity){
        this.notify.info("select for all required quantity!")
        return;
      }
    }
    const payload = {
      EventId: evt.id,
      TicketTypeId: this.form.value.ticketTypeId,
      Quantity: this.form.value.quantity,
      SeatNumbers: isSeatable ? this.form.value.seatNumbers : null,
      Payment: {
        PaymentType: this.form.value.paymentType,
        UseWallet: this.form.value.useWallet,
        TransactionId: uuidv4(),
      }
    };
    // console.log(payload)
    this.ticketService.bookTicket(payload).subscribe({
      next: () => this.router.navigate(['/user']),
      error: (err:any) => {console.log(err);this.notify.error(err.error.errors.message)},
    });
  }

}

