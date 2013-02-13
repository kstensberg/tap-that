using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse
{
    public class ErrorResponse : IWebApiResponse
    {
        public string msg = null;
        public string displayError = null;

        public void FromJsonData(JsonData json)
        {
            if (json["msg"] != null)
                this.msg = (string)json["msg"];

            if (json["displayError"] != null)
                this.displayError = (string)json["displayError"];
        }
    }
}
