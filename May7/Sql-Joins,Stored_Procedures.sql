use pubs;
go


select * from titles;
select * from publishers;


select title,pub_name from titles t join publishers p ON t.pub_id = p.pub_id;

--print the publisher details of the publisher who has never published
select * from publishers where pub_id not in (select distinct pub_id from titles);

--rightouterjoin
select t.title,p.pub_id, pub_name from titles t right outer join publishers p ON t.pub_id = p.pub_id;

--select author id for all books. print the author_id and the book name.
select * from authors;
select * from titles;
--select title,authors.au_fname,authors.au_lname from titles join titleauthor On titles.title_id = titleauthor.title_id join authors on authors.au_id=titleauthor.au_id;
--concat(authors.au_fname ,' ',  authors.au_lname) as name
select title, au_id from titles join titleauthor on titles.title_id = titleauthor.title_id;


--select concat(au_fname,' ',au_lname) Author_name from authors;
select concat(au_fname,' ',au_lname) Author_name,title_id from authors a join titleauthor ta on a.au_id = ta.au_id order by 2;

select concat(au_fname,' ',au_lname) Author_name,title as book_name from authors a join titleauthor ta on a.au_id = ta.au_id 
join titles t on ta.title_id=t.title_id;


--print the pub name ,book name,and the order date of books
select pub_name, title, ord_date from titles 
join publishers on titles.pub_id = publishers.pub_id 
join sales on titles.title_id = sales.title_id;
--order by 3[asc/desc]: to more organising


--print the publisher name and the first book sale date for all the publishers [min for earlier , max for latest]
select pub_name,  MIN(ord_date) first_order_Date from titles 
join publishers on titles.pub_id = publishers.pub_id 
join sales on titles.title_id = sales.title_id
group by pub_name;

--need all publishers, even when they have not published it
select pub_name, MIN(ord_date) first_order_Date from publishers 
left outer join titles on publishers.pub_id = titles.pub_id 
left outer join sales on titles.title_id = sales.title_id
group by pub_name
order by 2 desc;

--print the bookname and the store address of the sale 
select title Book_name, stor_address Store_Address from titles
join sales on titles.title_id = sales.title_id
join stores on sales.stor_id = stores.stor_id
order by 1;	


--procedures

create procedure proc_FirstProcedure
as
begin
	print 'Hello world'
end

exec proc_FirstProcedure

create table Products
(id int identity(1,1) constraint pk_productId primary key,  --assigning a constraint name while creating the table itself.
name nvarchar(100) not null,
details nvarchar(max))
GO
--variables always starts with '@'
create procedure proc_InsertProduct(@pname nvarchar(100), @pdetails nvarchar(max))
as
begin
	insert into Products(name,details) values(@pname,@pdetails)
end
go

proc_InsertProduct 'Laptop','{"brand":"lenovo","spec":{"ram":"32GB","CPU":"i5"}}'
go
select * from Products;

select JSON_QUERY(details, '$.spec') Product_Specification from Products;
--takes two paramters: that respective col, what do u want from that col
--need to give an alias name
--add-hawk query

create procedure 
proc_UpdateProductSpec(@p_id int,@newvalue varchar(20))
as
begin
	update Products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @p_id
end

proc_UpdateProductSpec 1,'24GB'
select JSON_QUERY(details, '$.spec') Product_Specification from Products;

--another way of resulting it
select id, name, JSON_VALUE(details,'$.brand') Brand_Name from Products;
--here,can't able to print the spec entirely.




--bulk insert with store procedures
--how to bulk insert from json data
DECLARE @jsondata nvarchar(max) = '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  },
  {
    "userId": 1,
    "id": 3,
    "title": "ea molestias quasi exercitationem repellat qui ipsa sit aut",
    "body": "et iusto sed quo iure\nvoluptatem occaecati omnis eligendi aut ad\nvoluptatem doloribus vel accusantium quis pariatur\nmolestiae porro eius odio et labore et velit aut"
  }

]';

create table Posts
(id int primary key,
title nvarchar(100),
user_id int,
body nvarchar(max)
);	


insert into Posts(user_id,id,title,body)
select userId,id,title,body 
from openjson(@jsondata)
with (userId int,id int, title varchar(100), body varchar(max));
--always print the jsondata with the insert statements together!....

select * from posts

--now lets do this with procedure
delete from posts;

create or alter procedure
proc_BulkInsert(@jsondata nvarchar(max))
as
begin
	insert into Posts(user_id,id,title,body)
	select userId,id,title,body 
	from openjson(@jsondata)
with (userId int,id int, title varchar(100), body varchar(max));
end

proc_BulkInsert '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  },
  {
    "userId": 1,
    "id": 3,
    "title": "ea molestias quasi exercitationem repellat qui ipsa sit aut",
    "body": "et iusto sed quo iure\nvoluptatem occaecati omnis eligendi aut ad\nvoluptatem doloribus vel accusantium quis pariatur\nmolestiae porro eius odio et labore et velit aut"
  }

]'

select * from posts




--trycast
select * from products where JSON_VALUE(details,'$.spec.CPU') = 'i7';
select * from products where try_cast(JSON_VALUE(details,'$.spec.CPU') as nvarchar(20)) = 'i7';


  --create a procedure that brings post by taking the user_id as parameter
  create or alter procedure
  proc_GetPostId(@p_id int)
  as
  begin
	select * from Posts where id =@p_id;
  end

  proc_GetPostId 1;

