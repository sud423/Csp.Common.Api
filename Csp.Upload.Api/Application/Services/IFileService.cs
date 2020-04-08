using Csp.Upload.Api.Models;
using Csp.Web;

namespace Csp.Upload.Api.Application.Services
{
    public interface IFileService
    {

        /// <summary>
        /// 添加文件记录
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="size">文件大小</param>
        /// <param name="key">image,file,flash,media</param>
        /// <returns></returns>
        FileModel Add(string fullPath, string contentType, double size, string key);

        /// <summary>
        /// 根据key获取允许上传的文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetAllowExtension(string key);
        
        /// <summary>
        /// 根据主键id获取文件存放路径
        /// </summary>
        /// <param name="id">文件编号</param>
        /// <returns></returns>
        FileModel GetFilePathById(string id);

        /// <summary>
        /// 根据key判断当前的文件是否允许上传
        /// </summary>
        /// <param name="filePath">当前上传的文件路径</param>
        /// <param name="key"></param>
        /// <returns>true表示允许上传，false不允许</returns>
        bool IsAllowUploadExtension(string filePath,string key);

        /// <summary>
        /// 根据key判断文件是否超出大小
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        OptResult IsContentLength(long length,string key);
    }
}
