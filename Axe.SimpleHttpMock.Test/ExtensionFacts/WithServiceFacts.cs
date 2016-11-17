﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Axe.SimpleHttpMock.Test.ExtensionFacts
{
    public class WithServiceFacts : HttpServerFactBase
    {
        [Fact]
        public async void should_get_service_defined_api_result()
        {
            var httpServer = new MockHttpServer();
            httpServer
                .WithService("http://www.baidu.com")
                .Api("user/login", _ => HttpStatusCode.OK.AsResponse(), "1")
                .Api("user/login/{userId}", _ => HttpStatusCode.Accepted.AsResponse(), "2")
                .Api("another", _ => HttpStatusCode.Ambiguous.AsResponse(), "3")
                .Done()
                .WithService("http://www.sina.com")
                .Api("user/login", _ => HttpStatusCode.Continue.AsResponse(), "4")
                .Done();

            var client = new HttpClient(httpServer);

            await AssertGetStatusCode(client, "http://www.baidu.com/user/login", HttpStatusCode.OK);
            await AssertGetStatusCode(client, "http://www.baidu.com/user/login/2", HttpStatusCode.Accepted);
            await AssertGetStatusCode(client, "http://www.baidu.com/another", HttpStatusCode.Ambiguous);
            await AssertGetStatusCode(client, "http://www.sina.com/user/login", HttpStatusCode.Continue);

            AssertCalled(httpServer, "1");
            AssertCalled(httpServer, "2");
            AssertCalled(httpServer, "3");
            AssertCalled(httpServer, "4");
        }

        static void AssertCalled(MockHttpServer httpServer, string handlerName)
        {
            IRequestHandlerTracer tracer = httpServer.GetNamedHandlerTracer(handlerName);
            tracer.VerifyHasBeenCalled();
        }

        // ReSharper disable once UnusedParameter.Local
        static async Task AssertGetStatusCode(HttpClient client, string uri, HttpStatusCode expectedStatusCode)
        {
            HttpResponseMessage responseOK = await client.GetAsync(uri);
            Assert.Equal(expectedStatusCode, responseOK.StatusCode);
        }
    }
}