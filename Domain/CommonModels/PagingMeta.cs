namespace Domain.CommonModels
{
    public class PagingMeta
    {
        public PagingMeta(Meta meta, data data)
        {
            Meta = meta;
            this.data = data;
        }

        public PagingMeta(Meta meta)
        {
            Meta = meta;
        }

        public Meta Meta { get; set; }
        public data data { get; set; }
    }
    public class data
    {
        public data(int? pageNo, bool nextPageAvailable, object list)
        {
            this.pageNo = pageNo;
            this.list = list;
            this.nextPageAvailable = nextPageAvailable;
        }
        public data(int? pageNo, bool nextPageAvailable)
        {
            this.pageNo = pageNo;
            this.nextPageAvailable = nextPageAvailable;
        }

        public data(int? pageNo, int noOfData, bool nextPageAvailable, object list)
        {
            this.pageNo = pageNo;
            this.noOfData = noOfData;
            this.nextPageAvailable = nextPageAvailable;
            this.list = list;
        }

        public int? pageNo { get; set; }
        public int? noOfData { get; set; }
        public bool nextPageAvailable { get; set; }
        public object? list { get; set; }
    }
}
