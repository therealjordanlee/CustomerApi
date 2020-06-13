using System;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace CustomerApiStub
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://+:8080" },
                StartAdminInterface = true
            });

            // GET /customers?firstnameincludes=*&lastnameincludes=*
            stub
            .Given(Request.Create().WithPath("/customers")
                .UsingGet()
                .WithParam("firstnameincludes", new RegexMatcher(".*"))
                .WithParam("lastnameincludes", new RegexMatcher(".*"))
            )
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(@"[{""id"":1,""firstName"":""John"",""lastName"":""Doe"",""dateOfBirth"":""2000-01-01T00:00:00""}]"));

            // GET /customers?firstnameincludes=*
            stub
            .Given(Request.Create().WithPath("/customers")
                .UsingGet()
                .WithParam("firstnameincludes", new RegexMatcher(".*"))
            )
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(@"[{""id"":1,""firstName"":""John"",""lastName"":""Doe"",""dateOfBirth"":""2000-01-01T00:00:00""}]"));

            // GET /customers?lastnameincludes=*
            stub
            .Given(Request.Create().WithPath("/customers")
                .UsingGet()
                .WithParam("lastnameincludes", new RegexMatcher(".*"))
            )
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(@"[{""id"":1,""firstName"":""John"",""lastName"":""Doe"",""dateOfBirth"":""2000-01-01T00:00:00""}]"));

            // GET /customers
            stub
            .Given(Request.Create().WithPath("/customers").UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithBody(@"[{""id"":1,""firstName"":""John"",""lastName"":""Doe"",""dateOfBirth"":""2000-01-01T00:00:00""},{""id"":2,""firstName"":""Jane"",""lastName"":""Doe"",""dateOfBirth"":""2000-01-01T00:00:00""},{""id"":3,""firstName"":""Mohammed"",""lastName"":""Ali"",""dateOfBirth"":""2000-01-01T00:00:00""}]"));

            // POST /customers
            stub
            .Given(Request.Create().WithPath("/customers")
                .UsingPost()
                .WithHeader("Content-Type", "application/json")
                .WithBody(new RegexMatcher(@"^{""firstname"":""(.*)"",""lastname"":""(.*)"",""dateofbirth"":""([0-9]{4}\-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2})""}$"))
            )
            .RespondWith(Response.Create().WithStatusCode(200));

            // POST /customers
            stub
            .Given(Request.Create().WithPath("/customers")
                .UsingPost()
                .WithBody(new RegexMatcher("(.*)"))
            )
            .RespondWith(Response.Create().WithStatusCode(400));

            // DELETE /customers/{id}
            stub
            .Given(Request.Create().WithPath(new RegexMatcher("/customers/(.*)")).UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(200));

            // PUT /customers/{id}
            stub
            .Given(Request.Create().WithPath(new RegexMatcher("/customers/(.*)"))
                .UsingPut()
                .WithHeader("Content-Type", "application/json")
                .WithBody(new RegexMatcher(@"^{""firstname"":""(.*)"",""lastname"":""(.*)"",""dateofbirth"":""([0-9]{4}\-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2})""}$"))
            )
            .RespondWith(Response.Create().WithStatusCode(200));

            // PUT /customers/{id}
            stub
            .Given(Request.Create().WithPath(new RegexMatcher("/customers/(.*)"))
                .UsingPut()
                .WithBody(new RegexMatcher("(.*)"))
            )
            .RespondWith(Response.Create().WithStatusCode(400));

            Console.WriteLine("Press any key to stop the server");
            Console.ReadLine();
            stub.Stop();
        }
    }
}