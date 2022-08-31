import { NavController, NavParams } from 'ionic-angular';
import { Component, Input, ViewChild, AfterViewInit } from '@angular/core';
import { Content, FabButton } from 'ionic-angular';
import * as Constant from '../../util/constants';
import { ArticleService } from '../../providers';
import { ArticleModel } from '../../model';

/**
 * Generated class for the DoctorDetailPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-doctor-detail',
  templateUrl: 'doctor-detail.html',
  providers: [ArticleService]
})
export class DoctorDetailPage implements AfterViewInit {

  category: string = "info";
  public kalenderPetugas: boolean = true; //Whatever you want to initialise it as
  public jadwalPetugas: boolean = true;
  dataFeed: any = {};
  eventsFeed: any = {};
  help:any = {};

  @Input() data: any;
  @Input() events: any;
  @ViewChild(Content)
  content: Content;
  @ViewChild(FabButton)
  fabButton: FabButton;
  articles: ArticleModel[];
  constructor(public navCtrl: NavController, public navParams: NavParams, public articleService: ArticleService) {
    this.getArticleStaffById();

    this.dataFeed = {
      items: [
        {
          id: 1,
          title: 'Card Title 1',
          titleHeader: 'Lorem Ipsum 1',
          description: 'Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock',
          image: 'assets/images/background/1.jpg',
          button: 'EXPLORE',
          shareButton: 'SHARE'
        },
        {
          id: 2,
          title: 'Card Title 2',
          titleHeader: 'Lorem Ipsum 2',
          description: 'Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock',
          image: 'assets/images/background/2.jpg',
          button: 'EXPLORE',
          shareButton: 'SHARE'
        },
        {
          id: 3,
          title: 'Card Title 3',
          titleHeader: 'Lorem Ipsum 3',
          description: 'Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock',
          image: 'assets/images/background/5.jpg',
          button: 'EXPLORE',
          shareButton: 'SHARE'
        },
        {
          id: 4,
          title: 'Card Title 4',
          titleHeader: 'Lorem Ipsum 4',
          description: 'Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock',
          image: 'assets/images/background/3.jpg',
          button: 'EXPLORE',
          shareButton: 'SHARE'
        },
        {
          id: 5,
          title: 'Card Title 5',
          titleHeader: 'Lorem Ipsum 5',
          description: 'Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock',
          image: 'assets/images/background/1.jpg',
          button: 'EXPLORE',
          shareButton: 'SHARE'
        }
      ]
    };

    this.help = {
            items: [
                {
                    id: 1,
                    title: 'Product 1',
                    backgroundImage: 'assets/images/background/22.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 2,
                    title: 'Product 2',
                    backgroundImage: 'assets/images/background/23.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 3,
                    title: 'Product 3',
                    backgroundImage: 'assets/images/background/24.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 4,
                    title: 'Product 4',
                    backgroundImage: 'assets/images/background/25.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 5,
                    title: 'Product 5',
                    backgroundImage: 'assets/images/background/26.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 6,
                    title: 'Product 5',
                    backgroundImage: 'assets/images/background/27.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                },
                {
                    id: 7,
                    title: 'Product 5',
                    backgroundImage: 'assets/images/background/28.jpg',
                    button: 'BUY',
                    items: [
                        'PAY WITH PAYPAL',
                        'PAY WITH VISA CARD',
                        'PAY WITH MAESTRO CARD'
                    ]
                }
            ]
        };

    this.eventsFeed = {
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
    console.log('ionViewDidLoad DoctorDetailPage');
  }

  public onButtonClick(id: any) {
    if (id === 0) {
      this.kalenderPetugas = !this.kalenderPetugas;
    } else {
      this.jadwalPetugas = !this.jadwalPetugas;
    }
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

  getArticleStaffById() {
    // ganti nanti jadi model
    let staffId = 1;
    let pageIndex = 1;
    let pageSize = Constant.PAGE_SIZE;

    this.articleService.getArticleStaffById(staffId, pageIndex, pageSize).subscribe(
      res => {
        console.log(res.items);
        this.articles = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

}
