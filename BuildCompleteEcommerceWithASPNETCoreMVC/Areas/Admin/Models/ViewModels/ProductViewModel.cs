using DoAn1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildCompleteEcommerceWithASPNETCoreMVC.Areas.Admin.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public List<SelectListItem> Categorises { get; set; } 
    }
}
