using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CommonModels
{
    public class ResponseJson
    {
        public ResponseJson(Meta meta)
        {
            this.meta = meta;
        }

        public ResponseJson(Meta meta, object data1)
        {
            this.meta = meta;
            data = data1;
        }
        public int NumberOfdata { get; set; }
        public Meta meta { get; set; }

        public object? data { get; set; }
    }
}
