using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Totem.Business.Core.Consts
{
    public static class Constants
    {
        public static string GetExpenseSummaryDetail = "GetExpenseSummaryDetail";
        public static string GetEvaluationCaseSummary = "SP_GetEvaluationCaseSummary";
        public static string ParamAccountId = "@Id";

        public static string SP_GetQuestionsList = "SP_GetQuestionsList";
        public static string SP_GetOptionsbyQueIdandCatId = "SP_GetOptionsbyQueIdandCatId";
        public static string ParamQuestionId = "@QuestionId";
        public static string ParamCategoryId = "@CategoryId";

        public static string SP_GetQuestionsListLatest = "SP_GetQuestionsListLatest";


        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        catch (Exception)
                        {
                           
                        }
                       
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
