using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreAppRole : DbSucre<AppRole, Guid>, IDbSucreAppRole
    {
        private readonly ApplicationDbContext _db;

        public DbSucreAppRole(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        //public IEnumerable<SelectListItem> GetAllDropdownList(string strInclude)
        //{            
        //    List<SelectListItem> returnValues = new List<SelectListItem>();
        //    SelectListItem value = new SelectListItem();             
        //    if (strInclude == WC.EnergyName)
        //    {
        //        value.Text = "--Select the energy type--";
        //    }
        //    if (strInclude == WC.CexName)
        //    {
        //        value.Text = "--Select the location--";
        //    }            
        //    value.Value = "0";
        //    value.Disabled = true;
        //    value.Selected = true;
        //    returnValues.Add(value);
        //    if (strInclude == WC.EnergyName)
        //    {
        //        foreach (var item in _db.Energies)
        //        {   
        //            value = new SelectListItem();
        //            value.Text = item.EnergyName;
        //            value.Value = item.Id.ToString();
        //            returnValues.Add(value);
        //        }

        //        //return _db.Energies.Select(item => new SelectListItem
        //        //{
        //        //    Text = item.EnergyName,
        //        //    Value = item.Id.ToString()
        //        //});
        //    }
        //    if (strInclude == WC.CexName)
        //    {
        //        //List<SelectListItem> returnValues= new List<SelectListItem>();

        //        foreach (var item in _db.Cexs)
        //        {
        //            value = new SelectListItem();
        //            //List<string> listText = new List<string>();                    
        //            //if (item.Management !=null && item.Management.Trim() !="")
        //            //    listText.Add(item.Management);
        //            //if (item.CexName != null && item.CexName.Trim() != "")
        //            //    listText.Add(item.CexName);
        //            //if (item.Area != null && item.Area.Trim() != "")
        //            //    listText.Add(item.Area);
        //            //if (item.Device != null && item.Device.Trim() != "")
        //            //    listText.Add(item.Device);
        //            //if (item.Location != null && item.Location.Trim() != "")
        //            //    listText.Add(item.Location);
        //            //string textValue = String.Join("->", listText.ToArray());
        //            //string textValue = GetStringCex(item);


        //            string textValue = GetStringName(item);
        //            value.Text = textValue;
        //            value.Value = item.Id.ToString();
        //            returnValues.Add(value);
        //        };                
        //    }
        //    return returnValues;
        //}


        //public string GetStringName(object obj)
        //{
        //    Cex cex = (Cex)obj;
        //    List<string> listText = new List<string>();
        //    if (cex.Management != null && cex.Management.Trim() != "")
        //        listText.Add(cex.Management);
        //    if (cex.CexName != null && cex.CexName.Trim() != "")
        //        listText.Add(cex.CexName);
        //    if (cex.Area != null && cex.Area.Trim() != "")
        //        listText.Add(cex.Area);
        //    if (cex.Device != null && cex.Device.Trim() != "")
        //        listText.Add(cex.Device);
        //    if (cex.Location != null && cex.Location.Trim() != "")
        //        listText.Add(cex.Location);
        //    return String.Join("->", listText.ToArray());

        //}

        public void Update(AppRole appRole)
        {
            //_db.Update(appRole);
            var objFromDb = base.FirstOrDefault(item => item.Id == appRole.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = appRole.Name;
                objFromDb.Value = appRole.Value;
            }
        }
    }
}
