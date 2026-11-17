using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    var sslProtocols = builder.Configuration.GetValue<System.Security.Authentication.SslProtocols?>("Kestrel:SslProtocols")
        ?? (System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13);
    options.ConfigureHttpsDefaults(https => https.SslProtocols = sslProtocols);

    if (builder.Configuration.GetValue<long?>("Kestrel:Limits:MaxRequestBodySize") is long maxRequestBodySize)
    {
        options.Limits.MaxRequestBodySize = maxRequestBodySize < 0 ? null : maxRequestBodySize;
    }
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Clear();
    options.KnownIPNetworks.Clear();

    foreach (var proxy in builder.Configuration.GetSection("ForwardedHeaders:TrustedProxies").Get<string[]>() ?? [])
    {
        if (System.Net.IPAddress.TryParse(proxy, out var address))
        {
            options.KnownProxies.Add(address);
        }
    }

    foreach (var network in builder.Configuration.GetSection("ForwardedHeaders:TrustedNetworks").Get<string[]>() ?? [])
    {
        if (System.Net.IPNetwork.TryParse(network, out var ipNetwork))
        {
            options.KnownIPNetworks.Add(ipNetwork);
        }
    }

    if (options.KnownProxies.Count == 0 && options.KnownIPNetworks.Count == 0)
    {
        options.KnownIPNetworks.Add(new System.Net.IPNetwork(System.Net.IPAddress.Loopback, 32));
        options.KnownIPNetworks.Add(new System.Net.IPNetwork(System.Net.IPAddress.IPv6Loopback, 128));
    }

    foreach (var host in builder.Configuration.GetSection("ForwardedHeaders:AllowedHosts").Get<string[]>() ?? [])
    {
        options.AllowedHosts.Add(host);
    }

    if (options.AllowedHosts.Count == 0)
    {
        options.ForwardedHeaders &= ~ForwardedHeaders.XForwardedHost;
    }

    if (builder.Configuration.GetValue<int?>("ForwardedHeaders:ForwardLimit") is int forwardLimit)
    {
        options.ForwardLimit = forwardLimit < 0 ? null : forwardLimit;
    }
});

builder.Services.AddHttpForwarder();
builder.Services.AddControllersWithViews();
builder.Services.Configure<Microsoft.AspNetCore.Builder.CookiePolicyOptions>(options =>
{
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
});

var app = builder.Build();

app.UseForwardedHeaders();
app.Use(async (context, next) =>
{
    context.Response.OnStarting(static state =>
    {
        var headers = ((HttpResponse)state).Headers;
        headers.Remove("Server");
        headers.Remove("X-Powered-By");
        headers.Remove("X-AspNet-Version");
        headers.Remove("X-AspNetMvc-Version");
        headers["X-Content-Type-Options"] = "nosniff";
        return Task.CompletedTask;
    }, context.Response);

    await next(context);
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/StatusErrorCode", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.MapControllerRoute(
    name: "error",
    pattern: "Home/Error",
    defaults: new { controller = "Home", action = "Error" });
app.MapControllerRoute(
    name: "status-error",
    pattern: "Home/StatusErrorCode",
    defaults: new { controller = "Home", action = "StatusErrorCode" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]!).WithOrder(int.MaxValue);

app.Run();
