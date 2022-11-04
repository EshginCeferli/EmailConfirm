using Asp.Net_PartialViews.Models;
using System.Collections.Generic;

namespace Asp.Net_PartialViews.ViewModels
{
    public class AccessoriesVM
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
