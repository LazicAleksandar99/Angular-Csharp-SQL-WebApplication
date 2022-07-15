import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { StorageService } from 'src/app/shared/services/storage.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective{
  @Input()
  set appHasRole(role: string){

    if(this.storageService.hasRole(role)){

      this.viewContainerRef.createEmbeddedView(this.templateRef)
    } else{

      this.viewContainerRef.clear();
    }
  }
  constructor(private templateRef: TemplateRef<any>, private viewContainerRef: ViewContainerRef,private storageService: StorageService) { }

}
