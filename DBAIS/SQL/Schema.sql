CREATE TABLE Category
(
    category_number serial primary key,
    category_name   varchar(50)
);

CREATE TABLE Product
(
    id_product      serial primary key,
    category_number int          not null,
    product_name    varchar(50)  not null,
    characteristics varchar(100) not null,
    foreign key (category_number) references Category (category_number)
        on update cascade
        on delete no action
);

CREATE TABLE Employee
(
    id_employee     varchar(10) primary key,
    empl_surname    varchar(50)    not null,
    empl_name       varchar(50)    not null,
    empl_patronymic varchar(50)    not null,
    role            varchar(10)    not null,
    salary          decimal(13, 4) not null,
    date_of_birth   date           not null,
    date_of_start   date           not null,
    phone_number    varchar(13)    not null,
    city            varchar(50)    not null,
    street          varchar(50)    not null,
    zip_code        varchar(9)     not null
);

CREATE TABLE Customer_Card
(
    card_number     varchar(13) primary key,
    cust_surname    varchar(50) not null,
    cust_name       varchar(50) not null,
    cust_patronymic varchar(50) not null,
    phone_number    varchar(13) not null,
    city            varchar(50),
    street          varchar(50),
    zip_code        varchar(9),
    percent         int         not null
);

CREATE TABLE "Check"
(
    check_number varchar(10) primary key,
    id_employee  varchar(10)    not null,
    card_number  varchar(13),
    print_date   date           not null,
    sum_total    decimal(13, 4) not null,
    vat          decimal(13, 4) not null,
    foreign key (id_employee) references Employee (id_employee)
        on update cascade
        on delete no action,
    foreign key (card_number) references Customer_Card (card_number)
        on update cascade
        on delete no action
);

CREATE TABLE Store_Product
(
    UPC                 varchar(12) primary key,
    UPC_prom            varchar(12),
    id_product          int            not null,
    selling_price       decimal(13, 4) not null,
    products_number     int            not null,
    promotional_product bool           not null,
    foreign key (UPC_prom) references Store_Product (UPC)
        on update cascade
        on delete set null,
    foreign key (id_product) references Product (id_product)
        on update cascade
        on delete no action
);

CREATE TABLE Sale
(
    UPC            varchar(12),
    check_number   varchar(10),
    product_number int            not null,
    selling_price  decimal(13, 4) not null,
    primary key (UPC, check_number),
    foreign key (UPC) references Store_Product (UPC)
        on update cascade
        on delete no action,
    foreign key (check_number) references "Check" (check_number)
        on update cascade
        on delete cascade
)