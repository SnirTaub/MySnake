using System;
using System.Drawing;
using System.Media;
using System.Collections.Generic;


namespace mySnake
{
    [Serializable]
    class Square : Shape
    {
        public Square()
        {
            MinTime = 10;
            MaxTime = 20;
            _id = 1;
        }
        public override void drawFood(ref System.Windows.Forms.Label label, Graphics g)
        {
            g.FillRectangle(System.Drawing.Brushes.Orange, new Rectangle(X * Settings.Width,
            Y * Settings.Height,
            Settings.Width,
            Settings.Height));
            label.Visible = true;
        }

        public override void eatFood(ref int score, Random rand, int maxHeight, int maxWidth)
        {
            score += 30;
            this.X = rand.Next(2, maxWidth);
            this.Y = rand.Next(2, maxHeight);
            this.makeSound();
        }

        
        public override void drawSnake(ref List<Shape> s, Graphics g)
        {

            for (int i = 0; i < s.Count; i++)
            {
                if (i == 0)
                {
                    g.FillRectangle(System.Drawing.Brushes.Black, new Rectangle(s[i].X * Settings.Width,
                        s[i].Y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
                else
                {
                    g.FillRectangle(System.Drawing.Brushes.Orange, new Rectangle(s[i].X * Settings.Width,
                        s[i].Y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
            }
        }

        public override void makeSound()
        {
            SoundPlayer gameSound = new SoundPlayer(Properties.Resources.square);
            gameSound.Play();
        }
    }
}
