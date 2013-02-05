using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using EightBitIdeas.WebApi.Json;

namespace EightBitIdeas.WebApi
{
	public class WebApi
	{
		public static readonly string ApiRootUrl = "http://8-bitideas.com/api/";
		
		public static string AuthUrl
		{
			get
			{
				return ApiRootUrl + "user/auth";
			}
		}
		
		public ErrorResponse GetError(WWW www)
		{
			if (!IsError(www))
				return null;
			else
				return new ErrorResponse(JsonMapper.ToObject(www.text));
		}
		
		public LoginResponse GetLoginResponse(WWW www)
		{
			if (IsError(www))
				return null;
			else
				return new LoginResponse(JsonMapper.ToObject(www.text));
		}
		
		public WWW GetAuthWWW(string username, string password)
		{
			WWWForm form = new WWWForm();
			form.AddField("username", username);
			form.AddField("password", password);
			
			return new WWW(AuthUrl, form);
		}
		
		public int GetStatusCode(WWW www)
		{
			string statusString = www.responseHeaders["STATUS"];
			
			string[] statusSplit = statusString.Split(' ');
			
			if (statusSplit.Length != 3)
				return 0;
			else
				return Convert.ToInt32(statusSplit[1]);
		}
		
		private bool IsError(WWW www)
		{
			if (www.responseHeaders.ContainsKey("ERROR-STATE"))
				return StringToBool(www.responseHeaders["ERROR-STATE"]);
			
			return false;
		}
		
		private bool StringToBool(string toParse)
		{
			switch(toParse.ToUpper())
			{
				case "YES":
				case "TRUE":
				case "1":
				case "Y":
				case "T":
					return true;
				default:
					return false;
			}
		}
	}
}