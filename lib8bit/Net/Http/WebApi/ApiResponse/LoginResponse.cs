using LitJson;
using System;
using System.Collections.Generic;
using System.Text;

namespace EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse
{
    public class LoginResponse : IWebApiResponse
    {
        public string authToken;
        public int userId;
        public string name;

        public void FromJsonData(JsonData json)
        {
            this.authToken = (string)json["authToken"];
            this.userId = (int)json["userId"];
            this.name = (string)json["name"];
        }
    }
}
