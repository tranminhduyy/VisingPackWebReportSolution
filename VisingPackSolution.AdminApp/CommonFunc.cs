using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisingPackSolution.AdminApp
{
    public class CommonFunc
    {
        protected CommonFunc()
        {

        }

        private static CommonFunc _instance;
        public static CommonFunc Instance()
        {
            if (_instance == null)
            {
                _instance = new CommonFunc();
            }
            return _instance;
        }
        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
