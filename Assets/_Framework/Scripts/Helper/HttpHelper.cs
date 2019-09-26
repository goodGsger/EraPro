using System.Net;
using System;
using System.IO;
using System.Text;
using System.Collections;
using UnityEngine;

namespace Framework
{
    public delegate void HttpRequestCallback(string jsonStr);
    public class HttpHelper
    {
        /// <summary>
        /// HttpWebRequest:Post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static void SendRequest1(string postData, string url, HttpRequestCallback callback = null)
        {
            if (url == null || url.Equals(""))
            {
                App.logManager.Info("--- httpHelper url is null ---");
                return;
            }
            if (postData == null)
            {
                postData = "";
            }
            if (callback == null)
            {
                PostAsyn(url, postData);
            }
            else
            {
                CoroutineTask task = App.objectPoolManager.GetObject<CoroutineTask>();
                task.routine = PostSyn(url, postData, callback);
                task.Start();
                task.callback = (t) =>
                {
                    App.objectPoolManager.ReleaseObject(t);
                };
            }
        }
        /// <summary>
        /// 同步POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="callback"></param>
        private static IEnumerator PostSyn(string url, string postData, HttpRequestCallback callback)
        {
            string htmlStr = string.Empty;
            string _responseInfo = string.Empty;
            Encoding encoding = Encoding.GetEncoding("UTF-8");

            //创建一个客户端的Http请求实例
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            byte[] _data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = _data.Length;
            request.ReadWriteTimeout = 60000;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(_data, 0, _data.Length);
            requestStream.Close();

            //获取当前Http请求的响应实例
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                var sr = new StreamReader(responseStream, encoding);
                //返回结果网页（html）代码  
                _responseInfo = sr.ReadToEnd();

            }

            App.logManager.Info("--- post back info ：" + _responseInfo);
            callback(_responseInfo);
            yield return null;
        }

        /// <summary>
        /// 异步POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        private static void PostAsyn(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream((IAsyncResult asyncResult) =>
            {
                HttpWebRequest request1 = asyncResult.AsyncState as HttpWebRequest;
                Stream postStream;
                try
                {
                    postStream = request1.EndGetRequestStream(asyncResult);
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    postStream.Write(byteArray, 0, byteArray.Length);
                    postStream.Close();
                    request1.BeginGetResponse((IAsyncResult asyncResult2) =>
                    {
                        HttpWebRequest request2 = asyncResult2.AsyncState as HttpWebRequest;
                        HttpWebResponse response = request2.EndGetResponse(asyncResult2) as HttpWebResponse;
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            String _responseInfo = reader.ReadToEnd();
                            App.logManager.Info("--- post back info ：" + _responseInfo);
                        }
                    }, request1);
                }
                catch (Exception exception)
                {
                    App.logManager.Warn("Request Failed!"  + exception + "  url:" + url);
                }
            }, request);

        }
    }
}
