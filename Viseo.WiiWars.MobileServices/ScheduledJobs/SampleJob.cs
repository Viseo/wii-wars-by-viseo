﻿using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;

namespace Viseo.WiiWars.MobileServices.ScheduledJobs
{
    // A simple scheduled job which can be invoked manually by submitting an HTTP
    // POST request to the path "/jobs/sample".

    public class SampleJob : ScheduledJob
    {
        public override Task ExecuteAsync()
        {
            Services.Log.Info("Hello from scheduled job Hello World WiiWars!");
            return Task.FromResult(true);
        }
    }
}