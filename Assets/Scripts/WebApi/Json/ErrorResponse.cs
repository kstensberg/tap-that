using System;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class ErrorResponse
	{
		public string msg;
		public string displayError;
		
		public ErrorResponse(JsonData json)
		{
			this.msg = (string)json["msg"];
			this.displayError = (string)json["displayError"];
		}
	}
}

