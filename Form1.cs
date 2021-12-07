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
        Graphics ellipses2;                        // An additional environment for painting
        bool environment;                          // False - > environment of drawing is ellipses. True -> ellipses2.
        public Form1()
        {
            InitializeComponent();

            ellipses = CreateGraphics();
            ellipses2 = CreateGraphics();
            environment = false;
            storage = new MyStorage();
    }


    public class CCircle
        {
            private Rectangle rect;
            private int x;
            private int y;
            private int r = 40;
            private bool is_focused;

            Pen defaultPen = new Pen(Color.Blue, 6);
            Pen focusedPen = new Pen(Color.Violet, 6);

            public bool checkUnderMouse(Graphics ellipses, int x_mouse, int y_mouse)
            {
                int x0 = x;
                int y0 = y;

                int x1 = x + r * 2 + ((int)defaultPen.Width);
                int y1 = y + r * 2 + ((int)defaultPen.Width);

                if ((x_mouse > x0) && (x_mouse < x1) && (y_mouse > y0) && (y_mouse < y1))
                    return true;
                else
                    return false;
            }

            public void paint(Graphics ellipses)
            {
                if (this.is_focused == true)
                    ellipses.DrawEllipse(focusedPen, rect);
                else
                    ellipses.DrawEllipse(defaultPen, rect);
            }

            public bool focusCheck()
            {
                if (is_focused == true)
                    return true;
                else
                    return false;
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
                this.x = x - r - ((int)focusedPen.Width / 2);
                this.y = y - r - ((int)focusedPen.Width / 2);
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

            public void removeFocused(Graphics deleting, Graphics inserting)
            {
                int del = 0;                                // number of elements we'll delete
                for (int i = 0; i < size; i++)              
                {
                    if (storage[i].focusCheck() != true)
                        del = del + 1;
                    else
                        storage[i] = null;                  // placing null in the storage items for the deleting elements
                }

                CCircle[] tempStorage = new CCircle[del];   // here we'll put elements that should remain


                int j = 0;
                for (int i = 0; i < size; i++)
                    if(storage[i] != null)
                    {
                        tempStorage[j] = storage[i];        // put remaining elements
                        j = j + 1;
                    }

                count = size - (size - del);
                size = del;
                iter = size - 1;
                if (iter < 0)
                    iter = 0;

                storage = new CCircle[size];
                for (int i = 0; i < size; i++)
                    storage[i] = tempStorage[i];            // moved all remained elements

                deleting.Dispose();                         // now our previous elements will not be repainted

                for (int i = 0; i < size; i++)
                    storage[i].paint(inserting);

                ActiveForm.Invalidate();
            }
            public void focusOnClick(Graphics ellipses, int x_mouse, int y_mouse)
            {

                int i = size;
                bool found = false;
                while ((found == false) && (i > 0))
                {
                    i = i - 1;
                    found = storage[i].checkUnderMouse(ellipses, x_mouse, y_mouse);
                }
                
                if (found == true)
                {
                    foreach (CCircle circle in storage)
                        circle.unfocus();

                    storage[i].focus();
                }

            }
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

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            //PointToClient returns mouse position in relation to the form, not to the screen
            Point mousePos = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));

            if (environment == false)
                storage.add(new CCircle(mousePos.X, mousePos.Y, ellipses), ellipses);
            else
                storage.add(new CCircle(mousePos.X, mousePos.Y, ellipses2), ellipses2);

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (storage.getCount() != 0)
                if (environment == false)
                    storage.paint(ellipses);
                else
                    storage.paint(ellipses2);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (storage.getCount() != 0)
            {
                Point mousePos = PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y));

                if (environment == false)
                    storage.focusOnClick(ellipses, mousePos.X, mousePos.Y);
                else
                    storage.focusOnClick(ellipses2, mousePos.X, mousePos.Y);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (environment == false)
                {
                    storage.removeFocused(ellipses, ellipses2);
                    ellipses = CreateGraphics();
                }
                else
                {
                    storage.removeFocused(ellipses2, ellipses);
                    ellipses2 = CreateGraphics();
                }
                if (environment == false)
                    environment = true;
                else
                    environment = false;
            }
        }
    }
}
