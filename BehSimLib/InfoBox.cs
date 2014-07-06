using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BehSimLib
{
    public class InfoBoxRow
    {
        public string Name;
        public object Value;

        public InfoBoxRow(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public void Update(float Value)
        {
            this.Value = Value;
        }
    }

    public class InfoBox
    {
        public static Texture2D tex;
        public static SpriteFont InfoBoxFont;

        Rectangle boxRectangle;
        Vector2 position;
        int width=0, height;
        List<InfoBoxRow> rows;

        public InfoBox()
        {
            rows = new List<InfoBoxRow>();
        }

        public static void Load(SpriteFont font, Texture2D texture)
        {
            tex = texture;
            InfoBoxFont = font;
        }

        public void Update(Vector2 position, List<InfoBoxRow> rowList)
        {
            rows = rowList;
            this.position = position;

            if (rows.Count == 0)
            {
                rows.Add(new InfoBoxRow("No data passed", 0));
            }
            
            //calculate box size
            height = rows.Count * (int)InfoBoxFont.MeasureString(rows[0].Name).Y + 50;


            boxRectangle = new Rectangle((int)position.X, (int)position.Y, width, height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //get and measure box info
            string text = "";
            foreach (var row in rows)
            {
                text += row.Name + ": " + row.Value.ToString() + "\n";
            }
            Vector2 textSize = InfoBoxFont.MeasureString(text);
            //adjust box size
            boxRectangle.Width = (int)textSize.X + 30;
            boxRectangle.Height = (int)textSize.Y + 10;
            //draw box
            spriteBatch.Draw(tex, boxRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            //write box info
            Vector2 textPos = position;
            textPos.X += 10;
            textPos.Y += 10;
            spriteBatch.DrawString(InfoBoxFont, text, textPos, Color.White);
        }
    }
}
