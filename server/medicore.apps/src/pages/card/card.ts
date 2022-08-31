import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';

/**
 * Generated class for the CardPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-card',
  templateUrl: 'card.html',
})
export class CardPage {

  params:any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams) {
      this.params.data = {
            items: [
                {
                    id: 1,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/0.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 2,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/1.jpg',
                    description: 'Birth year: 1980',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 3,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/2.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 4,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/3.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 5,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/4.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 6,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/5.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 7,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/6.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                },
                {
                    id: 8,
                    title: 'Atrist Name',
                    image: 'assets/images/avatar-small/7.jpg',
                    description: 'Birth year: 1984',
                    shortDescription: 'Country: Germany',
                    longDescription: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do',
                    iconLike: 'icon-thumb-up',
                    iconFavorite: 'icon-heart',
                    iconShare: 'icon-share-variant'
                }
            ]
        };

        this.params.events = {
            'onItemClick': (item: any) => {
              console.log(item);
            },
            'onExplore': (item: any) => {
              console.log("Explore");
            },
            'onShare': (item: any) => {
              console.log("Share");
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

  ionViewDidLoad() {
    console.log('ionViewDidLoad CardPage');
  }

}
