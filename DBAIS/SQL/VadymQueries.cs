using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAIS.SQL
{
    public class VadymQueries
    {
        public static readonly string VADYM_QUERY_2 = @"
SELECT
res.year,
TO_CHAR(
	TO_DATE (res.month::text, 'MM'), 'Month'
) AS month,
res.category_name,
res.quantity as quantity
FROM (
	(
	SELECT grouped_by_month.year, grouped_by_month.month, grouped_by_month.category_number, SUM(grouped_by_month.product_number) AS quantity
	FROM (
		SELECT EXTRACT(YEAR FROM print_date) AS year,EXTRACT(MONTH FROM print_date) AS month, p.category_number, s.product_number
			FROM ""Check"" c
			INNER JOIN sale s ON c.check_number=s.check_number
            INNER JOIN store_product sp ON s.UPC = sp.UPC

            INNER JOIN product p ON sp.id_product = p.id_product
        ) grouped_by_month
    GROUP BY grouped_by_month.year, grouped_by_month.month, grouped_by_month.category_number
) grouped_by_category
INNER JOIN category ca ON grouped_by_category.category_number = ca.category_number) res
ORDER BY year, month, quantity DESC;
            ";

        public static readonly string VADYM_QUERY_4 = @"
SELECT check_number
FROM ""Check""
WHERE check_number NOT IN(
  SELECT c.check_number
  FROM ""Check"" c
  INNER JOIN sale s ON c.check_number= s.check_number
  INNER JOIN store_product sp ON s.upc= sp.upc
  WHERE sp.upc NOT IN(
    SELECT DISTINCT upc
    FROM store_product
    WHERE promotional_product = TRUE
  )
);
            ";
    }
}
