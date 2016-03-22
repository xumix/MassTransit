﻿// Copyright 2007-2016 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.HttpTransport.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using Configuration.Builders;
    using Hosting;
    using NUnit.Framework;
    using Util;


    public class HttpHostTests
    {
        [Test]
        public async void X()
        {
            var sup = new TaskSupervisor("test");
            var cache = new OwinHostCache(new HttpHostSettingsImpl("http","localhost",8080,HttpMethod.Post), sup);
            await cache.Send(Pipe.Empty<OwinHostContext>(), default(CancellationToken));
            await sup.Stop("test");
        }

        [Test]
        public async void HttpHost_NoEndpoints()
        {
            var host = new HttpHost(new HttpHostSettingsImpl("http","localhost",8080, HttpMethod.Post));
            var handle = host.Start();
            using (var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                await handle.Stop(tokenSource.Token);
            }
        }

        [Test]
        public async void HttpHost_Endpoints()
        {
            var host = new HttpHost(new HttpHostSettingsImpl("http", "localhost", 8080, HttpMethod.Post));
            var rep = new HttpReceiveTransport(host, new HttpReceiveSettings("localhost",8080,"bob"), null);
            var randle = rep.Start(Pipe.Empty<ReceiveContext>());

            var handle = host.Start();
            using (var tokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                await randle.Stop(tokenSource.Token);
                await handle.Stop(tokenSource.Token);
            }
        }

        [Test]
        public void Endpoint()
        {
            
        }
    }
}