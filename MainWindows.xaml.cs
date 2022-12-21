using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CockroachRace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _totalScore = 2000, _bet = 200;
        private int _myCockroach = 1, _myPlace, _playerSpeed;
        private bool _isGaming, _isSpeed, _isTapok, _countkill;
        private int _winner;
        private string _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ФРОЛ» — \nБОЛЕЕТ ЗА СБОРНУЮ РОССИИ  ПО ФУТБОЛУ.";
        private Task _cockroach1, _cockroach2;
        CancellationTokenSource cts1, cts2, cts3, cts4, cts5, cts6, cts7, cts8;
        CancellationTokenSource[] cts = new CancellationTokenSource[9];

        public MainWindow()
        {
            InitializeComponent();
            Task _loader = loader();
            RestartGameInit();
        }

        private async Task loader()
        {
            await Task.Delay(5000);
            firstLayer.Visibility = Visibility.Hidden;
        }

        private void StartBtn(object sender, MouseButtonEventArgs e)
        {
            _bet = Int32.Parse(betEdit.Text);
            if (_bet > 0 && _bet <= _totalScore)
            {
                _totalScore -= _bet;
                totalCount.Content = _totalScore.ToString();
                _winner = 0;
                _isGaming = true;
                _isSpeed = false;
                _isTapok = false;
                _countkill = true;
                StartGame();
            }
        }

        private void StartGame()
        {
            startbtn.Visibility = Visibility.Hidden;
            startbtnShadow.Visibility = Visibility.Hidden;
            betEdit.Visibility = Visibility.Hidden;
            about_cockroach.Visibility = Visibility.Hidden;

            if (_totalScore >= 50)
            {
                image2.Source = new BitmapImage(new Uri(@"/assets/turbo.png", UriKind.Relative));
                _isSpeed = true;
            }
            if (_totalScore >= 100)
            {
                image3.Source = new BitmapImage(new Uri(@"/assets/tapok.png", UriKind.Relative));
                _isTapok = false;
            }

           // var cts = new CancellationTokenSource[9];
            //var cockroaches = new Image[9];
            //var _cockroaches = new Task[9];

            for (int i = 0; i < 8; i++)
            {
                cts[i] = new CancellationTokenSource();
            }

            _cockroach1 = SetSpeed(cockroach1, 1, 1, cts[0].Token);
            _cockroach2 = SetSpeed(cockroach2, 2, 1, cts[1].Token);
            Task _cockroach3 = SetSpeed(cockroach3, 3, 1, cts[2].Token);
            Task _cockroach4 = SetSpeed(cockroach4, 4, 1, cts[3].Token);
            Task _cockroach5 = SetSpeed(cockroach5, 5, 1, cts[4].Token);
            Task _cockroach6 = SetSpeed(cockroach6, 6, 1, cts[5].Token);
            Task _cockroach7 = SetSpeed(cockroach7, 7, 1, cts[6].Token);
            Task _cockroach8 = SetSpeed(cockroach8, 8, 1, cts[7].Token);

        }

        private async Task SetSpeed(Image _cockroach, int _number, int _speed, CancellationToken token)
        {
            int _x = 125;
            bool _win = true;
            Random rnd = new();
         
            while (_x <= 830)
            {
                if (token.IsCancellationRequested) return;

                _x += rnd.Next(2, 15) * _speed; //speed
                if (_myCockroach == _number) {
                    _x += _playerSpeed;
                }

                Thickness margin = _cockroach.Margin;
                margin.Left = _x;
                _cockroach.Margin = margin;

                if (_x >= 764)
                {
                    if (_winner == 0) _winner = _number;
                    if (_win) _myPlace++;

                    if (_myCockroach == _number && _win)
                    {

                        if (_myCockroach == _winner)
                        {
                            _totalScore += _bet * 2;
                            betWin.Content = (_bet * 2).ToString() + "+";
                            totalCount.Content = _totalScore.ToString();
                            winner.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            betWin.Content = _bet.ToString() + "-";
                            lose.Visibility = Visibility.Visible;
                        }

                        place.Content = _myPlace.ToString();
                        totalCountW.Content = _totalScore.ToString();
                        _isSpeed = false;
                        _isTapok = false;
                        _countkill = true;
                        GameOver();
                    }
                    _win = false;
                }
                await Task.Delay(50);
            };
        }

        private void ClickTapokBtn(object sender, MouseButtonEventArgs e)
        {

            if (_isGaming && !_isTapok && _countkill)
            {
                if (_totalScore >= 100)
                {
                    _totalScore -= 100;
                    _isTapok = true;
                    totalCount.Content = _totalScore.ToString();
                    image3.Source = new BitmapImage(new Uri(@"/assets/tapok_disabled.png", UriKind.Relative));

                }
            }
        }

        private void RestartGame(object sender, MouseButtonEventArgs e)
        {
            RestartGameInit();
        }

        private void RestartGameInit()
        {
            about_cockroach.Content = _aboutCockroach;
            image4.Visibility = Visibility.Hidden;
            winner.Visibility = Visibility.Hidden;
            lose.Visibility = Visibility.Hidden;
            reloadBtn.Visibility = Visibility.Hidden;
            totalCountW.Visibility = Visibility.Hidden;
            place.Visibility = Visibility.Hidden;
            betWin.Visibility = Visibility.Hidden;

            cockroach1.Margin = new Thickness(125, 170, 0, 0);
            cockroach2.Margin = new Thickness(125, 213, 0, 0);
            cockroach3.Margin = new Thickness(125, 258, 0, 0);
            cockroach4.Margin = new Thickness(125, 300, 0, 0);
            cockroach5.Margin = new Thickness(125, 344, 0, 0);
            cockroach6.Margin = new Thickness(125, 388, 0, 0);
            cockroach7.Margin = new Thickness(125, 432, 0, 0);
            cockroach8.Margin = new Thickness(125, 477, 0, 0);

            startbtn.Visibility = Visibility.Visible;
            startbtnShadow.Visibility = Visibility.Visible;
            betEdit.Visibility = Visibility.Visible;
            about_cockroach.Visibility = Visibility.Visible;

            _playerSpeed = 0;
            _myPlace = 0;
            _isSpeed = false;
            _isTapok = false;
            _isGaming = false;
            _countkill = true;

            image2.Source = new BitmapImage(new Uri(@"/assets/turbo_disabled.png", UriKind.Relative));
            image3.Source = new BitmapImage(new Uri(@"/assets/tapok_disabled.png", UriKind.Relative));

        }

        private void GameOver()
        {
            image4.Visibility = Visibility.Visible;
            reloadBtn.Visibility = Visibility.Visible;
            totalCountW.Visibility = Visibility.Visible;
            place.Visibility = Visibility.Visible;
            betWin.Visibility = Visibility.Visible;
        }

        private void ClickSpeedBtn(object sender, MouseButtonEventArgs e)
        {
            if (_isGaming && _isSpeed)
            {
                if (_totalScore >= 50) {
                    _totalScore -= 50;
                    _playerSpeed = 1;
                    _isSpeed = false;
                    totalCount.Content = _totalScore.ToString();
                    image2.Source = new BitmapImage(new Uri(@"/assets/turbo_disabled.png", UriKind.Relative));
                    
                }
            }
        }

        private void ChangeCockroach(object sender, MouseButtonEventArgs e)
        {
            if (_isGaming && _isTapok && _countkill)
            {

                FrameworkElement b = (FrameworkElement)sender;
                string _name = b.Name;
                bool _cheak = true;
                switch (_name)
                {
                    case "cockroach1":
                        if (_myCockroach == 1)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[0].Cancel();
                        }
                        break;
                    case "cockroach2":
                        if (_myCockroach == 2)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[1].Cancel();
                        }
                        break;
                    case "cockroach3":
                        if (_myCockroach == 3)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[2]?.Cancel();
                        }
                        break;
                    case "cockroach4":
                        if (_myCockroach == 4)
                        { 
                            _cheak = false;
                        }
                        else
                        {
                            cts[3]?.Cancel();
                        }
                        break;
                    case "cockroach5":
                        if (_myCockroach == 5)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[4]?.Cancel();
                        }
                        break;
                    case "cockroach6":
                        if (_myCockroach == 6)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[5]?.Cancel();
                        }
                        break;
                    case "cockroach7":
                        if (_myCockroach == 7)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[6]?.Cancel();
                        }
                        break;
                    case "cockroach8":
                        if (_myCockroach == 8)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[7]?.Cancel();
                        }
                        break;
                }

                if (_cheak)
                {
                    Image srcImage = e.Source as Image;
                    srcImage.Source = new BitmapImage(new Uri(@"/assets/cockroach_die.png", UriKind.Relative));
                    _isTapok = false;
                    _countkill = false;
                }
            }

            if (!_isGaming)
            {
                int _ringX = 172;
                
                FrameworkElement b = (FrameworkElement)sender;
                string _name = b.Name;

                switch (_name)
                {
                    case "cockroach1":
                        _myCockroach = 1;
                        _ringX = 172;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ФРОЛ» — \nБОЛЕЕТ ЗА СБОРНУЮ  РОССИИ  ПО ФУТБОЛУ.";
                        break;
                    case "cockroach2":
                        _myCockroach = 2;
                        _ringX = 215;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «БАГЗ БАННИ» — \nОЧЕНЬ УМНЫЙ ТАРАКАН,\nРАНЬШЕ ОБИТАЛ В БИБЛИОТЕКЕ.";
                        break;
                    case "cockroach3":
                        _myCockroach = 3;
                        _ringX = 261;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «СТАСИК» — \nЕГО ПРИНЕС, КАКОЙ-ТО СТАРИК.\nСКАЗАЛ, ЧТО ОН БУДУЮЩИЙ ЧЕМПИОН.";
                        break;
                    case "cockroach4":
                        _myCockroach = 4;
                        _ringX = 303;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ОЛИВЬЕ» — \nОЧЕНЬ ЛЮБИТ ПОЛАКОМИТСЯ ОЛИВЬЕ, ОТТУДА\nИ НЕИМОВЕРНАЯ СИЛА.";
                        break;
                    case "cockroach5":
                        _myCockroach = 5;
                        _ringX = 347;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ЖУШИРО» — \nПРИБЫЛ НА КОРАБЛЕ ИЗ ЯПОНИИ.\nПЛОХОЕ ЗРЕНИЕ.";
                        break;
                    case "cockroach6":
                        _myCockroach = 6;
                        _ringX = 390;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «КЛЯКСИК» — \nНЕБОЛЬШОЙ ТАРАКАН, НО ОЧЕНЬ БЫСТРЫЙ\nДЛЯ СВОЕГО РАЗМЕРА.";
                        break;
                    case "cockroach7":
                        _myCockroach = 7;
                        _ringX = 434;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ХАРПЕР» — \nАМЕРИКАНЕЦ, ПОСТОЯННО ЕСТЬ ПРОТЕИНОВЫЕ БАТОНЧИКИ.";
                        break;
                    case "cockroach8":
                        _myCockroach = 8;
                        _ringX = 480;
                        _aboutCockroach = "ТАРАКАН ПО КЛИЧКЕ «ЧУЧУНДРА» — \nРАНЬШЕ ЖИЛ НА ПОМОЙКЕ.\nИНОГДА ПОДТУПЛИВАЕТ.";
                        break;
                    default:
                        _myCockroach = 1;
                        break;
                }
                about_cockroach.Content = _aboutCockroach;
                
                Thickness margin = ring.Margin;
                margin.Top = _ringX;
                ring.Margin = margin;
            }
            
        }
    }
}
