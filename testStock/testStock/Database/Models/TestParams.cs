namespace Tester.Database
{
    public class Params
    {
        public Params(int client_id, int st_id, string barcode)
        {
            this.client_id = client_id;
            this.st_id = st_id;
            this.barcode = barcode;
        }

        public virtual int client_id { get; set; }
        public virtual int st_id { get; set; }
        public virtual string barcode { get; set; }
    }

}
