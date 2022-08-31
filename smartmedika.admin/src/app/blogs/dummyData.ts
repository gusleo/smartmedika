import { BlogCategoryModel, BlogModel } from '../model';

export class DummyDataBlogCategory {
        getAllCategory(): Array<BlogCategoryModel> {
        return [
            {id: 1, parentId: 0, name: 'Balita', slug: '', isVisible: null, articleCount: 0},
            {id: 2, parentId: 0, name: 'Ibu Hamil', slug: '', isVisible: null, articleCount: 0}            

        ];
    }

    getAllArticle(): Array<BlogModel> {
        return [
            new BlogModel(1, 'Tanda-Tanda Anak Sehat', 1,
            'Sebelum memeriksakan kesehatan anak ke dokter, bagian tubuh ini bisa menjadi parameter sehatnya seorang anak.<br>Seperti disampaikan dokter pemerhati gaya hidup, Dr Grace Judio-Kahl, MSc, MH, secara fisik orangtua dapat melihat kriteria anak sehat dengan melihat fisiknya saja, selain dari segi keaktifan sang anak dalam berkegiatan.<br />Selain itu, bagian bawah mata yang berwarna merah menjadi tanda bahwa anak tergolong sehat.<br />"Kuku juga menjadi cara melihat anak sehat, kuku anak sehat itu warnanya pink-pink merah bukan pink yang pucat. Untuk anak aja itu saja cukup menandakan anaknya sehat dan aktif," tutupnya.',
            'Sebelum memeriksakan kesehatan anak ke dokter, bagian tubuh ini bisa menjadi parameter sehatnya seorang anak.<br>Seperti disampaikan dokter pemerhati gaya hidup, Dr Grace Judio-Kahl, MSc, MH, secara fisik orangtua dapat melihat kriteria anak sehat dengan melihat fisiknya saja, selain dari segi keaktifan sang anak dalam berkegiatan',
            false, 0, '17 Juli 2017'),
            new BlogModel(2, 'Rumah Sakit Anak Ini Gunakan Pokemon Go untuk Hibur Pasien', 1,
            'Sebuah rumah sakit di Ann Arbor, Michigan, Amerika Serikat, C.S. Mott Children Hospital di University of Michigan mengizinkan pasien-pasiennya bermain Pokemon Go. Para pasien tampak mulai berkeliling lorong rumah sakit dan kampus memainkan Pokemon Go dan berinteraksi dengan pasien lainnya.<br>J.J. Bouchard, manajer digital media rumah sakit yang baru, berhari-hari fokus mencari teknologi baru untuk anak yang bisa memberi hiburan sekaligus manfaat therapeutic yang bisa digunakan di lingkungan rumah sakit. Saat melihat fenomena Pokemon Go, Bouchard langsung yakin game tersebut merupakan pilihan terbaik. Dia pun langsung mendiskusikannya dengan tim.<br>Bouchard yang juga mengantongi ijazah spesialis anak dan hiburan therapeutic mengatakan tugas barunya di rumah sakit memungkinkan dirinya bekerja sama dengan developer game untuk meredakan stres dan sakit dampak dari prosedur pengobatan yang rumit pada pasien anak-anak.<br>Sejauh ini sifat augmented reality yang diberikan Pokemon Go pada pemainnya membuat pasien-pasien cilik di Mott bergerak dan tidak terlalu memikirkan rasa takut mereka menghadapi penyakit.<br>Mayo, salah seorang insinyur rehabilitasi Mott menyatakan manfaat Pokemon Go bagi para pasien adalah membuat hidup mereka menjadi lebih normal dan merekatkan hubungan sesama pasien serta para orangtua.<br>"Mereka mendengar permainan ini dari teman-temannya di rumah dan sekarang mereka juga merasa bisa ikut memainkannya. Game ini juga membutuhkan interaksi antara pasien dan orangtuanya karena menggunakan GPS. Jadi tentu menyenangkan bagi orangtua melihat senyum di wajah anak-anak mereka," tutur Mayo.<br>Bouchard dan timnya setiap hari memantau keamanan para pasien saat memainkan Pokemon Go. Ada spot spesial untuk mendapatkan Pokemon yang bisa digunakan para pasien dengan aman.<br>Bouchard dan Mayo berharap suatu saat nanti game seperti Pokemon Go akan menginspirasi profesional kesehatan untuk membuka mata mereka pada manfaat bermain game dan virtual reality, terutama bagi pasien cilik.',
            'Sebuah rumah sakit di Ann Arbor, Michigan, Amerika Serikat, C.S. Mott Children Hospital di University of Michigan mengizinkan pasien-pasiennya bermain Pokemon Go. Para pasien tampak mulai berkeliling lorong rumah sakit dan kampus memainkan Pokemon Go dan berinteraksi dengan pasien lainnya.',
            false, 0, '22 Juli 2016')            
        ];        
    }
}