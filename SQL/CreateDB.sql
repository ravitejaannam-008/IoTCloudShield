## Create the Database
CREATE SCHEMA Store;

## Create a user
CREATE USER 'store'@'localhost' IDENTIFIED BY 'password';

## Grant rights to the user to access the database
GRANT ALL PRIVILEGES ON *.* TO 'store'@'localhost' WITH GRANT OPTION;
