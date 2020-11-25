namespace Csp.Upload.Api.Application.Dtos
{
    public class UploadOutput
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 状态有：uploading done error removed
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 文件唯一标识，建议设置为负数，防止和内部产生的 id 冲突
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string ThumbUrl { get; set; }


        public UploadOutput(string name,string status,string uid,string url,string thumbUrl="")
        {
            Name = name;
            Status = status;
            Uid = uid;
            Url = url;
            ThumbUrl = thumbUrl;
        }
    }
}
