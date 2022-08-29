using System;
using System.Drawing;
using System.Collections.Generic;

namespace mySnake
{
    [Serializable]
    public abstract class Shape
    {
        protected int _id;
        protected bool _godModeFlag;

        public bool godModeProp { get { return this._godModeFlag; } }
        public int _idProp
        {
            get { return this._id; }
        }
        public int X { get; set; }

        public int Y { get; set; }

        public int MaxTime { get; set; }
        public int MinTime { get; set; }

        public Shape()
        {
            this.X = 0;
            this.Y = 0;
            MinTime = 0;
            MaxTime = 0;
            _id = 0;
            _godModeFlag = false;
        }

        abstract public void eatFood(ref int score, Random rand, int maxHeight, int maxWidth);
        abstract public void drawFood(ref System.Windows.Forms.Label label, Graphics g);
        abstract public void drawSnake(ref List<Shape> s, Graphics g);
        abstract public void makeSound();
    }
}
