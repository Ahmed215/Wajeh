using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using Wajeeh.Subcategories;

namespace Wajeeh.Categories
{
    public class Category : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
