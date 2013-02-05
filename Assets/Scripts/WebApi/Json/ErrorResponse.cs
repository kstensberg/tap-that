using System;
using LitJson;

namespace EightBitIdeas.WebApi.Json
{
	public class ErrorResponse
	{
		public string msg = null;
		public string displayError = null;
		
		public ErrorResponse(JsonData json)
		{
			if (json["msg"] != null)
				this.msg = (string)json["msg"];
			
			if (json["displayError"] != null)
				this.displayError = (string)json["displayError"];
		}
	}
}

