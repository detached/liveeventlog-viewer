using LogViewerObjects;

namespace LogViewerDaemon
{
    public delegate string GetInputDelegate(MSMQMessage e);
    public delegate bool FilterDelegate(string input, string reference);

    public class Filter
    {
        private FilterDelegate filterDelegate;
        private GetInputDelegate getInput;
        private string reference;

        public Filter(GetInputDelegate i, FilterDelegate f, string r) {
            this.getInput = i;
            this.filterDelegate = f;
            this.reference = r;
        }

        public bool doFilter(MSMQMessage e) 
        {
            return this.filterDelegate.Invoke(this.getInput.Invoke(e), this.reference);
        }
    }
}
