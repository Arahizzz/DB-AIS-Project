INSERT INTO public.category (category_number, category_name)
VALUES (default, 'Fruits');
INSERT INTO public.category (category_number, category_name)
VALUES (default, 'Vegetables');
INSERT INTO public.category (category_number, category_name)
VALUES (default, 'Clothing');
INSERT INTO public.category (category_number, category_name)
VALUES (default, 'Sweets');

INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (1, 'Apple', 'Green');
INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (1, 'Banana', 'From Africa');
INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (2, 'Tomato', 'Red');
INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (3, 'Jacket', 'Men''s jacket');
INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (4, 'Chocolate', 'Roshen');
INSERT INTO public.product (category_number, product_name, characteristics)
VALUES (4, 'Marmelade', 'From Belgium');

INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('502867184634', null, 1, 20.0000, 350, false);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('752320678217', null, 2, 55.0000, 125, false);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('926097294137', null, 3, 25.0000, 400, false);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('305210625843', null, 4, 2000.0000, 20, true);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('636541812428', '305210625843', 4, 2500.0000, 0, false);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('455217613717', null, 5, 110.0000, 55, false);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('359194108878', null, 6, 60.0000, 35, true);
INSERT INTO public.store_product (upc, upc_prom, id_product, selling_price, products_number, promotional_product)
VALUES ('401094100243', '359194108878', 6, 80.0000, 0, false);

INSERT INTO public.customer_card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street,
                                  zip_code, percent)
VALUES ('2827353407381', 'Petrenko', 'Ivan', 'Ivanovych', '+380953215535', 'Kyiv', 'Kreschatyk', '01001', 3);
INSERT INTO public.customer_card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street,
                                  zip_code, percent)
VALUES ('0185586120586', 'Avramov', 'Petro', 'Mykolayovych', '+380934532357', null, null, null, 2);
INSERT INTO public.customer_card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street,
                                  zip_code, percent)
VALUES ('7121520341739', 'Teslia', 'Anna', 'Vitaliyivna', '+380505785789', 'Odesa', null, '02346', 2);
INSERT INTO public.customer_card (card_number, cust_surname, cust_name, cust_patronymic, phone_number, city, street,
                                  zip_code, percent)
VALUES ('3836104182447', 'Lutsenko', 'Kateryna', 'Mykhayilivna', '+380345779809', 'Lviv', 'Svobody', '02456', 3);

-- Employee
INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id1', 'Davydova', 'Olena', 'Vasylivna', 'cashier', 1500, '1974-05-22', '2006-04-30', '0688856321', 'Kyiv', 'Street1', 1001, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id2', 'Zhukovskaya', 'Olga', 'Nikolaevna', 'manager', 3400, '1981-02-27', '2005-01-23', '0688855321', 'Kyiv', 'Street2', 1002, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id3', 'Kipelov', 'Anton', 'Valerievich', 'cashier', 1603, '1966-04-07', '2000-04-30', '0688854521', 'Kyiv', 'Street3', 1003, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id4', 'Boychuk', 'Stanislav', 'Petrovich', 'cashier', 1606, '1971-11-11', '2003-01-10', '0686666321', 'Kyiv', 'Street4', 1004, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id5', 'Durnov', 'Anton', 'Valeriyovych', 'cashier', 1808, '1977-07-06', '2000-04-30', '0688858721', 'Kyiv', 'Street5', 1005, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id6', 'Boychenko', 'Lyudmila', 'Vasylivna', 'cashier', 1900, '1966-10-6', '2011-07-27', '0689856321', 'Lviv', 'Street6', 1006, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id7', 'Uwu', 'Mandy', 'Petrovich', 'manager', 3250, '1974-05-22', '2011-06-12', '0688856671', 'Lviv', 'Street7', 1007, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id8', 'Danilko', 'Brad', 'Olehovych', 'cashier', 1880, '1974-05-22', '2011-05-11', '0688776321', 'Lviv', 'Street8', 1008, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id9', 'Hanover', 'Mike', 'Denysovych', 'cashier', 1444, '1980-01-21', '2011-04-10', '0689056321', 'Lviv', 'Street9', 1009, 'password');

INSERT INTO employee (id_employee, empl_surname, empl_name, empl_patronymic, "role", salary, date_of_birth, date_of_start,phone_number,city,street,zip_code,password)
VALUES ('id10', 'Stuart', 'Jeremy', 'Tarasovych', 'cashier', 1310, '1979-03-24', '2011-03-01', '0663356321', 'Lviv', 'Street10', 1010, 'password');


INSERT INTO public."Check" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('2428784833', 'id1', '2827353407381', '2021-04-15', 119.0000, 22.0000);
INSERT INTO public."Check" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('9546921356', 'id3', '7121520341739', '2021-04-01', 265.2000, 30.0000);
INSERT INTO public."Check" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('2428784834', 'id6', null, '2021-04-07', 76.0000, 19.0000);
INSERT INTO public."Check" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('9546921358', 'id1', '7121520341739', '2021-04-06', 105.0000, 5.0000);
INSERT INTO public."Check" (check_number, id_employee, card_number, print_date, sum_total, vat) VALUES ('9546921357', 'id3', '7121520341739', '2021-04-06', 2100.0000, 20.0000);
INSERT INTO public."Check" (check_number,id_employee,card_number,print_date,sum_total,vat)
VALUES ('100000000','id1','2827353407381','2021-04-04',200,2.5); -- 10 apples

INSERT INTO public."Check" (check_number,id_employee,card_number,print_date,sum_total,vat)
VALUES ('100000002','id1','2827353407381','2021-04-01',265,2.5); -- 5 apples 3 bananas (165)

INSERT INTO public."Check" (check_number,id_employee,card_number,print_date,sum_total,vat)
VALUES ('100000003','id1','0185586120586','2021-03-05',2000,2.5); -- Man`s Jacket

INSERT INTO public."Check" (check_number,id_employee,card_number,print_date,sum_total,vat)
VALUES ('100000004','id1','0185586120586','2021-03-20',360,2.5); -- 6 marmelades

INSERT INTO public."Check" (check_number,id_employee,card_number,print_date,sum_total,vat)
VALUES ('100000005','id3','3836104182447','2021-02-20',2700,2.5); -- 1 jacket 3 chocolate bars

-- SALE
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('502867184634', '2428784833', 5, 20.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('502867184634', '9546921356', 3, 20.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('359194108878', '9546921356', 3, 60.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('502867184634', '2428784834', 3, 20.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('502867184634', '9546921358', 5, 20.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('305210625843', '9546921357', 1, 2000.0000);
INSERT INTO public.sale (upc, check_number, product_number, selling_price) VALUES ('502867184634', '9546921357', 4, 20.0000);

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('502867184634','100000000',10,20); -- 10 apples

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('502867184634','100000002',5,20); -- 5 apples

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('752320678217','100000002',3,55); -- 3 bananas

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('305210625843','100000003',1,2000); -- 1 jacket

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('359194108878','100000004',6,60);-- 6 marmelades

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('636541812428','100000005',1,2500);-- 6 jacket

INSERT INTO public.sale (UPC,check_number,product_number,selling_price)
VALUES ('455217613717','100000005',3,110);-- 3 chocolate bars
