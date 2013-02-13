using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Web;
using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;
using LitJson;

namespace EightBitIdeas.Lib8bit.Net.Http
{
    public class HttpClient
    {
        private string HttpVersion;
        private int StatusCode; 
        private string StatusMessage;
        
        private Dictionary<string, string> ResponseHeaders;

        private string ResponseBody;

        public HttpClient(Uri uri)
        {
            Query(uri, "GET", null, null);
        }

        public HttpClient(Uri uri, string method, Dictionary<string, string> headers, string body)
        {
            Query(uri, method, headers, body);
        }

        public HttpClient(Uri uri, string method, Dictionary<string, string> headers, Dictionary<string, string> form)
        {
            if (headers == null)
                headers = new Dictionary<string,string>();

            if (headers.ContainsKey("Content-Type"))
                headers.Remove("Content-Type");

            headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string body = "";
            foreach (KeyValuePair<string, string> kvp in form)
            {
                body += string.Format("{0}={1}&", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value));
            }

            body = body.TrimEnd('&');
            Query(uri, method, headers, body);
        }

        private void Query(Uri uri, string method, Dictionary<string, string> headers, string body)
        {
            //build the request string we'll be sending to the server
            string request = string.Format("{0} {1} HTTP/1.1\r\n", method.ToUpper(), uri.PathAndQuery);

            if (headers == null)
                headers = new Dictionary<string, string>();

            if (body == null)
                body = "";

            if (!headers.ContainsKey("Host"))
                request += string.Format("Host: {0}:{1}\r\n", uri.Host, uri.Port);

            foreach (KeyValuePair<string, string> header in headers)
            {
                if (header.Key.Contains(":"))
                    throw new Exception("headers cannot contain ':'");

                request += string.Format("{0}: {1}\r\n", header.Key, header.Value);
            }

            // we don't support persistant connections yet, so tell the server to close the connection
            request += "Connection: close\r\n";

            //add content length if it's not in the header collection
            if (!headers.ContainsKey("Content-Length") && body.Length != 0)
                request += string.Format("Content-Length: {1}\r\n", body.Length);

            request += "\r\n";

            request += body;

            //open a socket connection to the host and port in the uri
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect(uri.Host, uri.Port);

            //send the request we built to the server
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(request);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            //get the response a string
            StreamReader reader = new StreamReader(serverStream);
            string response = reader.ReadToEnd();

            if (!response.StartsWith("HTTP"))
                throw new Exception("server did not respond with a proper http response: " + response);

            //parse the response from the server
            string[] responseSplit = response.Split(new string[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

            if (responseSplit.Length != 2)
                throw new Exception("server did not give a proper Http response: " + response);

            string[] headerSplit = responseSplit[0].Split(new string[] { "\r\n" }, StringSplitOptions.None);

            ResponseHeaders = new Dictionary<string, string>();

            string responseStatusLine = headerSplit[0];
            string[] statusLineSplit = responseStatusLine.Split(new char[] { ' ' }, 3);

            HttpVersion = statusLineSplit[0];
            StatusCode = Convert.ToInt32(statusLineSplit[1]);
            StatusMessage = statusLineSplit[2];

            for (int c = 1; c < headerSplit.Length; c++)
            {
                string[] headerKvp = headerSplit[c].Split(new char[] { ':' }, 2, StringSplitOptions.None);
                ResponseHeaders.Add(headerKvp[0].Trim(), headerKvp[1].Trim());
            }

            ResponseBody = responseSplit[1];
        }

        public Dictionary<string, string> GetHeaders()
        {
            return ResponseHeaders;
        }

        public string GetHeader(string key)
        {
            if (ResponseHeaders.ContainsKey(key))
                return ResponseHeaders[key];
            else
                return null;
        }

        public string GetBody()
        {
            return ResponseBody;
        }

        public int GetStatusCode()
        {
            return StatusCode;
        }

        public string GetResponseHttpVersion()
        {
            return HttpVersion;
        }

        public JsonData GetJsonData()
        {
            return LitJson.JsonMapper.ToObject(GetBody());
        }

        public bool IsError()
        {
            if (StatusCode == 200)
                return false;
            else
                return true;
        }

        public ErrorResponse GetError()
        {
            ErrorResponse error = new ErrorResponse();
            error.FromJsonData(GetJsonData());

            return error;
        }
    }
}
