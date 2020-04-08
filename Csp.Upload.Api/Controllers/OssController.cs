
using Csp.Upload.Api.Application.Services;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;

namespace Csp.Upload.Api.Controllers
{
    //[Route("api/v1/[controller]")]
    public class OssController : ControllerBase
    {

        private readonly IFileService _fileSerivce;
        

        public OssController( IFileService fileService)
        {
            _fileSerivce = fileService;
        }

        [HttpGet,Route("oss/{id}/{width:int}")]
        public IActionResult Index(string id,int width)
        {
            var file = _fileSerivce.GetFilePathById(id);

            if (file == null || !System.IO.File.Exists(file.FilePath))
                return BadRequest(OptResult.Failed("文件不存在"));

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
            return File(bytes, file.ContentType);
        }

        [HttpGet, Route("oss/{id}")]
        public IActionResult Index(string id)
        {
            var file = _fileSerivce.GetFilePathById(id);


            if (file==null || !System.IO.File.Exists(file.FilePath))
                return BadRequest(OptResult.Failed("文件不存在"));

            using var sw = new FileStream(file.FilePath, FileMode.Open);
            var bytes = new byte[sw.Length];
            sw.Read(bytes, 0, bytes.Length);
            sw.Close();
            return File(bytes, file.ContentType);
        }

        [Authorize]
        [HttpPost, Route("oss/upload")]
        public IActionResult Upload(IFormFile file, string key = "image")
        {
            string saveUrl;
            if (file != null)
            {
                if (string.IsNullOrEmpty(key))
                    key = "image";

                var result = _fileSerivce.IsContentLength(file.Length, key);
                if (!result.Succeed)
                    return BadRequest(result);

                if (_fileSerivce.IsAllowUploadExtension(file.FileName, key))
                    return BadRequest(OptResult.Failed("上传文件扩展名是不允许的扩展名\n只允许" + _fileSerivce.GetAllowExtension(key) + "格式。"));

                var fileModel = _fileSerivce.Add(file.FileName, file.ContentType, file.Length, key);

                saveUrl = $"{Request.GetDomain()}{Url.Action(nameof(Index).ToLower(), new { id = fileModel.Id })}";

                using (var fileStream = new FileStream(fileModel.FilePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    fileStream.Close();
                }
                return Ok(OptResult.Success(saveUrl));
            }

            return BadRequest(OptResult.Failed("请选择文件"));
        }
    }
}
