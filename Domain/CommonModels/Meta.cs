using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.CommonModels
{
    public class Meta
    {
        public Meta(int status_code, string status_message)
        {
            this.status_code = status_code;
            this.status_message = status_message;
        }

        public int status_code { get; set; }
        public string status_message { get; set; }
    }
}
