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

        public class Obj
        {
            public virtual void someMethod()
            {

            }
            public virtual void name()
            {

            }

        }

        public class MyStorage {
            private Obj[] storage;
            protected int iter;
            protected int size;
            protected int count;

            MyStorage()
            {
                iter = 0;
                count = 0;
                size = 1;
                for (int i = 0; i < size; i++)
                    storage[i] = null;
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
