namespace Csp.Upload.Api.Application.Dtos
{
    public class FileOutput
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// 文件内容类型
        /// </summary>
        public string ContentType { get; set; }

        public FileOutput(byte[] bytes,string contentType)
        {
            Bytes = bytes;
            ContentType = contentType;
        }
    }
}
