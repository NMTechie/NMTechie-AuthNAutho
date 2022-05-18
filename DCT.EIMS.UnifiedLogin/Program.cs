
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



/*This code use the ASP.NET core deafult scafolding of Authentication middleware that use implicit grant flow.
 * builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
 * .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
 */

/* This code provides a way to perform the Authorization code flow 
 * token generation process where all the options are static in terms of the 
 * Azure AD B2C flow
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
 .AddMicrosoftIdentityWebApp(msIdentityOpt => 
 {
     builder.Configuration.Bind("AzureADB2C", msIdentityOpt);
     msIdentityOpt.ClientSecret = "uvc7Q~Jjb.fbuJiGTwvvbdigPh0d_ITtGsZ~z";
     msIdentityOpt.Scope.Add(msIdentityOpt.ClientId);
     msIdentityOpt.Scope.Add("offline_access");
     msIdentityOpt.SaveTokens = true;
     msIdentityOpt.ResponseType = OpenIdConnectResponseType.Code;
     //msIdentityOpt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() { };

 });
*/
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
 .AddMicrosoftIdentityWebApp(SetupAzureADB2CHnadShakeOptions);

void SetupAzureADB2CHnadShakeOptions(MicrosoftIdentityOptions options)
{
    builder.Configuration.Bind("AzureADB2C", options);
    options.ClientSecret = "uvc7Q~Jjb.fbuJiGTwvvbdigPh0d_ITtGsZ~z";
    options.Scope.Add(options.ClientId);
    options.Scope.Add("offline_access");
    options.SaveTokens = true;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Events = new OpenIdConnectEvents()
    {
        OnMessageReceived =  HandleMessageReceived,
        OnRemoteFailure = HandleRemoteFailure,
        OnRedirectToIdentityProvider = ConfigureIDPParameters
    };

}

Task ConfigureIDPParameters(RedirectContext redirectContext)
{
    redirectContext.ProtocolMessage.IssuerAddress = @"https://nmtechie.b2clogin.com/nmtechie.onmicrosoft.com/b2c_1_unifiedloginpolicy1/oauth2/v2.0/authorize";
    redirectContext.Options.Authority = @"https://nmtechie.b2clogin.com/NMTechie.onmicrosoft.com/B2C_1_UnifiedLoginPolicy1/v2.0";
    return Task.FromResult(0);
}

Task HandleRemoteFailure(RemoteFailureContext remoteFailureContext)
{
    return Task.FromResult(0);
}

Task HandleMessageReceived(MessageReceivedContext messageReceivedContext)
{
    return Task.FromResult(0);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Start}/{id?}");

app.Run();
