using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace mySnake
{
    public partial class Window : Form
    {
        List<Shape> _snake = new List<Shape>();
        Shape food = new Circle();

        Shape _mySnake;

        Shape _bonnusFood;
        int _maxHeight, _maxWidth, _score, _highScore, _squareTick,
            mainTimer, _circleTick, _isBonnus, _spCircleTick, _godModeTick;
        bool _goLeft, _goRight, _goUp, _goDown, _isGodMode;
        Random rand = new Random();

        private void ElementTick(object sender, EventArgs e)
        {
            _squareTick++;
            _circleTick++;


            if (progressBar.Value == 100)
                _spCircleTick++;

            if (_squareTick == 10)
                _bonnusFood = new Square { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };
            if (_circleTick == 21)
                _bonnusFood = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };
            if (_spCircleTick == 10)
                _bonnusFood = new SpecialCircle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };

            if (_isGodMode)
            {
                _godModeTick--;
                progressBar.Value = 0;
                godModeLabel.Text = "GOD MODE: " + _godModeTick;

                if (_godModeTick == 0)
                {
                    godModeLabel.Visible = false;
                    _isGodMode = false;
                    _isBonnus = 0;
                    _godModeTick = 11;
                }
            }


            if (_squareTick == 20)
            {

                _bonnusFood.X = -1;
                _bonnusFood.Y = -1;
                orangeLabel.Visible = false;
                _squareTick = 0;
            }

            if (_circleTick == 35)
            {
                _bonnusFood.X = -1;
                _bonnusFood.Y = -1;
                grapeLabel.Visible = false;
                _circleTick = 0;
            }

            if (_spCircleTick == 25)
            {
                _bonnusFood.X = -1;
                _bonnusFood.Y = -1;
                grapeLabel.Visible = false;
                _spCircleTick = 0;
            }

            mainPicBox.Invalidate();
        }


        public Window()
        {
            InitializeComponent();
            new Settings();
            this._score = 0;
            this._highScore = 0;
            this._squareTick = 0;
            this.mainTimer = 0;
            this._isBonnus = 0;
            this._circleTick = 0;
            this._spCircleTick = 0;
            this._godModeTick = 11;
            this._isGodMode = false;
        }

        private void loadButton(object sender, EventArgs e)
        {
            if (mainTimer == 0)
                loadGame();
        }

        private void saveButton(object sender, EventArgs e)
        {
            if (mainTimer != 0 && !_isGodMode)
                saveGame();
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left && Settings._directions != "right")
                this._goLeft = true;
            if (e.KeyCode == Keys.Right && Settings._directions != "left")
                this._goRight = true;
            if (e.KeyCode == Keys.Down && Settings._directions != "up")
                this._goDown = true;
            if (e.KeyCode == Keys.Up && Settings._directions != "down")
                this._goUp = true;
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
                this._goLeft = false;
            if (e.KeyCode == Keys.Right)
                this._goRight = false;
            if (e.KeyCode == Keys.Down)
                this._goDown = false;
            if (e.KeyCode == Keys.Up)
                this._goUp = false;
        }

        private void gameTimerTick(object sender, EventArgs e)
        {
            mainTimer++;

            if (_goLeft)
                Settings._directions = "left";
            if (_goRight)
                Settings._directions = "right";
            if (_goUp)
                Settings._directions = "up";
            if (_goDown)
                Settings._directions = "down";

            for (int i = _snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings._directions)
                    {
                        case "left":
                            _snake[i].X--;
                            break;
                        case "right":
                            _snake[i].X++;
                            break;
                        case "down":
                            _snake[i].Y++;
                            break;
                        case "up":
                            _snake[i].Y--;
                            break;
                    }

                    if (_snake[i].X < 0)
                        _snake[i].X = _maxWidth;

                    if (_snake[i].X > _maxWidth)
                        _snake[i].X = 0;

                    if (_snake[i].Y < 0)
                        _snake[i].Y = _maxHeight;

                    if (_snake[i].Y > _maxHeight)
                        _snake[i].Y = 0;

                    if (_snake[i].X == food.X && _snake[i].Y == food.Y)
                        Eat();


                    if (_snake[i].X == _bonnusFood.X && _snake[i].Y == _bonnusFood.Y)
                    {
                        if (!_isGodMode)
                        {
                            switch (_bonnusFood._idProp)
                            {
                                case 2:
                                    _circleTick = 0;
                                    _isBonnus = 2;
                                    if (progressBar.Value != 100)
                                        progressBar.Value += 20;
                                    grapeLabel.Visible = false;
                                    break;
                                case 1:
                                    _squareTick = 0;
                                    _isBonnus = 1;
                                    orangeLabel.Visible = false;
                                    break;
                                case 3:
                                    _spCircleTick = 0;
                                    _isBonnus = 3;
                                    break;
                            }
                        }


                        if (_bonnusFood._idProp != 0)
                        {
                            _bonnusFood.eatFood(ref _score, rand, _maxHeight, _maxWidth);
                            score.Text = "Score: " + _score;
                        }
                    }

                    if (!_isGodMode)
                    {
                        for (int j = 1; j < _snake.Count; j++)
                        {

                            if (_snake[i].X == _snake[j].X && _snake[i].Y == _snake[j].Y)
                                gameOver();

                        }
                    }
                }

                else
                {
                    _snake[i].X = _snake[i - 1].X;
                    _snake[i].Y = _snake[i - 1].Y;
                }


            }

            mainPicBox.Invalidate();
        }




        private void startButtonClick(object sender, EventArgs e)
        {
            startGame();
        }

        private void snapButtonClick(object sender, EventArgs e)
        {

        }

        private void mainPicBoxGraphics(object sender, PaintEventArgs e)
        {


            Graphics g = e.Graphics;

            Brush snakeColor;

            if (_isBonnus == 0)
            {
                for (int i = 0; i < _snake.Count; i++)
                {
                    if (i == 0)
                        snakeColor = Brushes.Black;
                    else
                        snakeColor = Brushes.Yellow;

                    g.FillEllipse(snakeColor, new Rectangle(_snake[i].X * Settings.Width,
                        _snake[i].Y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
            }

            g.FillEllipse(Brushes.Red, new Rectangle(food.X * Settings.Width,
                food.Y * Settings.Height,
                Settings.Width,
                Settings.Height));


            switch (_isBonnus)
            {
                case 1:
                    if (!_isGodMode)
                    {
                        _mySnake = new Square();
                        _mySnake.drawSnake(ref _snake, g);
                    }
                    break;
                case 2:
                    if (!_isGodMode)
                    {
                        _mySnake = new Circle();
                        _mySnake.drawSnake(ref _snake, g);
                    }
                    break;
                case 3:
                    _mySnake = new SpecialCircle();
                    _mySnake.drawSnake(ref _snake, g);
                    this._isGodMode = true;
                    _spCircleTick = 0;
                    godModeLabel.Visible = true;
                    break;
            }




            if (mainTimer % 1000 == 0)
                gameTimer.Interval -= 5;


            if (_squareTick >= 10 && _squareTick <= 20)
            {
                _bonnusFood.drawFood(ref orangeLabel, g);
            }

            if (_circleTick >= 21 && _circleTick <= 35)
            {

                _bonnusFood.drawFood(ref grapeLabel, g);

            }


            if (_spCircleTick >= 10 && _circleTick <= 25 && _mySnake.godModeProp == false)
            {
                _bonnusFood.drawFood(ref grapeLabel, g);

            }
        }

        private void startGame()
        {
            this._maxWidth = mainPicBox.Width / Settings.Width - 1;
            this._maxHeight = mainPicBox.Height / Settings.Height - 1;
            this._snake = new List<Shape>();
            this._snake.Clear();
            this.startButton.Enabled = false;
            this.snapButton.Enabled = false;

            this.score.Text = "Score: " + _score;
            highScore.Text = "High Score: " + _highScore;
            Shape head = new Circle { X = 10, Y = 5 };
            this._snake.Add(head);

            for (int i = 0; i < 10; i++)
            {
                Shape body = new Circle();

                this._snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };
            _bonnusFood = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };



            gameTimer.Start();
            elementsTimer.Start();
        }
        private void Eat()
        {
            this._score += 10;
            score.Text = "Score: " + _score;
            Shape body = new Circle()
            {
                X = _snake[_snake.Count - 1].X,
                Y = _snake[_snake.Count - 1].Y
            };

            _snake.Add(body);


            food = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };
        }

        private void RefreshGame()
        {

            this._maxWidth = mainPicBox.Width / Settings.Width - 1;
            this._maxHeight = mainPicBox.Height / Settings.Height - 1;
            this.startButton.Enabled = false;
            this.snapButton.Enabled = false;

            this.score.Text = "Score: " + _score;
            highScore.Text = "High Score: " + _highScore;

            food = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };
            _bonnusFood = new Circle { X = rand.Next(2, _maxWidth), Y = rand.Next(2, _maxHeight) };

            gameTimer.Start();
            elementsTimer.Start();

        }
        private void gameOver()
        {
            gameTimer.Stop();
            elementsTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;
            gameTimer.Interval = 80;

            if (_score > _highScore)
            {
                _highScore = _score;
                highScore.Text = "High Score: " + _highScore;
                highScore.ForeColor = Color.Blue;
            }
            this.progressBar.Value = 0;
            this._squareTick = 0;
            this._circleTick = 0;
            this._score = 0;
            this._isBonnus = 0;
        }


        // Serialization

        private void saveGame()
        {
            gameTimer.Stop();
            elementsTimer.Stop();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Directory.GetCurrentDirectory();
            saveFile.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                IFormatter format = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFile.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    format.Serialize(stream, _snake);
                    format.Serialize(stream, _bonnusFood);
                    format.Serialize(stream, _score);
                    format.Serialize(stream, _highScore);
                    format.Serialize(stream, _isBonnus);
                }
            }

            this.Close();
        }
        private void loadGame()
        {
            OpenFileDialog loadFile = new OpenFileDialog();
            loadFile.InitialDirectory = Directory.GetCurrentDirectory();
            loadFile.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            loadFile.FilterIndex = 1;
            loadFile.RestoreDirectory = true;
            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(loadFile.FileName, FileMode.Open);
                var binaryFormatter = new BinaryFormatter();

                _snake = (List<Shape>)binaryFormatter.Deserialize(stream);
                _mySnake = (Shape)binaryFormatter.Deserialize(stream);
                _score = (int)binaryFormatter.Deserialize(stream);
                _highScore = (int)binaryFormatter.Deserialize(stream);
                _isBonnus = (int)binaryFormatter.Deserialize(stream);
                RefreshGame();

                mainPicBox.Invalidate();

            }
            gameTimer.Start();
        }

    }
}