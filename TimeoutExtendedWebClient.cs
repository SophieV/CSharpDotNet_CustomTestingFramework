﻿using System;
using System.Net;

namespace TestMVC4App.Models
{
    public class TimeoutExtendedWebClient : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request != null)
                request.Timeout = Timeout;
            return request;
        }

        public TimeoutExtendedWebClient()
        {
            this.Timeout = 100000;
        }
    }
}