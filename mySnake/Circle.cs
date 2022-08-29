using System;
using System.Drawing;
using System.Collections.Generic;
using System.Media;

namespace mySnake
{
    [Serializable]
    class Circle : Shape
    {

        public Circle()
        {
            MinTime = 21;
            MaxTime = 35;
            _id = 2;
        }

        public override void drawFood(ref System.Windows.Forms.Label label, Graphics g)
        {
            g.FillEllipse(System.Drawing.Brushes.DarkViolet, new Rectangle(X * Settings.Width,
            Y * Settings.Height,
            Settings.Width,
            Settings.Height));
            label.Visible = true;
        }

        public override void eatFood(ref int score, Random rand, int maxHeight, int maxWidth)
        {

            score += 50;
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
                    g.FillEllipse(System.Drawing.Brushes.AntiqueWhite, new Rectangle(s[i].X * Settings.Width,
                        s[i].Y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
                else
                {
                    g.FillEllipse(System.Drawing.Brushes.DarkViolet, new Rectangle(s[i].X * Settings.Width,
                        s[i].Y * Settings.Height,
                        Settings.Width,
                        Settings.Height));
                }
            }
        }

        public override void makeSound()
        {
            SoundPlayer gameSound = new SoundPlayer(Properties.Resources.circle);
            gameSound.Play();
        }
    }
}
