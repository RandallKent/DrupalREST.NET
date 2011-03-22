using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;


namespace DrupalREST
{
    public class DrupalRESTInterface
    {
        public string DrupalEndpoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Session { get; set; }

        public string Login()
        {

            Dictionary<string, string> postables = new Dictionary<string, string>();

            postables.Add("username", this.Username);
            postables.Add("password", this.Password);

            var result = HttpPost(this.DrupalEndpoint + "user/login/", postables);

            //TODO: Handle result to extract session informatino which needs to be stored for future authenticated requests
            return result;
        }

        //pass this guy a node id and it will return the raw node
        public string RetrieveNode(int nid)
        {
            var result = HttpGet(this.DrupalEndpoint + "node/" + nid);

            //TODO: Handle the result to make it more usable.
            return result;
        }

        static string HttpGet(string url)
        {
            // Setup the Request
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }

        static string HttpPost(string url, Dictionary<string, string> paramz)
        {

            // Setup the Request
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder query = new StringBuilder();
            foreach (KeyValuePair<string, string> param in paramz)
            {
                query.AppendFormat("{0}={1}&", param.Key, HttpUtility.UrlEncode(param.Value));
            }

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(query.ToString());

            request.ContentLength = formData.Length;

            // Send the request:
            using (Stream post = request.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            // Pick up the response:
            string result = null;
            using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }

    }
}
