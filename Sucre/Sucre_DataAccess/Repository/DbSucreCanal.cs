using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreCanal : DbSucre<Canal, int>, IDbSucreCanal
    {
        private readonly ApplicationDbContext _db;

        public DbSucreCanal(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        //public IEnumerable<SelectListItem> GetAllDropdownList(string strInclude)
        //{            
        //    if (strInclude == WC.ParameterTypeName)
        //    {
        //        List<SelectListItem> returnValues= new List<SelectListItem>();
        //        bool firstElement = true;
        //        foreach (var item in _db.ParameterTypes)
        //        {                    
        //            SelectListItem value = new SelectListItem();
        //            if (firstElement)
        //            {
        //                value.Text = "--Select the parameter type--";
        //                value.Value = "0";
        //                value.Disabled = true;
        //                value.Selected = true;                        
        //                returnValues.Add(value);
        //                value = new SelectListItem();
        //                firstElement = false;
        //            }
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
        //        return returnValues;
        //    }
        //    return null;
        //}
               
        //public string GetStringName(object obj)
        //{
        //    ParameterType parameterType = (ParameterType)obj;
        //    List<string> listText = new List<string>();
        //    if (parameterType.Name != null && parameterType.Name.Trim() != "")
        //        listText.Add(parameterType.Name);
        //    if (parameterType.Mnemo != null && parameterType.Mnemo.Trim() != "")
        //        listText.Add(parameterType.Mnemo);
        //    if (parameterType.UnitMeas != null && parameterType.UnitMeas.Trim() != "")
        //        listText.Add(parameterType.UnitMeas);            
        //    return String.Join(" ", listText.ToArray());            
        //}
                
        public void Update(Canal canal)
        {
            _db.Update(canal);            
        }
        public async Task UpdateAsync(Canal canal)
        {
            await Task.Run(() => Update(canal));
        }
    }
}
