using Csp.EF;

namespace Csp.SystemSet.Api.Models
{
    public class Carousel : Entity
    {
        /// <summary>
        /// 轮播图编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 轮播图名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 轮播图地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 轮播图排序   越大越往后
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态 1：使用中 0：删除 2：弃用
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 所属编号Id
        /// </summary>
        public int WebSiteId { get; set; }

        /// <summary>
        /// 所属租户
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 管理人
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public void Update(string name,string url,int sort)
        {
            Name = name;
            Url = url;
            Sort = sort;
        }


        public void Enabled()
        {
            Status = 1;
        }


        public void Disabled()
        {
            Status = 2;
        }


        public void Deleted()
        {
            Status = 0;
        }
    }
}
