import { Component, ViewContainerRef, OnInit } from '@angular/core';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { LoaderService } from './core/loader.service';
import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showLoader: boolean;
  constructor(
    public toastr: ToastsManager,
    vRef: ViewContainerRef,
    private loaderService: LoaderService,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {
    
    
    this.toastr.setRootViewContainerRef(vRef);
  }
  ngOnInit() {
    this.loaderService.status.subscribe((val: boolean) => {
      this.showLoader = val;
    });
  }
}
