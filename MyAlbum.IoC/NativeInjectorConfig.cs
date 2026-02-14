using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAlbum.Application;
using MyAlbum.Application.Identity;
using MyAlbum.Application.Identity.implement;
using MyAlbum.Application.Uploads;
using MyAlbum.Application.Uploads.implement;
using MyAlbum.Domain;
using MyAlbum.Models.Identity;
using MyAlbum.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyAlbum.IoC
{
    public static class NativeInjectorConfig
    {
        public static void RegisterService(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher<AccountDto>, PasswordHasher<AccountDto>>();

            // 統一登入入口
            services.AddScoped<ILoginManagerService, LoginManagerService>();
            // 多實作介面：ILoginService（會注入到 IEnumerable<ILoginService>）
            services.AddScoped<ILoginService, AdminUserLoginService>();
            services.AddScoped<ILoginService, MemberUserLoginService>();

            services.AddScoped<IFileUploadManagerService, FileUploadManagerService>();
            services.AddScoped<IFileUploadService, MemberUploadService>();

            // 固定線
            services.AddScoped<IAlbumDbContextFactory, AlbumDbContextFactory>();
            // 重試策略
            services.AddScoped<IExecutionStrategyFactory, EfExecutionStrategyFactory>();

            // 慣例掃描
            var servicesAsm = typeof(BaseService).Assembly;
            var efAsm = typeof(AlbumDbContextFactory).Assembly;
            services.RegisterByConvention(servicesAsm, efAsm);
        }
    }
}
