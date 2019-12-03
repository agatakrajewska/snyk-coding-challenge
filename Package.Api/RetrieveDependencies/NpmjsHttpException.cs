using System;
using System.Net.Http;

namespace Package.Api.RetrieveDependencies
{
    public class NpmjsHttpException : Exception
    {
        public HttpResponseMessage Response { get; }

        // in real life third party api would probably be giving us some sort of error so I would
        // serialize it, pass in on to this exception and add it to the message 
        public NpmjsHttpException(HttpResponseMessage response)
            : base($"Npm HTTP request to {response.RequestMessage.RequestUri} failed; Status code: {response.StatusCode}")
        {
            Response = response;
        }
    }
}