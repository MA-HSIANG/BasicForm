using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Model.Dtos
{
    public class ResponseDTO<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
    }
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public class ResponseListDTO<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<T> Data { get; set; } = new();
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 資料總數
        /// </summary>
        public int TotalCount { get; set; } = 0;
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
            }
        }
    }
}
