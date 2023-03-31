using SnakeGameWPF.GameLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace SnakeGameWPF
{
    public class Score
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ScoreDataModel
    {
        public List<Score> DataRows { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScoreDataModel dataContext = new ScoreDataModel();
        private GameState _gameState;
        private static readonly int PixelSize = 16;

        public MainWindow()
        {
            InitializeComponent();

            dataContext.DataRows = new List<Score>();
            LoadSettings();
            DataContext = dataContext;

            _gameState = new GameState();
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Tick += new EventHandler(RenderTick);
            timer.Interval = TimeSpan.FromMilliseconds(1000 / _gameState.Speed);
            timer.Start();

            this.KeyDown += new KeyEventHandler(HandleKeyPress);

            StartGame();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _gameState.Snake.Direction = Directions.Up; break;
                case Key.Down:
                    _gameState.Snake.Direction = Directions.Down; break;
                case Key.Left:
                    _gameState.Snake.Direction = Directions.Left; break;
                case Key.Right:
                    _gameState.Snake.Direction = Directions.Right; break;
                case Key.Enter:
                    if (_gameState.GameOver)
                        StartGame();
                    break;
            }
        }

        private void RenderTick(object sender, EventArgs e)
        {
            if (_gameState.GameOver)
            {
                labelGameOver.Content = "Game Over\nFinal Score is " + _gameState.Score + "\nPress Enter to restart\n";
                labelGameOver.Visibility = Visibility.Visible;
            }
            else
            {
                _gameState.Update();
                pbCanvas.Children.Clear();
                labelScoreValue.Content = _gameState.Score.ToString();
                for (int i = 0; i < _gameState.Snake.Body.Count; ++i)
                {
                    Rectangle BodyPart = new Rectangle
                    {
                        Fill = (i == 0) ? Brushes.Black : Brushes.Green,
                        Width = PixelSize - 1,
                        Height = PixelSize - 1,
                        StrokeThickness = 2,
                    };
                    Canvas.SetTop(BodyPart, _gameState.Snake.Body[i].X * PixelSize);
                    Canvas.SetLeft(BodyPart, _gameState.Snake.Body[i].Y * PixelSize);
                    pbCanvas.Children.Add(BodyPart);
                }
                Rectangle Apple = new Rectangle
                {
                    Fill = Brushes.Red,
                    Width = PixelSize - 1,
                    Height = PixelSize - 1
                };
                Canvas.SetTop(Apple, _gameState.Apple.X * PixelSize);
                Canvas.SetLeft(Apple, _gameState.Apple.Y * PixelSize);
                pbCanvas.Children.Add(Apple);
            }
        }

        private void StartGame()
        {
            _gameState = new GameState();
            labelScoreValue.Content = _gameState.Score.ToString();
            labelGameOver.Visibility = Visibility.Hidden;
        }

        private void LoadSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScoreDataModel));
            using (FileStream fs = new FileStream("scores.xml", FileMode.Open))
                dataContext = (ScoreDataModel)serializer.Deserialize(fs);
        }

        private void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScoreDataModel));
            using (FileStream fs = new FileStream("scores.xml", FileMode.Create))
                serializer.Serialize(fs, dataContext);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
