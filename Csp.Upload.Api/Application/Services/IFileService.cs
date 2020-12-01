using Csp.Upload.Api.Application.Dtos;
using Csp.Upload.Api.Models;
using Csp.Web;
using Microsoft.AspNetCore.Http;

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

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <param name="key">上传文件类型</param>
        /// <param name="localUrl">部署站点的url</param>
        /// <returns></returns>
        UploadOutput Upload(IFormFile file, string key, string localUrl);

        /// <summary>
        /// 根据id获取图片，并返回字节流数组
        /// </summary>
        /// <param name="id">图片编号</param>
        /// <returns></returns>
        FileOutput Get(string id);

        /// <summary>
        /// 根据id获取相应宽度的图片，并返回字节流数组 即按宽度获取缩略图
        /// </summary>
        /// <param name="id">图片编号</param>
        /// <param name="width">指定宽度</param>
        /// <returns></returns>
        FileOutput Get(string id, int width);

        /// <summary>
        /// 根据图片编码删除图片
        /// </summary>
        /// <param name="id">图片编号</param>
        /// <returns></returns>
        OptResult Remove(string id);

        void ImportImage();
    }
}
