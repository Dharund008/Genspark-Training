use shop;
/*
create table orderstatusmaster (o_statusID int primary key identity(1,1), message varchar(25) not null);

create table category ( CategoryID INT PRIMARY KEY, name VARCHAR(60), status Varchar(25) );

create table country (countryID INT Primary key, name Varchar(45));

create table state (stateID INT Primary Key, name Varchar(50), countryID Int, Foreign Key (countryID) References country(countryID));

create table city (cityID INT primary Key, name Varchar(60), stateID int, Foreign Key (stateID) References state(stateID));

create table area (zipcode INT primary key, name Varchar(50), cityID int, Foreign Key (cityID) References city(cityID));

create table address (addressID int primary key, door_number varchar(15), addressLine varchar(65), zipcode int, Foreign Key (zipcode) References area(zipcode));

create table supplier (supplierID int primary key, name varchar(25), contact_person varchar(65), phone varchar(20),email varchar(70), addressID int, status varchar(20), foreign key (addressID) references address(addressID));

create table product (productID int primary key, name varchar(30) not null, unit_price decimal(10,2),quantity int, description varchar(35), image varchar(85), categoryID int, foreign key (categoryID) references category(categoryID));

create table product_supplier (ps_id int primary key, productID int, supplierID int,date_of_supply DATE, quantity int, Foreign Key (productID) References product(productID), Foreign Key (supplierID) References supplier(supplierID));

create table customer (customerID int primary key, name varchar(50) not null, phone varchar(15),age int, addressID int,foreign key (addressID) references address(addressID));

create table orders (orderID int primary key, customerID int, date_of_order date, amount decimal(10,2),o_statusID int, foreign key (customerID) references customer(customerID), foreign key (o_statusID) references orderstatusmaster (o_statusID) );

create table order_details (detailID int primary key, orderID int not null, productID int not null, quantity int, unit_price decimal(10,2), foreign key (orderID) references orders(orderID), Foreign Key (productID) References product(productID) );
*/

/*
alter table order_details drop constraint PK__order_de__83077839AA4678F7; 

alter table order_details drop column detailID; 

alter table order_details add detailID int primary key identity(1,1);*/

