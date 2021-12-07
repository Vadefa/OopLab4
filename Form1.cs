using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OopLab4
{
    public partial class Form1 : Form
    {
     MyStorage storage;
     Graphics ellipses;                         // Graphics класс предоставляет методы для рисования объектов
        public Form1()
        {
            InitializeComponent();

            ellipses = CreateGraphics();

            storage = new MyStorage();
    }
    public class CCircle
        {
            private Rectangle rect;
            private int x;
            private int y;
            private int r = 40;

            public CCircle(int x, int y, Graphics ellipses, MyStorage storage)
            {

                Pen myPen = new Pen(Color.Aquamarine);
                myPen.Width = 5;

                this.x = x - r;
                this.y = y - r - ((int)myPen.Width);

                rect = new Rectangle(this.x, this.y, r * 2, r * 2);

                ellipses.DrawRectangle(myPen, rect);
                ellipses.DrawEllipse(myPen, rect);

                storage.add(rect);
                
            }


        }
        public class MyStorage
        {
            private object[] storage;
            protected int iter;
            protected int size;
            protected int count;

            private void shift()
            {
                object[] tempStorage = new object[size - iter + 1];      // we putting an element after the storage[iter] element
                for (int i = iter + 1; i < size; i++)
                    tempStorage[i - iter - 1] = storage[i];

                sizeImprove();
                storage[iter + 1] = null;                            // later we'll put a new element here

                for (int i = iter + 2; i < size; i++)
                    storage[i] = tempStorage[i - iter - 2];

            }

            private void sizeImprove()
            {
                object[] tempStorage = storage;


                size = size + 1;

                storage = new object[size];

                for (int i = 0; i < size - 1; i++)
                    storage[i] = tempStorage[i];

                storage[size - 1] = null;

            }
            public void add(object obj)
            {
                if (iter < size)
                {
                    if (storage[iter] == null)
                    {
                        storage[iter] = obj;
                        iter = iter + 1;
                    }
                    else
                    {
                        shift();
                        iter = iter + 1;
                        storage[iter] = obj;
                    }
                }
                else if (iter == size)
                {
                    sizeImprove();
                    storage[iter] = obj;
                    iter = iter + 1;
                }
                count = count + 1;
                
            }
            
            public void paint()
            {
                foreach(object a in storage)
                {
                    //a.GetType()
                }
            }
            public MyStorage()
            {
                iter = 0;
                count = 0;
                size = 1;
                storage = new object[size];
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            CCircle circle = new CCircle(Cursor.Position.X, Cursor.Position.Y, ellipses, storage);

            //myPen.Dispose();                  // Dispose() - освобождение ресурсов
            //formGraphics.Dispose();
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

            CCircle circle = new CCircle(Cursor.Position.X, Cursor.Position.Y, ellipses, storage);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
