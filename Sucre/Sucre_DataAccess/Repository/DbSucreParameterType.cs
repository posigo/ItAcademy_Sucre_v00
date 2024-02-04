using Microsoft.AspNetCore.Mvc.Rendering;
using Sucre_DataAccess.Data;
using Sucre_DataAccess.Entities;
using Sucre_DataAccess.Repository.IRepository;

namespace Sucre_DataAccess.Repository
{
    public class DbSucreParameterType : DbSucre<ParameterType, int>, IDbSucreParameterType
    {
        private readonly ApplicationDbContext _db;

        public DbSucreParameterType(ApplicationDbContext db): base(db)
        {
            _db = db;
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
                //"--Select parameter type--"
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
            ParameterType parameterType = (ParameterType)obj;
            List<string> listText = new List<string>();
            if (parameterType.Name != null && parameterType.Name.Trim() != "")
                listText.Add(parameterType.Name);
            if (parameterType.Mnemo != null && parameterType.Mnemo.Trim() != "")
                listText.Add(parameterType.Mnemo);
            if (parameterType.UnitMeas != null && parameterType.UnitMeas.Trim() != "")
                listText.Add(parameterType.UnitMeas);
            return String.Join(" ", listText.ToArray());
        }

        public void Update(ParameterType parameterType)
        {
            var objFromDb = base.FirstOrDefault(pt => pt.Id == parameterType.Id);
            if (objFromDb != null) 
            {
                objFromDb.Name = parameterType.Name;
                objFromDb.Mnemo = parameterType.Mnemo;
                objFromDb.UnitMeas = parameterType.UnitMeas;
            }            
        }
        public async Task UpdateAsync(ParameterType parameterType)
        {
            await Task.Run(() => Update(parameterType));
        }
    }
}
