
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



/*This code use the ASP.NET core deafult scafolding of Authentication middleware that use implicit grant flow.
 * builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
 * .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
 */

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
