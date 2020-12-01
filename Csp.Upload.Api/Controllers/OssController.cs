
using Csp.Upload.Api.Application.Services;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var file = _fileSerivce.Get(id,width);

            if (file == null)
                return BadRequest(OptResult.Failed("文件不存在"));

            return File(file.Bytes, file.ContentType);
        }

        [HttpGet, Route("oss/{id}")]
        public IActionResult Index(string id)
        {
            var fileOutput = _fileSerivce.Get(id);
            if(fileOutput == null )
                return BadRequest(OptResult.Failed("文件不存在"));

            return File(fileOutput.Bytes, fileOutput.ContentType);
        }

        [Authorize]
        [HttpPost, Route("oss/upload")]
        public IActionResult Upload(IFormFile file, string key = "image")
        {
            if (file != null)
            {
                if (string.IsNullOrEmpty(key))
                    key = "image";

                var result = _fileSerivce.IsContentLength(file.Length, key);
                if (!result.Succeed)
                    return BadRequest(result);

                if (_fileSerivce.IsAllowUploadExtension(file.FileName, key))
                    return BadRequest(OptResult.Failed("上传文件扩展名是不允许的扩展名\n只允许" + _fileSerivce.GetAllowExtension(key) + "格式。"));

                var uploadInfo = _fileSerivce.Upload(file, key, $"{Request.GetDomain()}/oss");
                return Ok(OptResult.Success(uploadInfo.Url));

            }

            return BadRequest(OptResult.Failed("请选择文件"));
        }

        [Authorize]
        [HttpPost, Route("oss/ant/upload")]
        public IActionResult AntUpload(IFormFile file, string key = "image")
        {
            if (file != null)
            {
                if (string.IsNullOrEmpty(key))
                    key = "image";

                var result = _fileSerivce.IsContentLength(file.Length, key);
                if (!result.Succeed)
                    return BadRequest(result);

                if (_fileSerivce.IsAllowUploadExtension(file.FileName, key))
                    return BadRequest(OptResult.Failed("上传文件扩展名是不允许的扩展名\n只允许" + _fileSerivce.GetAllowExtension(key) + "格式。"));

                var uploadInfo = _fileSerivce.Upload(file, key, $"{Request.GetDomain()}/oss");
                return Ok(uploadInfo);

            }

            return BadRequest(OptResult.Failed("请选择文件"));
        }

        /// <summary>
        /// ckeditor编辑的用的simple-upload后台上传代码
        /// </summary>
        /// <param name="upload">待上传的文件</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost, Route("oss/cke/upload")]
        public IActionResult Upload(IFormFile upload)
        {
            string key = "image";
            if (upload != null)
            {
                var result = _fileSerivce.IsContentLength(upload.Length, key);
                if (!result.Succeed)
                    return BadRequest(new { Uploaded = false, Error = new { Message = result.Msg } }); 

                if (_fileSerivce.IsAllowUploadExtension(upload.FileName, key))
                    return BadRequest(new { Uploaded = false, Error = new { Message = "上传文件扩展名是不允许的扩展名\n只允许" + _fileSerivce.GetAllowExtension(key) + "格式。" } });

                var uploadInfo = _fileSerivce.Upload(upload, key, $"{Request.GetDomain()}/oss");
                return Ok(new { Uploaded = true, uploadInfo.Url });
            }

            return BadRequest(new { Uploaded = false, Error = new { Message = "请选择文件" } });
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost,Route("oss/delete/{id}")]
        public IActionResult Remove(string id)
        {
            return Ok(_fileSerivce.Remove(id));
        }


        [Authorize]
        [Route("oss/import/img")]
        public IActionResult ImportImage()
        {
            _fileSerivce.ImportImage();

            return Ok(OptResult.Success());
        }
    }
}
