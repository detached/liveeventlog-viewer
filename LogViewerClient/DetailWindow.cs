using LogViewerObjects;
using System;
using System.Windows.Forms;

namespace LogViewerClient
{
    public partial class DetailWindow : Form, ISubject
    {
        private LogViewerClient client;
        private ListViewItemComparer comparer;
        private int sortColumn;

        public DetailWindow()
        {
            InitializeComponent();
            this.comparer = new ListViewItemComparer();
            this.view.Columns.Add("Index");
            this.view.Columns.Add("ID");
            this.view.Columns.Add("Source");
            this.view.Columns.Add("Type");
            this.view.Columns.Add("Categorynumber");
            this.view.Columns.Add("Category");
            this.view.Columns.Add("Machine");
            this.view.Columns.Add("User");
            this.view.Columns.Add("Generated");
            this.view.Columns.Add("Written");
            this.view.ListViewItemSorter = this.comparer;
            this.sortColumn = 0;
        }

        public void SetClient(LogViewerClient c)
        {
            this.client = c;
        }

        public void IncomingLog(object sender, MSMQMessage e)
        {
            ListViewItem item = new ListViewItem(new string[] 
            {
                e.Index.ToString(),
                e.InstanceId.ToString(),
                e.Source,
                e.EntryType.ToString(),
                e.CategoryNumber.ToString(),
                e.Category,
                e.MachineName,
                e.UserName,
                e.TimeGenerated.ToString(),
                e.TimeWritten.ToString(),
            });
            item.Tag = e.Index;
            item.Name = e.Index.ToString();

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    this.view.Items.Add(item);
                    this.view.Refresh();
                }));
            }
            else {
                this.view.Items.Add(item);
                this.view.Refresh();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private void view_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.view.SelectedItems.Count != 0){
                var index = (int)this.view.SelectedItems[0].Tag;
                var msg = this.client.GetMessages(index);
                this.text.Text = msg.Message;
            }
        }

        private void view_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != this.sortColumn)
            {
                this.sortColumn = e.Column;
                this.view.Sorting = SortOrder.Ascending;
            }
            else 
            {
                if (this.view.Sorting == SortOrder.Ascending)
                {
                    this.view.Sorting = SortOrder.Descending;
                }
                else 
                {
                    this.view.Sorting = SortOrder.Ascending;
                }
            }

            this.comparer.SetColumn(e.Column, this.view.Sorting);
            this.view.Sort();
        }

        internal void ShowMessage(int p)
        {
           var index = p.ToString();
           if (this.view.Items.ContainsKey(index))
           {
               this.view.FocusedItem = this.view.Items[index];
               this.view.Items[index].Selected = true;
           }
        }

        private void view_VisibleChanged(object sender, EventArgs e)
        {
            this.view.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
