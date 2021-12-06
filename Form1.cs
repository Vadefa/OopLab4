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


 
        public class CCircle
        {

            private int x;
            private int y;
            private int r;
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
            public MyStorage()
            {
                iter = 0;
                count = 0;
                size = 1;
                storage = new object[size];
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
