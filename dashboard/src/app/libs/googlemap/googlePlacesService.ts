import { Injectable } from '@angular/core';
import { MapsAPILoader } from '@ng-maps/core';
import { Observable } from 'rxjs';
import { Marker } from '../';

declare var google: any;

@Injectable()
export class GooglePlacesService{
    constructor(private mapsAPILoader: MapsAPILoader){

    }

    getAddressFromCurrentCoordinate(marker: Marker):Observable<object>{        
        return Observable.create(observer =>{
            var latlng = new google.maps.LatLng(marker.lat, marker.lng);
            this.mapsAPILoader.load().then(() =>{
                let geocoder = new google.maps.Geocoder();
                geocoder.geocode({
                    'latLng': latlng
                }, function (results, status) {
                    if (status === google.maps.GeocoderStatus.OK) {
                        let address = results[0];
                        if (address) {                 
                            observer.next(address);
                            observer.complete();                       
                        } else {
                            observer.error("No location found");
                            observer.complete();
                        }
                    } else {
                        observer.error('Geocoder failed due to: ' + status);
                        observer.complete();
                    }
                });
            });
        })
    }
}