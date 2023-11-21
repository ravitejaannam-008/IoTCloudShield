create table User
(
    id       int auto_increment
        primary key,
    username varchar(100) null,
    name     varchar(100) null,
    email    varchar(100) null,
    password varchar(100) null
);