using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicForm.Heplers
{
    public static class FormBindingHepler
    {
        public static void BindControl(Control control,object dataSource,string propertyName)
        {
            if (control == null || dataSource == null || string.IsNullOrEmpty(propertyName)) return;

            string controlProperty = control switch
            {
                TextBox => "Text",
                CheckBox => "Checked",
                ComboBox => "SelectedValue",
                DateTimePicker => "Value",
                //依照自己套件命名規則去擴展
                AntdUI.Input =>"Text",
                AntdUI.Checkbox =>"Checked",
               _ => null
            };


            if (controlProperty == null)
                throw new NotSupportedException($"控制項類型不支援：{control.GetType().Name}");

            // 加入雙向綁定
            control.DataBindings.Add(controlProperty, dataSource, propertyName, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public static void BindAllControl(Control parent, object dataSource)
        {
            foreach (Control control in parent.Controls)
            {
             
                if (string.IsNullOrWhiteSpace(control.Name))
                {
                    if (control.HasChildren)
                        BindAllControl(control, dataSource);
                    continue;
                }

                var propName = control.Name
                    .Replace("txt", "")
                    .Replace("chk", "")
                    .Replace("cmb", "")
                    .Replace("dtp", "")
                    .Replace("lbl", "");

                if (dataSource.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    BindControl(control, dataSource, propName);
                }

        
                if (control.HasChildren)
                    BindAllControl(control, dataSource);
            }
        }
    }
}
