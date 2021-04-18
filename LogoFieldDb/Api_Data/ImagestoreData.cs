using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Api_Data
{
    public class ImagestoreData
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();

        public List<ImagestoreML> GetImagestore()
        {
            try
            {
                List<ImagestoreML> iml = (from li in db.Imagestores
                                          select new ImagestoreML
                                          {
                                              Id = li.Id,
                                              Name = li.Name
                                          }
                                          ).ToList();
                return iml;

            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }


    }

  

}