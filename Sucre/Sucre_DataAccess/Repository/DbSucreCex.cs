using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_Core.LoggerExternal;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreCex : DbSucre<Cex, int>, IDbSucreCex
    {
        private readonly ApplicationDbContext _db;

        public DbSucreCex(ApplicationDbContext db): base(db)
        {
            _db = db;
            LoggerExternal.LoggerEx.Information($"*->Use IRepository DbSucreCex-{DateTime.Now.ToShortTimeString()}");
        }

        public bool CheckAllFieldsEmpty (Cex cex)
        {
            if ((cex.Management.Trim() == string.Empty || cex.Management.Trim() == "") &&
                (cex.CexName.Trim() == string.Empty || cex.CexName.Trim() == "") &&
                (cex.Area.Trim() == string.Empty || cex.Area.Trim() == "") &&
                (cex.Device.Trim() == string.Empty || cex.Device.Trim() == "") &&
                (cex.Location.Trim() == string.Empty || cex.Location.Trim() == ""))
            { 
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить список элементов объекта
        /// </summary>
        /// <param name="AddFirstSelect">Добавить первый элемент</param>
        /// <param name="valueFirstSelect">Название первого элемента</param>
        /// <returns>список элементов</returns>
        public IEnumerable<SelectListItem> GetAllDropdownList(bool addFirstSelect = true, string valueFirstSelect = null)
        {
            List<SelectListItem> returnValues = new List<SelectListItem>();
            SelectListItem value;
            
            if (addFirstSelect)
            {
                //value.Text = "--Select metering point location--";
                //"--Select the location of the metering point--"
                value = new SelectListItem(
                    text: (valueFirstSelect == null || valueFirstSelect.Trim() == "") ? 
                            "--Select from the list--" : 
                            valueFirstSelect,
                    value: "0",
                    selected: true, 
                    disabled: true);

                returnValues.Add(value);

            }                      
            
            foreach (var item in dbSet)
            {
                value = new SelectListItem();
                string textValue = GetStringName(item);
                value.Text = textValue;
                value.Value = item.Id.ToString();
                returnValues.Add(value);
            };
            
            return returnValues;
        }

        /// <summary>
        /// Получить имя объекта из полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns>имя объекта</returns>
        public string GetStringName(object obj)
        {
            Cex cex = (Cex)obj;
            List<string> listText = new List<string>();
            if (cex.Management != null && cex.Management.Trim() != "")
                listText.Add(cex.Management);
            if (cex.CexName != null && cex.CexName.Trim() != "")
                listText.Add(cex.CexName);
            if (cex.Area != null && cex.Area.Trim() != "")
                listText.Add(cex.Area);
            if (cex.Device != null && cex.Device.Trim() != "")
                listText.Add(cex.Device);
            if (cex.Location != null && cex.Location.Trim() != "")
                listText.Add(cex.Location);
            return String.Join("->", listText.ToArray());
        }

        public void Update(Cex cex)
        {
            var objFromDb = base.FirstOrDefault(item => item.Id == cex.Id);
            if (objFromDb != null) 
            {
                objFromDb.Area = cex.Area;
                objFromDb.Device = cex.Device;
                objFromDb.CexName = cex.CexName;
                objFromDb.Location = cex.Location;
                objFromDb.Management = cex.Management;


            }
        }
        public async Task UpdateAsync(Cex cex)
        {
            await Task.Run(() => Update(cex));
        }
    }
}
