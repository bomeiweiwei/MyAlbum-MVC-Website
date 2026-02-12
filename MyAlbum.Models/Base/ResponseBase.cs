using MyAlbum.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAlbum.Models.Base
{
    public class ResponseBase<T>
    {
        /// <summary>
        /// 內容
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 總筆數
        /// </summary>
        public long Count { get; set; } = 0;
    }
}
