namespace Tester.Database
{
    public class TestParams
    {
        public TestParams(int scan_type, string barcode, int st_id, int client_id)
        {
            this.scan_type = scan_type;
            this.barcode = barcode;
            this.st_id = st_id;
            this.client_id = client_id;
        }
        public virtual int scan_type { get; set; }
        public virtual string barcode { get; set; }
        public virtual int st_id { get; set; }
        public virtual int client_id { get; set; }
    }

}
