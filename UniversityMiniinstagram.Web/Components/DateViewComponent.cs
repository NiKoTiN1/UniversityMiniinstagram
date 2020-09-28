using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityMiniinstagram.Web.Components
{
    public class DateViewComponent: ViewComponent
    {
        public string Invoke()
        {
            return $"Текущая дата: {DateTime.Now.ToString("d")}";
        }
    }
}
