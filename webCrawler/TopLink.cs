using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCrawler
{
    public class TopLink
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public List<ChildLink> ChildLinks { get; set; }
    }
}
