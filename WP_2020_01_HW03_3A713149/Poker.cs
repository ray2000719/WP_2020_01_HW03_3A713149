using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP_2020_01_HW03_3A713149
{
    class Poker
    {
        public List<int> GetPoker(int count)
        {
            List<int> result = new List<int>();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    result.Add(i);
                }
                for (int i = count - 2; i > 0; i--)
                {
                    int lastIndex = i + 1;
                    int randomIndex = (new Random().Next(i));

                    int temp = result[lastIndex];
                    result[lastIndex] = result[randomIndex];
                    result[randomIndex] = temp;

                }

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                throw;
            }
            return result;
        }
    }
}
