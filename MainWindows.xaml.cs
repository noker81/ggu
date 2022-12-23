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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace CockroachRace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _totalScore = 2000, _bet = 200;
        private int _myCockroach, _myPlace;
        private float _playerSpeed;
        private bool _isGaming, _isSpeed, _isTapok, _countkill;
        private int _winner;
        private Image[] _img, _objects;
        CancellationTokenSource[] cts = new CancellationTokenSource[8];
        private string[] _aboutCockroach;

        public MainWindow()
        {
            InitializeComponent();

            _img = new Image[] { cockroach1, cockroach2, cockroach3, cockroach4, cockroach5, cockroach6, cockroach7, cockroach8 };
            _aboutCockroach = new string[] { "ТАРАКАН ПО КЛИЧКЕ «ФРОЛ» — \nБОЛЕЕТ ЗА СБОРНУЮ  РОССИИ  ПО ФУТБОЛУ.",
                    "ТАРАКАН ПО КЛИЧКЕ «БАГЗ БАННИ» — \nОЧЕНЬ УМНЫЙ ТАРАКАН,\nРАНЬШЕ ОБИТАЛ В БИБЛИОТЕКЕ.",
                    "ТАРАКАН ПО КЛИЧКЕ «СТАСИК» — \nЕГО ПРИНЕС, КАКОЙ-ТО СТАРИК.\nСКАЗАЛ, ЧТО ОН БУДУЮЩИЙ ЧЕМПИОН.",
                    "ТАРАКАН ПО КЛИЧКЕ «ОЛИВЬЕ» — \nОЧЕНЬ ЛЮБИТ ПОЛАКОМИТСЯ ОЛИВЬЕ, ОТТУДА\nИ НЕИМОВЕРНАЯ СИЛА.",
                    "ТАРАКАН ПО КЛИЧКЕ «ЖУШИРО» — \nПРИБЫЛ НА КОРАБЛЕ ИЗ ЯПОНИИ.\nПЛОХОЕ ЗРЕНИЕ.",
                    "ТАРАКАН ПО КЛИЧКЕ «КЛЯКСИК» — \nНЕБОЛЬШОЙ ТАРАКАН, НО ОЧЕНЬ БЫСТРЫЙ\nДЛЯ СВОЕГО РАЗМЕРА.",
                    "ТАРАКАН ПО КЛИЧКЕ «ХАРПЕР» — \nАМЕРИКАНЕЦ, ПОСТОЯННО ЕСТЬ ПРОТЕИНОВЫЕ БАТОНЧИКИ.",
                    "ТАРАКАН ПО КЛИЧКЕ «ЧУЧУНДРА» — \nРАНЬШЕ ЖИЛ НА ПОМОЙКЕ.\nИНОГДА ПОДТУПЛИВАЕТ."};

            _objects = new Image[] { image4, winner, lose, reloadBtn};
            Task _loader = loader();
            RestartGameInit();
        }

        private async Task loader()
        {
            await Task.Delay(4000);
            firstLayer.Visibility = Visibility.Hidden;
        }

        private void StartBtn(object sender, MouseButtonEventArgs e)
        {

            if (int.TryParse(betEdit.Text, out _bet))
            {
                if (_bet > 0 && _bet <= _totalScore)
                {
                    _totalScore -= _bet;
                    totalCount.Content = _totalScore.ToString();
                    _winner = 0;
                    _isGaming = _countkill = true;
                    _isSpeed = _isTapok = false;
                    StartGame();
                }
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

            Task[] _cockroachTask = new Task[_img.Length];

            for (int i = 0; i < _img.Length; i++)
            {
                cts[i] = new CancellationTokenSource();
                _cockroachTask[i] = SetSpeed(_img[i], i, 1, cts[i].Token);
            }

        }

        private async Task SetSpeed(Image _cockroach, int _number, float _speed, CancellationToken token)
        {
            float _x = 125;
            bool _win = true;
            Random rnd = new();

            while (_x <= 830)
            {
                if (token.IsCancellationRequested) return;

                _x += rnd.Next(2, 15) * _speed; //speed

                if (_myCockroach == _number)
                {
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
                        _isSpeed = _isTapok = false;
                        _countkill = true;
                        GameOver();
                    }
                    _win = false;
                }

                await Task.Delay(50, token);
            };
        }

        private void ClickTapokBtn(object sender, MouseButtonEventArgs e)
        {
            if (_isGaming && !_isTapok && _countkill)
            {
                if (_totalScore >= 100)
                {
                    _totalScore -= 100;
                    totalCount.Content = _totalScore.ToString();
                    _isTapok = true;

                    image3.Source = new BitmapImage(new Uri(@"/assets/tapok_disabled.png", UriKind.Relative));
                    StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"/assets/wait.ani", UriKind.Relative));
                    Cursor customCursor = new(sri.Stream);
                    Cursor = customCursor;

                }
            }
        }

        private void RestartGame(object sender, MouseButtonEventArgs e)
        {
            RestartGameInit();
        }

        private void RestartGameInit()
        {
            
            about_cockroach.Content = _aboutCockroach[_myCockroach];

            foreach(Image _obj in _objects)
            {
                _obj.Visibility = Visibility.Hidden;
            }
            totalCountW.Visibility = Visibility.Hidden;
            place.Visibility = Visibility.Hidden;
            betWin.Visibility = Visibility.Hidden;
            


            for (int i = 0; i < _img.Length; i++)
            {
                cts[i]?.Cancel();
                _img[i].Source = new BitmapImage(new Uri(@"/assets/cockroach.png", UriKind.Relative));
                _img[i].Margin = new Thickness(125, _img[i].Margin.Top, 0, 0);
            }

            startbtn.Visibility = Visibility.Visible;
            startbtnShadow.Visibility = Visibility.Visible;
            betEdit.Visibility = Visibility.Visible;
            about_cockroach.Visibility = Visibility.Visible;

            _playerSpeed = _myPlace = 0;
            _isSpeed = _isTapok = _isGaming = false;
            _countkill = true;

            image2.Source = new BitmapImage(new Uri(@"/assets/turbo_disabled.png", UriKind.Relative));
            image3.Source = new BitmapImage(new Uri(@"/assets/tapok_disabled.png", UriKind.Relative));

        }

        private void GameOver()
        {
            Cursor = Cursors.Arrow;
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
                if (_totalScore >= 50)
                {
                    _totalScore -= 50;
                    _playerSpeed = 0.5f;
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

                for (int i = 0; i < _img.Length; i++)
                {
                    if (_name == _img[i].Name)
                    {
                        if (_myCockroach == i)
                        {
                            _cheak = false;
                        }
                        else
                        {
                            cts[i]?.Cancel();
                        }
                        break;
                    }
                }

                if (_cheak)
                {
                    Image srcImage = e.Source as Image;
                    srcImage.Source = new BitmapImage(new Uri(@"/assets/cockroach_die.png", UriKind.Relative));
                    _isTapok = _countkill = false;
                    Cursor = Cursors.Arrow;
                }
            }

            if (!_isGaming)
            {          
                FrameworkElement b = (FrameworkElement)sender;
                string _name = b.Name;

                for (int i = 0; i < _img.Length; i++)
                {
                    if (b == _img[i])
                    {
                        _myCockroach = i;
                        about_cockroach.Content = _aboutCockroach[i];

                        //Обозначение выбранного таракана
                        Thickness margin = ring.Margin;
                        margin.Top = _img[i].Margin.Top + 4;
                        ring.Margin = margin;

                        break;
                    }
                }
            }
        }
    }
}
