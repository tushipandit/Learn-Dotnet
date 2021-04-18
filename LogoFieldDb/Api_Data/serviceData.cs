using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Api_Data
{
    public class serviceData
    {

        LogoFieldDbEntities db = new LogoFieldDbEntities();

        public List<servicesML> getservices()
        {
            try
            {
                List<servicesML> ser = (from li in db.services
                                        select new servicesML
                                        {

                                            id = li.id,
                                            Name = li.Name

                                        }).ToList();

                return (ser);

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}