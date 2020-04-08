using Csp.Jwt;
using Csp.Upload.Api.Infrastructure;
using Csp.Upload.Api.Models;
using Csp.Web;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace Csp.Upload.Api.Application.Services
{

    public class FileService : IFileService
    {
        readonly IWebHostEnvironment _environment;

        //定义允许上传的文件扩展名
        readonly Hashtable extTable = new Hashtable();

        readonly OssDbContext _ossDbContext;

        readonly AppUser _appUser;


        public FileService(IWebHostEnvironment environment, OssDbContext ossDbContext,IIdentityParser<AppUser> parser)
        {
            _environment = environment;
            _ossDbContext = ossDbContext;
            _appUser = parser.Parse();


            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,mp4,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
        }

        public FileModel Add(string fullPath,string contentType, double size, string key)
        {
            var dirName = Path.Combine(_environment.ContentRootPath, "attached", key, DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            var name = Path.GetFileNameWithoutExtension(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var savePath = Path.Combine(dirName, fileName.Replace(name, Path.GetRandomFileName()));

            FileModel model = new FileModel
            {
                Name = name,
                FilePath = savePath,
                FileSize = size,
                Ext = Path.GetExtension(fullPath),
                ContentType = contentType,
                UserId = _appUser.Id,
                TenantId = _appUser.TenantId
            };

            _ossDbContext.Files.Add(model);

            _ossDbContext.SaveChanges();

            return model;
        }

        public string GetAllowExtension(string key)
        {
            return extTable[key].ToString();
        }


        public FileModel GetFilePathById(string id)
        {
            var file = _ossDbContext.Files.SingleOrDefault(a => a.Id == id);

            return file;
        }

        public bool IsAllowUploadExtension(string filePath, string key)
        {
            var fileExt = Path.GetExtension(filePath);

            return string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[key]).Split(','), fileExt.Substring(1).ToLower()) == -1;
        }

        public OptResult IsContentLength(long length, string key)
        {
            if (length > 30*1024*1024 && key != "image")
                OptResult.Failed("上传文件大小超过限制30M");

            if (length > 1 * 1024 * 1024 && key == "image")
                return OptResult.Failed("上传文件大小超过限制1M");

            return OptResult.Success();
        }
    }
}
