using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totem.Business.Core.DataTransferModels.WebsiteDesign
{
    public class WebsiteConten
    {
        public string Page { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Section { get; set; }
    }

    public class WebsiteContenID : WebsiteConten
    {
        public int Id { get; set; }
    }
}
