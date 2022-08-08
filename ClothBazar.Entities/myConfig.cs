using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothBazar.Entities
{
 //   the follwing class will have a list of attributes related to Site this will be info shown on the HomePage its like the
 //related facebook/twitter account site contactNo or various headings like topDeals, summerArrivals e.t.c these values
 //should not be hardcoded their values may be changed in future or we may be saling this Website to various clients
 //so for such things(Links/Heading) we create a class traditionally named as Config.cs we named as myConfig in Entities Project so its table will
 //   be created and we store data/info we want to display and later on we can also change this info by accessing this class.
 //   but if we create a seperate attribute for each info i.e for related faceBook account we careted a if we carete an attribute here like
 //   "FBaccount" in table it will have only one value always all other attributes like this will have only one value so the table will have only one row
 //   so to avoid this we created two attributs in this class "key" and "value" both string key will have name like "FBaccount" and
 //   "Value" will have its realted value i.e facbook account link..
    public class myConfig
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
