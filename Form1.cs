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
            storage.observer += System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
    }
    public class CCircle
        {
            private Rectangle rect;
            private int x;
            private int y;
            private int r = 40;

            Pen defaultPen = new Pen(Color.Blue, 5);
            Pen focusedPen = new Pen(Color.Violet, 5);

            public void paint(Graphics ellipses)
            {
                ellipses.DrawEllipse(focusedPen, rect);   
            }

            public void clear(Graphics ellipses)
            {
                ellipses.Dispose();
            }
            public CCircle(int x, int y)
            {
                this.x = x - r;
                this.y = y - r;
                rect = new Rectangle(this.x, this.y, r * 2, r * 2);
         
            }


        }

        public class MyStorage
        {
            private CCircle[] storage;
            protected int iter;
            protected int size;
            protected int count;

            public PaintEventHandler observer;

            public int getCount()
            {
                return count;
            }
            
            public void paint(Graphics ellipses)
            {
                foreach (CCircle circle in storage)
                    circle.paint(ellipses);
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


                observer.Invoke(this, null);
            }
            public MyStorage()
            {
                iter = 0;
                count = 0;
                size = 1;
                storage = new CCircle[size];
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            storage.add(new CCircle(Cursor.Position.X, Cursor.Position.Y), ellipses);

            //myPen.Dispose();                  // Dispose() - освобождение ресурсов
            //formGraphics.Dispose();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (storage.getCount() != 0)
                storage.paint(ellipses);
        }
    }
}
