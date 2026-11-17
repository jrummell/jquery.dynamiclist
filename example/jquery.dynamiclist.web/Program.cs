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

builder.Services.AddControllersWithViews();
builder.Services.Configure<Microsoft.AspNetCore.Builder.CookiePolicyOptions>(options =>
{
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
});

var app = builder.Build();

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

app.Run();
