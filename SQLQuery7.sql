drop database LibraryDb

create database LibraryDb

use LibraryDb

create table Books
(BookId int primary key identity(101,2),
Title nvarchar(50) not null,
Author nvarchar(50) not null,
Genre nvarchar(50) not null,
Quantity int default(10)
)

insert into Books values ('Death on the Nile','Agatha Christie','Detective & Mystery', 25)
insert  into Books values ('HP and the Chamber of Secrets',' JK Rowling','Fantasy', 50)
insert into Books values ('Pride & Prejudice',' Jane Austin','Classics', 15)
insert into Books(Title,Author,Genre) values ('TFIOS','John Green','Romance')
insert into Books(Title,Author,Genre) values ('The Hunger Games','Suzanne Collins','Action & Adventure')

select * from Books

drop table Books