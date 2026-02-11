using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Base
{
    public class PageRequestBase<T> where T : new()
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public T Data { get; set; } = new();
    }
}
