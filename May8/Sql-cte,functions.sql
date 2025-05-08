use pubs
go


--cte
--- whatever we write within cte will be stored as object which can be usedn in the further queries
--all col should have headings

sp_help authors


---example
with cteAuthors --cte_name
as
(
select au_id,concat(au_fname,' ',au_lname) as author_name from authors
)

select * from cteAuthors

--pagination
--rownumber :predefined function -identify unique row numbers for the value

--example
declare @page int=2,@pageSize int =10;
with PaginatedBooks as
(select title_id,title,price, ROW_NUMBER() over (order by price desc) as RowNum
from titles)

--select * from paginatedbooks;
--now acting it like as an page
select * from paginatedbooks where RowNum between ((@page-1)*@pageSize) and (@page * @pageSize); --returns pages from 1 to 10 

select * from paginatedbooks where RowNum between ((@page-1)*@pageSize) and (@page * @pageSize); --returns next set of pages 10 to 20...

--create a sp that will take the page number and size as param and print the books
create or alter procedure
proc_Paginationsp(@page int =1,@pageSize int = 10)
as
begin
	with PaginatedBooks as
	(select title_id,title,price, ROW_NUMBER() over (order by price desc) as RowNum
	from titles)
	select * from PaginatedBooks where rowNUm between((@page-1)*(@pageSize+1)) and (@page*@pageSize) --pagesize+1 must
end


exec proc_Paginationsp 2,5



--offset : advanced way of fetching data
 select  title_id,title, price
  from titles
  order by price desc
  offset 10 rows fetch next 10 rows only --syntax: offset (no.of rows to skip) rows fetch next (no.of rows to display) rows only....

  --need to look into it



  --functions
  --table value functions .. return tables / single value
  --must return a value
  --not mandatory here to use begin/end for table valued function


create or alter function fn_calculateprice(@baseprice float,@tax float)
returns float
as
begin
	return (@baseprice + (@baseprice*@tax/100))
end

select dbo.fn_calculateprice(1000,10)--dbo : database owner : syntac for calling function

select pubs.fn_calculateprice(1000,10) ---not valid
  --scalar function : a function that just returns a single value
  --considers as a col by select.

 select title,dbo.fn_calculateprice(price,12) from titles --can also use function like this
  

--table valued function: 

create function fn_tablesample(@minprice float)
returns table
as
	return select title,price from titles where price >= @minprice

select * from dbo.fn_tablesample(10)
--returns table by itself

--older and slower but supports more logic - table value function
create function fn_tableSampleOld(@minprice float)
  returns @Result table(Book_Name nvarchar(100), price float)
  as
  begin
    insert into @Result select title,price from titles where price>= @minprice
    return 
end

select * from dbo.fn_tableSampleOld(10)