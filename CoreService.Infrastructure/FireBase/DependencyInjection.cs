using CoreService.Contracts.Interfaces;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreService.Infrastructure.FireBase;

public static class DependencyInjection
{
    public static void AddFirebase(this WebApplicationBuilder builder)
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(@"C:\Users\third\Desktop\indifferentnobank-firebase-adminsdk-fbsvc-7e7b5462e0.json"),
        });
        builder.Services.AddTransient<IFireBaseSender, FireBaseSender>();
    }
}