﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081018.DomainModel
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }
        public int ParentCategoryID { get; set; }
    }
}
