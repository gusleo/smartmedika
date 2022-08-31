import { NavController, NavParams } from 'ionic-angular';
import { Component, Input, ViewChild } from '@angular/core';
import { Content, FabButton } from 'ionic-angular';
import { AppointmentPage } from '../appointment/appointment';

/**
 * Generated class for the ClinicDetailPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-clinic-detail',
  templateUrl: 'clinic-detail.html',
})
export class ClinicDetailPage {

  category: string = "info";
  dokter: any = {};
  help:any = {};
  @Input() data: any;
  @Input() events: any;
  @ViewChild(Content)
  content: Content;
  @ViewChild(FabButton)
  fabButton: FabButton;
  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.dokter = {
      items: [
        {
            id: 1,
            title: 'Dokter Adi',
            description: 'Dokter Spesialis Anak',
            image: 'assets/images/avatar/15.jpg',
            location: 'Denpasar',
            iconLike: 'icon-thumb-up',
            iconFavorite: 'icon-heart',
            iconShare: 'icon-share-variant',
            items: [
                {
                    id: 1,
                    title: '',
                    description: 'Universal, 2016',
                    image: '',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 2,
                    title: 'AlbumName',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/11.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 3,
                    title: 'AlbumName',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/12.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 4,
                    title: 'AlbumName',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/13.jpg',
                    iconPlay: 'icon-play-circle'
                }
            ]
        },
        {
            id: 2,
            title: 'Dokter Wayan',
            description: 'BASSO',
            image: 'assets/images/avatar/2.jpg',
            location: 'Denpasar',
            iconLike: 'icon-thumb-up',
            iconFavorite: 'icon-heart',
            iconShare: 'icon-share-variant',
            items: [
                {
                    id: 1,
                    title: 'AlbumName1',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/14.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 2,
                    title: 'AlbumName2',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/15.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 3,
                    title: 'AlbumName3',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/14.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 4,
                    title: 'AlbumName4',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/13.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 5,
                    title: 'AlbumName5',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/12.jpg',
                    iconPlay: 'icon-play-circle'
                },
                {
                    id: 6,
                    title: 'AlbumName6',
                    description: 'Universal, 2016',
                    image: 'assets/images/avatar/11.jpg',
                    iconPlay: 'icon-play-circle'
                }
            ]
        }
    ]
    };

        this.events = {
          'onItemClick': (item: any) => {
            console.log(item);
          },
          'onExplore': (item: any) => {
            console.log("Explore");
          },
          'onBooking': (item: any) => {
            console.log("Booking");
            this.navCtrl.push(AppointmentPage);
          },
          'onLike': (item: any) => {
            console.log("onLike");
          },
          'onFavorite': (item: any) => {
            console.log("onFavorite");
          },
          'onFab': (item: any) => {
            console.log("Fab");
          },
        };


  }

  onEvent(event: string, item: any, e: any) {
    if (e) {
      e.stopPropagation();
    }
    if (this.events[event]) {
      this.events[event](item);
    }
  }

  toggleGroup(group: any) {
    group.show = !group.show;
  }

  isGroupShown(group: any) {
    return group.show;
  }

  ngAfterViewInit() {
    this.content.ionScroll.subscribe((d) => {
      // this.fabButton.setElementClass("fab-button-out", d.directionY == "down");
    });
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ClinicDetailPage');
  }

}
