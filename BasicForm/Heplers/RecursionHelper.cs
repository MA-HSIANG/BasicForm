using BasicForm.Model.Dtos;
using BasicForm.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Heplers
{
    public class RecursionHelper
    {
        public static void AppendChildrenMenu(List<MenuDTO> allMenu, MenuDTO parent)
        {
            var children = allMenu.Where(x => x.ParentId == parent.Id).ToList();

            if (children.Count > 0)
            {
                parent.Sub = children;

                foreach (var child in children)
                {
                    AppendChildrenMenu(allMenu, child);
                }
            }
            else
            {
                parent.Sub = [];
            }
        }
        public static Type GetPageType(string key)
        {
            Dictionary<string, Type> dict = new Dictionary<string, Type>();
            dict.Add("SystemManager", typeof(Home));
            dict.Add("UserManager", typeof(UserManager));
            dict.Add("RoleManager", typeof(RoleManager));
            dict.Add("MenuManager", typeof(MenuManager));
            dict.Add("TestControl", typeof(TestControl));

            var d = dict[key];

            return d;
        }
    }

}
