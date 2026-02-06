using MyAlbum.Domain;
using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Test.implement
{
    public class TestService : BaseService, ITestService
    {
        public TestService(IAlbumDbContextFactory factory) : base(factory) { }

        public async Task<bool> GetConnectResult()
        {
            using var ctx = MainDB(ConnectionMode.Slave);
            return await ctx.CanConnectAsync();
        }
    }
}
