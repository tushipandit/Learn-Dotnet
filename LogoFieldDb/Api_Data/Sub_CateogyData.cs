using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Api_Data
{
    public class Sub_CateogyData
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();
        public List<Sub_CategoryML> GetSub_Category()
        {
            try
            {

                List<Sub_CategoryML> scat = (from li in db.Sub_Category
                                             select new Sub_CategoryML
                                             {
                                                 Id = li.Id,
                                                 Name = li.Name,
                                                 Description = li.Description
                                             }
                                             ).ToList();
                return scat;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}