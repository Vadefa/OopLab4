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
            private bool is_focused;

            Pen defaultPen = new Pen(Color.Blue, 5);
            Pen focusedPen = new Pen(Color.Violet, 5);

            public void paint(Graphics ellipses)
            {
                if (this.is_focused == true)
                    ellipses.DrawEllipse(focusedPen, rect);
                else
                    ellipses.DrawEllipse(defaultPen, rect);
            }

            public void clear(Graphics ellipses)
            {
                ellipses.Dispose();
            
            }

            public void focus()
            {
                is_focused = true;
                ActiveForm.Invalidate();
            }
            public void unfocus()
            {
                is_focused = false;
                ActiveForm.Invalidate();
            }
            public CCircle(int x, int y, Graphics ellipses)
            {
                this.x = x - r;
                this.y = y - r;
                is_focused = true;
                rect = new Rectangle(this.x, this.y, r * 2, r * 2);

                ellipses.DrawEllipse(focusedPen, rect);
            }


        }

        public class MyStorage
        {
            private CCircle[] storage;
            protected int iter;
            protected int size;
            protected int count;

            public void paint(Graphics ellipses)
            {
                foreach (CCircle circle in storage)
                    circle.paint(ellipses);
            }
            public int getCount()
            {
                return count;
            }
            private void shift()
            {
                CCircle[] tempStorage = new CCircle[size - iter + 1];      // we putting an element after the storage[iter] element
                for (int i = iter + 1; i < size; i++)
                    tempStorage[i - iter - 1] = storage[i];

                sizeImprove();
                storage[iter + 1] = null;                            // later we'll put a new element here

                for (int i = iter + 2; i < size; i++)
                    storage[i] = tempStorage[i - iter - 2];

            }

            private void sizeImprove()
            {
                CCircle[] tempStorage = storage;


                size = size + 1;

                storage = new CCircle[size];

                for (int i = 0; i < size - 1; i++)
                    storage[i] = tempStorage[i];

                storage[size - 1] = null;

            }

            public void add(CCircle circle, Graphics ellipses)
            {
                if (count != 0)
                    foreach (CCircle c in storage)
                        c.unfocus();

                if (iter < size)
                {
                    if (storage[iter] == null)
                    {
                        storage[iter] = circle;
                        iter = iter + 1;
                    }
                    else
                    {
                        shift();
                        iter = iter + 1;
                        storage[iter] = circle;
                    }
                }
                else if (iter == size)
                {
                    sizeImprove();
                    storage[iter] = circle;
                    iter = iter + 1;
                }
                count = count + 1;
            }
            public MyStorage()
            {
                iter = 0;
                count = 0;
                size = 1;
                storage = new CCircle[size];
            }
        }

        private void GetMousePosition(object sender, MouseEventArgs e)
        {

        }
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            storage.add(new CCircle(Cursor.Position.X, Cursor.Position.Y, ellipses), ellipses);

            int p = MousePosition.X;
            int t = Cursor.Position.X;


        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (storage.getCount() != 0)
                storage.paint(ellipses);

        }
    }
}
