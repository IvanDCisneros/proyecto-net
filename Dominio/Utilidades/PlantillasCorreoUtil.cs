using System.Reflection;
using System.Text;

namespace Dominio.Utilidades
{
    public static class PlantillasCorreoUtil
    {
        public static string MergeInfoPlantilla<T>(T entidad, string plantilla)
        {
            string result = plantilla;

            PropertyInfo[] myPropertyInfo;

            myPropertyInfo = entidad!.GetType().GetProperties();

            for (int i = 0; i < myPropertyInfo.Length; i++)
            {
                result = result.Replace("$" + myPropertyInfo[i].Name, myPropertyInfo[i].GetValue(entidad, null)?.ToString());
            }

            return result;
        }

        public static string MergeListPlantilla<T>(IEnumerable<T> Datos, string plantilla, string replaceToken)
        {
            string result = plantilla;

            StringBuilder rows = new StringBuilder();

            foreach (var item in Datos)
            {
                rows.AppendLine("<tr>");

                var myPropertyInfo = item!.GetType().GetProperties();

                for (int i = 0; i < myPropertyInfo.Length; i++)
                    rows.AppendFormat("<td>{0}</td>", myPropertyInfo[i].GetValue(item, null)?.ToString());

                rows.AppendLine("</tr>");
            }

            result = result.Replace(replaceToken, rows.ToString());

            return result;
        }
    }
}
