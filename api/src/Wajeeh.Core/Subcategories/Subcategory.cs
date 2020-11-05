using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Wajeeh.Categories;
using Wajeeh.Requests;

namespace Wajeeh.Subcategories
{
    public class Subcategory : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string Image { get; set; }
        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public Category Category { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}
