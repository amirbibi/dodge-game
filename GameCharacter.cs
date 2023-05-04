using System;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace DodgeGame.Shared
{
    [XmlInclude(typeof(Enemy))]
    [XmlInclude(typeof(Player))]
    public class GameCharacter
    {
        protected Canvas canvas;
        protected Rectangle character;
        protected Random rand = new Random();
        protected string path = Path.Combine(Directory.GetCurrentDirectory(), "images");
        public int deserializedX;
        public int deserializedY;
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Image { get; set; }

        public GameCharacter()
        {
        }

        public GameCharacter(Canvas canvas)
        {
            this.canvas = canvas;
        }

        protected virtual void Draw()
        {
        }
    }
}