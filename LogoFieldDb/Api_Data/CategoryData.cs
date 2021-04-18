using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogoFieldDb.Api_Data
{
    public class CategoryData
    {

        LogoFieldDbEntities db = new LogoFieldDbEntities();


        public List<CategoryML> GetCategory()
        {

            try
            {

                List<CategoryML> cat = (from li in db.Categories.Where(a=> a.IsAlive == true)
                                        join sc in db.Sub_Category.Where(a => a.IsAlive == true) on li.Id equals sc.CategoryId
                                        join s in db.services.Where(a => a.IsAlive == true) on sc.Id equals s.SubCatId
                                        select new CategoryML
                                        {
                                            Id = li.Id,
                                            Name = li.Name,
                                            Description = li.Description
                                        }
                                       ).ToList();
                return cat;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public long Postbarval(CategoryML model)
        {
            try
            {


                var result = new Category();
                result.Name = model.Name;
                result.Description = model.Description;
                result.createdon = DateTime.Now;
                db.Categories.Add(result);
                db.SaveChanges();

                var id = result.Id;
                return id;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            
        }




        public List<CategoryML> Getbarval()
        {
            try
            {

                List<CategoryML> sc = new List<CategoryML>();

                var cat = (from li in db.Categories
                           select new CategoryML
                           {
                               Id = li.Id,
                               Name = li.Name
                           }
                                       ).ToList();

                foreach (var r in cat)
                {

                    List<Sub_CategoryML> subcat = new List<Sub_CategoryML>();

                    var rescat = (from li in db.Sub_Category.Where(x => x.IsAlive == true && x.CategoryId == r.Id)
                                  select new Sub_CategoryML
                                  {
                                      Id = li.Id,
                                      Name = li.Name
                                  }
                      ).ToList();


                    foreach (var ri in rescat)
                    {

                        var res = (from li in db.services.Where(x => x.IsAlive == true && x.SubCatId == ri.Id)
                               select new servicesML
                               {
                                   id = li.id,
                                   Name = li.Name
                               }).ToList();

                        subcat.Add(new Sub_CategoryML
                        {
                            Id = ri.Id,
                            Name = ri.Name,
                            ServicesML = res

                        });

                        sc.Add(new CategoryML
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Sub_Category = subcat

                        });
                    }
                }


                return sc;

            }

            catch (Exception ex)
            {
                throw ex;
            }



        }

    }
}


               



 



    
        //public List<CategoryML>GetCategoryML()
        //{
        //    try
        //    { 

        //        List<CategoryML> sc= new List<CategoryML>();

        //        var result = (from li in db.Categories.Where(x => x.IsAlive == true && x.Id == 0)
        //                      select new CategoryML
        //                      {
        //                          Id = li.Id,
        //                          Name = li.Name,

        //                      }

        //                      ).ToList();

        //        foreach(var r in result)
        //        {
        //            List<Sub_CategoryML> subCat = new List<Sub_CategoryML>();

        //            var res = (from li in db.Sub_Category.Where(x => x.IsAlive == true && x.Id==r.)
        //                       select new Sub_CategoryML
        //                       {
        //                           Id = li.id,
        //                           Name = li.Name
        //                       }

        //                     ).OrderBy(o => o.position).ToList();

        //            var findCat = res.Select(a => a.Id).ToList();
        //            //var items=db.services.Where(y=>findCat.Contains.Where(y=>findCat.Contains((long)y.Categorie)).ToList();

        //            var s = new List<serviceML>();
        //            foreach(var i in res)
        //            {
        //                s = (from sli in db.services.Where(a1 => a1.id == i.id)

        //                     select new serviceML
        //                     {
        //                         id = sli.id,
        //                         Name = sli.Name
        //                     }
        //                   ).ToList();

        //                subCat.Add(new Sub_CategoryML
        //                {
        //                    id = r.Id,
        //                    Name = r.Name,
        //                    sub_category = subCat
        //                });
        //            }

        //            return sc;

        //        }
        //        catch(Exception ex)
        //    {
        //        throw ex;
        //    }



        //    }








