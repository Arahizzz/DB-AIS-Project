using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBAIS.SQL
{
    public class VadymQueries
    {

        public static readonly string VADYM_QUERY_1 = @"SELECT
                e.empl_surname,
                c.print_date,
                c.check_number,
                p.product_name,
                s.product_number,
                s.selling_price * s.product_number AS price_sum
               FROM employee E
               INNER JOIN ""Check"" c ON e.id_employee = c.id_employee
               INNER JOIN sale s ON c.check_number = s.check_number
               INNER JOIN store_product sp ON s.UPC = sp.UPC
               INNER JOIN product p ON sp.id_product = p.id_product
               WHERE e.id_employee = @id AND e.role = 'cashier'; ";

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

		public static readonly string VADYM_QUERY_3 = @"
SELECT ee.empl_surname, ee.empl_name, ee.empl_patronymic, res.quantity as sold
FROM employee ee
INNER JOIN
(
	SELECT employees_of_month.id_employee, employees_of_month.quantity
	FROM (
		SELECT e.id_employee, SUM(s.product_number) AS quantity
		FROM employee e
		INNER JOIN ""Check"" c ON e.id_employee=c.id_employee
		INNER JOIN sale s ON c.check_number=s.check_number
		WHERE e.role = 'cashier' AND EXTRACT(MONTH FROM print_date) = @month
		GROUP BY e.id_employee) employees_of_month
	ORDER BY employees_of_month.quantity DESC LIMIT 1
) res ON ee.id_employee=res.id_employee;
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
