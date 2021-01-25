import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import * as CryptoJS from 'crypto-js';


import { User } from '../model/user';
import { environment } from '../../environments/environment';
import jsSHA from 'jssha';


@Injectable({ providedIn: 'root' })
export class WebsocketService {
  private ws = new WebSocket(environment.wsUrl);

  constructor(
    
  ) {
    
  }

  private create(url: string) {
    this.ws = new WebSocket(environment.wsUrl); 
    this.ws.onopen = (evt) => {
      console.log("ws connected!" )
    };

  }


  send(command): Observable<any> {
    let data = {
      jsonData: JSON.stringify(command)
    };

    if (this.ws.readyState === WebSocket.OPEN) {
      this.ws.send(JSON.stringify(data));
    }
    
    return Observable.create(observer => {
      this.ws.onmessage = (evt) => {
        //var responds = this.blobToString(evt.data);
        //observer.next(JSON.parse(responds));
        observer.next(JSON.parse(evt.data));
      };
    });
  }

  blobToString(blob) {
    var u, x;
    u = URL.createObjectURL(blob);
    x = new XMLHttpRequest();
    x.open('GET', u, false); 
    x.send();
    URL.revokeObjectURL(u);
    return x.responseText;
  }

  jsSHA_hass(password,user) {
    var shaObj = new jsSHA("SHA-256", "TEXT");
    shaObj.setHMACKey(password, "TEXT");
    shaObj.update(user);
    return shaObj.getHMAC("HEX"); //hashedPassword will contain the value 
  }

  hash_hmac(hasspassword, secretKey) {
    return CryptoJS.HmacSHA256(hasspassword, secretKey).toString();
   }

 
}
