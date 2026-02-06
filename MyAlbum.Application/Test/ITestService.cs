using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Application.Test
{
    public interface ITestService
    {
        Task<bool> GetConnectResult();
    }
}
