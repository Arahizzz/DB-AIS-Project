CREATE TABLE Category(
  category_number serial primary key,
  category_name varchar(50) 
);

CREATE TABLE Product (
  id_product serial primary key,
  category_number int not null,
  product_name varchar(50) not null ,
  characteristics varchar(100) not null,
  foreign key (category_number) references Category(category_number)
                     on update cascade 
                     on delete no action 
);