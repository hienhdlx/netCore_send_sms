﻿using System;
using System.Net;
using System.IO;
namespace SpeedSMSAPI
{
    class SpeedSMSAPI
    {
        public const int TYPE_QC = 1;
        public const int TYPE_CSKH = 2;
        public const int TYPE_BRANDNAME = 3;
        public const int TYPE_BRANDNAME_NOTIFY = 4; // Gửi sms sử dụng brandname Notify
        public const int TYPE_GATEWAY = 5; // Gửi sms sử dụng app android từ số di động cá nhân, download app tại đây: https://play.google.com/store/apps/details?id=com.speedsms.gateway
        public const int TYPE_FIXNUMBER = 6; //sms gui bang dau so co dinh 0901756186
        public const int TYPE_OWN_NUMBER = 7; //sms gui bang dau so rieng da duoc dang ky voi SpeedSMS
        public const int TYPE_TWOWAY_NUMBER = 8; //sms gui bang dau so co dinh 2 chieu

        const String rootURL = "https://api.speedsms.vn/index.php";
        private String accessToken = "S2ejUPVthW3rN4AwwMuHeRRYDjGxXH9W";

        public SpeedSMSAPI()
        {

        }

        public SpeedSMSAPI(String token)
        {
            this.accessToken = token;
        }

        private string EncodeNonAsciiCharacters(string value)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public String getUserInfo()
        {
            String url = rootURL + "/user/info";
            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            return reader.ReadToEnd();
        }

        public String GetCode(String phone)
        {
            string url = rootURL + "/pin/create";
            if (phone.Length <= 0) { return ""; };
            string content = "{ \"to\": \"" + phone + "\", \"content\": \"Your verification code is: {pin_code}\", \"app_id\": \"9LYF1zv14z-O6YJI1FDrnmXCEcpsvx1x\"}";
            NetworkCredential myCreds = new NetworkCredential(accessToken, "-H");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            var result = client.UploadString(url, content.ToString());
            return result;
        }

        public string VerifyCode(string phone, string code)
        {
            string url = rootURL + "/pin/verify";
            if (phone.Length <= 0) return "";
            if (code.Length <= 0) return "";
            string content = "{\"phone\": \"" + phone + "\", \"pin_code\": \"" + code + "\", \"app_id\": \"9LYF1zv14z-O6YJI1FDrnmXCEcpsvx1x\"}";
            NetworkCredential myCreds = new NetworkCredential(accessToken, "-H");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            var result = client.UploadString(url, content.ToString());
            return result;
        }

        public String sendSMS(String[] phones, String content, int type, String sender)
        {
            String url = rootURL + "/sms/send";
            if (phones.Length <= 0)
                return "";
            if (content.Equals(""))
                return "";

            if (type == TYPE_BRANDNAME && sender.Equals(""))
                return "";
            if (!sender.Equals("") && sender.Length > 11)
                return "";

            NetworkCredential myCreds = new NetworkCredential(accessToken, ":x");
            WebClient client = new WebClient();
            client.Credentials = myCreds;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string builder = "{\"to\":[";

            for (int i = 0; i < phones.Length; i++)
            {
                builder += "\"" + phones[i] + "\"";
                if (i < phones.Length - 1)
                {
                    builder += ",";
                }
            }
            builder += "], \"content\": \"" + EncodeNonAsciiCharacters(content) + "\", \"type\":" + type + ", \"sender\": \"" + sender + "\"}";

            String json = builder.ToString();
            return client.UploadString(url, json);
        }
    }
}
