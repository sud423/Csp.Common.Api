using Csp.Jwt;
using Csp.Upload.Api.Application.Dtos;
using Csp.Upload.Api.Infrastructure;
using Csp.Upload.Api.Models;
using Csp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Drawing;
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

        private readonly AppSettings _settings;


        public FileService(IWebHostEnvironment environment, OssDbContext ossDbContext,IIdentityParser<AppUser> parser, IOptions<AppSettings> appSettings)
        {
            _environment = environment;
            _ossDbContext = ossDbContext;
            _appUser = parser.Parse();
            _settings = appSettings.Value;


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

        public FileOutput Get(string id)
        {
            var file = _ossDbContext.Files.SingleOrDefault(a => a.Id == id);

            if (file == null || !File.Exists(file.FilePath))
                return null;

            using var sw = new FileStream(file.FilePath, FileMode.Open, FileAccess.Read);
            var bytes = new byte[sw.Length];
            sw.Read(bytes, 0, bytes.Length);
            sw.Close();

            return new FileOutput(bytes,file.ContentType);
        }

        public FileOutput Get(string id, int width)
        {
            var file = _ossDbContext.Files.SingleOrDefault(a => a.Id == id);

            if (file == null || !File.Exists(file.FilePath))
                return null;

            //缩小图片
            using var imgBmp = new Bitmap(file.FilePath);
            //找到新尺寸
            var oWidth = imgBmp.Width;
            var oHeight = imgBmp.Height;
            var height = oHeight;
            if (width > oWidth)
            {
                width = oWidth;
            }
            else
            {
                height = width * oHeight / oWidth;
            }
            var newImg = new Bitmap(imgBmp, width, height);
            newImg.SetResolution(100, 100);
            var ms = new MemoryStream();
            newImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var bytes = ms.GetBuffer();
            ms.Close();

            return new FileOutput(bytes, file.ContentType);
        }

        public string GetAllowExtension(string key)
        {
            return extTable[key].ToString();
        }

        public void ImportImage()
        {
            string dirPath = Path.Combine(_environment.ContentRootPath, @"attached\image");
            var dires = Directory.GetDirectories(dirPath);
            foreach(string dir in dires)
            {
                var files=Directory.GetFiles(dir);

                foreach(string file in files)
                {
                    var ext= Path.GetExtension(file);

                    FileModel model = new FileModel
                    {
                        Name = Path.GetFileName(file),
                        FilePath = file,
                        FileSize = File.Open(file, FileMode.Open, FileAccess.Read).Length,
                        Ext = ext,
                        ContentType = ext == "png" ? "image/png" : "image/jpeg",
                        UserId = 263,
                        TenantId = 3,
                        Id = Path.GetFileName(file).Replace(ext, "")
                    };

                    _ossDbContext.Files.Add(model);

                    _ossDbContext.SaveChanges();
                }
            }

        }

        public bool IsAllowUploadExtension(string filePath, string key)
        {
            var fileExt = Path.GetExtension(filePath);

            return string.IsNullOrEmpty(fileExt) || Array.IndexOf(((string)extTable[key]).Split(','), fileExt.Substring(1).ToLower()) == -1;
        }

        public OptResult IsContentLength(long length, string key)
        {
            if (length > _settings.FileMaxLength*1024*1024 && key != "image")
                return OptResult.Failed("上传文件大小超过限制30M");

            if (length > _settings.ImageMaxLength * 1024 * 1024 && key == "image")
                return OptResult.Failed("上传文件大小超过限制1M");

            return OptResult.Success();
        }

        public OptResult Remove(string id)
        {
            var file = _ossDbContext.Files.SingleOrDefault(a => a.Id == id);
            if (file == null || !File.Exists(file.FilePath))
                return OptResult.Failed("文件不存在");

            File.Delete(file.FilePath);

            _ossDbContext.Files.Remove(file);
            _ossDbContext.SaveChanges();

            return OptResult.Success();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <param name="key">上传文件类型</param>
        /// <param name="localUrl">部署站点的url</param>
        /// <returns></returns>
        public UploadOutput Upload(IFormFile file, string key, string localUrl)
        {
            var fileModel = Add(file.FileName, file.ContentType, file.Length, key);

            string saveUrl = $"{localUrl}/{ fileModel.Id }";

            using (var fileStream = new FileStream(fileModel.FilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Close();
            }
            return new UploadOutput(fileModel.Name, fileModel.Id, saveUrl);
        }
    }
}
