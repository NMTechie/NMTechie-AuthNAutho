
using DCT.EIMS.UnifiedLogin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;

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
/*
 * All are in perspective of B2C setup
 * The fundmental thing is that , 
 * If you need ID_token then it works both in implicit grant and hybrid flows --> means does not require client secrect and responsetype to be auth code flow
 * if you need access_token then it requires the Implicit grant if you doesnot pass client secrect --> menas responsetype is not auth code flow
 * if you need access_token with auth code flow , then you require a client secrect and response type is auth code flow --> means do not require implicit grant 
 */
void SetupAzureADB2CHnadShakeOptions(MicrosoftIdentityOptions options)
{
    builder.Configuration.Bind("AzureADB2C", options);
    //options.ClientSecret = "-aE8Q~YoqdqXYvVbpuqrTyCc5n79hPF-dQ9xgcb-";
    options.Scope.Add(options.ClientId);
    options.Scope.Add("offline_access");
    options.SaveTokens = true;
    //options.ResponseType = OpenIdConnectResponseType.Code;
    options.Events = new OpenIdConnectEvents()
    {
        OnMessageReceived = HandleMessageReceived,
        OnRemoteFailure = HandleRemoteFailure,
        OnRedirectToIdentityProvider = ConfigureIDPParameters
    };

}

Task ConfigureIDPParameters(RedirectContext redirectContext)
{   
    redirectContext.ProtocolMessage.IssuerAddress = @"https://SuiteLoginExp.b2clogin.com/SuiteLoginExp.onmicrosoft.com/b2c_1_unifiedloginpolicy1/oauth2/v2.0/authorize";
    redirectContext.Options.Authority = @"https://SuiteLoginExp.b2clogin.com/SuiteLoginExp.onmicrosoft.com/B2C_1_UnifiedLoginPolicy1/v2.0";
    redirectContext.Options.MetadataAddress = $"{UnifiedAuthConstant.B2CInstance}/{UnifiedAuthConstant.B2CDomain}/{UnifiedAuthConstant.policyID}/v2.0/.well-known/openid-configuration";
    redirectContext.Options.ConfigurationManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<OpenIdConnectConfiguration>(redirectContext.Options.MetadataAddress, new OpenIdConnectConfigurationRetriever());
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
    IdentityModelEventSource.ShowPII = true;
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    IdentityModelEventSource.ShowPII = true;

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
