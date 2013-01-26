using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EightBitIdeas.WebApi
{
	public class WebApi
	{
		public static readonly string AuthUrl = "http://8-bitideas.com/api/user/auth";
		
		public bool IsError(WWW www)
		{
			if (www.responseHeaders.ContainsKey("ERROR-STATE"))
				return StringToBool(www.responseHeaders["ERROR-STATE"]);
			
			return false;
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