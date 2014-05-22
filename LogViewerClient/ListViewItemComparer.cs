using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewerClient
{
    class ListViewItemComparer: IComparer
    {
        protected int column;
        protected SortOrder order;

        public ListViewItemComparer() 
        {
            this.column = 0;
            this.order = SortOrder.Ascending;
        }

        public void SetColumn(int c, SortOrder o) 
        {
            this.column = c;
            this.order = o;
        }

        public int Compare(object x, object y)
        {
            int val = -1;

            if (this.column == 8 || this.column == 9)
            {
                val =
                DateTime.Compare(
                DateTime.Parse(((ListViewItem)x).SubItems[this.column].Text),
                DateTime.Parse(((ListViewItem)y).SubItems[this.column].Text));

            }
            else
            {
                val =
                    String.Compare(
                    ((ListViewItem)x).SubItems[this.column].Text,
                    ((ListViewItem)y).SubItems[this.column].Text);
            }

            if (this.order == SortOrder.Descending)
            {
                return val * -1;
            }

            return val;
        }
    }
}
