using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MlsaBadgeMaker.Api;
using MlsaBadgeMaker.Api.Repositories;
using MlsaBadgeMaker.Api.Services;

[assembly: FunctionsStartup(typeof(Startup))]
namespace MlsaBadgeMaker.Api
{
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<MlsaDirectoryService>();
            builder.Services.AddSingleton<IIntrospectionService, MsGraphIntrospectionService>();
            builder.Services.AddSingleton<IAvatarGenerator, ImageSharpAvatarGenerator>();

            //builder.Services.AddSingleton<ILiteDatabase, LiteDatabase>(_ => 
            //    new LiteDatabase(Path.Combine(builder.GetContext().ApplicationRootPath, "data.db")));
            //builder.Services.AddSingleton<IMembersRepository, LiteDbMembersRepository>();

            builder.Services.AddSingleton<CloudTableClient>(x =>
            {
                var storageAccount =
                    CloudStorageAccount.Parse(builder.GetContext().Configuration
                        .GetConnectionStringOrSetting("TableConnection"));

                return storageAccount.CreateCloudTableClient();
            });

            builder.Services.AddSingleton<IMembersRepository, TableMembersRepository>();
        }
    }
}