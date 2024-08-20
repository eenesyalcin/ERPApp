import { Component, ElementRef, ViewChild } from '@angular/core';
import { OrderModel } from '../../models/order.model';
import { SharedModule } from '../../modules/shared.module';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { OrderPipe } from '../../pipes/order.pipe';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [SharedModule, OrderPipe],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent {

  orders: OrderModel[] = [];
  search: string = "";

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;
  createModel: OrderModel = new OrderModel();
  updateModel: OrderModel = new OrderModel();

  constructor(
    private http: HttpService,
    private swal: SwalService
  ) {}

  ngOnInit(): void {
    this.getAll();
  }

  getAll(){
    this.http.post<OrderModel[]>("Orders/GetAll", {}, (res) => {
      this.orders = res;
    });
  }

  create(form: NgForm){
    if (form.valid) {
      this.http.post<string>("Orders/Create", this.createModel, (res) => {
        this.swal.callToast(res);
        this.createModel = new OrderModel();
        this.createModalCloseBtn?.nativeElement.click();
        this.getAll();
      });
    }
  }

  deleteById(model: OrderModel){
    const number: string = "TS" + model.orderNumberYear + model.orderNumber;
    this.swal.callSwal("Siparişi Sil", `${model.customer.name} - ${number} numaralı siparişi silmek istiyor musunuz?`, () => {
      this.http.post<string>("Orders/DeleteById", {id: model.id}, (res) => {
        this.getAll();
        this.swal.callToast(res, "info");
      });
    });
  }

  get(model: OrderModel){
    this.updateModel = {...model};
  }

  update(form: NgForm){
    if (form.valid) {
      this.http.post<string>("Orders/Update", this.updateModel, (res) => {
        this.swal.callToast(res, "warning");
        this.updateModalCloseBtn?.nativeElement.click();
        this.getAll();
      });
    }
  }

}
