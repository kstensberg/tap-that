using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse
{
    public interface IWebApiResponse
    {
        void FromJsonData(JsonData jsonData);
    }
}
