import { Injectable } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  requestCount = 0;
  constructor(private spinnerService: NgxSpinnerService) { }

  startSpin(){
    this.requestCount++;
    this.spinnerService.show();
  }
  stopSpin(){
    this.requestCount--;
    if(this.requestCount <= 0){
      this.requestCount = 0;
      this.spinnerService.hide();
    }
  }
}
