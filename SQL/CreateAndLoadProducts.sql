create table Product
(
    id       int auto_increment
        primary key,
    title    varchar(50) null,
    price    double      null,
    quantity int         null
);

insert into Product (title, price, quantity) values ('Monitor', 200, 10);
insert into Product (title, price, quantity) values ('Computer', 1200, 10);
insert into Product (title, price, quantity) values ('Keyboard', 20, 10);
insert into Product (title, price, quantity) values ('Mouse', 10, 10);