use pubs 
go

--May8 continuation:-
--storedprocedures with out paramater:
select * from products;

select * from products where try_cast(JSON_VALUE(details,'$.spec.CPU') as nvarchar(20)) = 'i7';

--in & out together
create or alter procedure --- as default all parameters are in, if out mention it.
proc_FilterProducts (@pcpu varchar(20), @pcount int out)
as
begin
	set @pcount = (select id from products where  --id/count(*)
	try_cast(JSON_VALUE(details,'$.spec.CPU') as nvarchar(20)) = @pcpu)
end

begin
declare @cnt int 
exec proc_FilterProducts 'i5', @cnt out
print concat('The number of computers is ',@cnt)
end


--stored_procedure : bulk_insert with filepath
create table people
(id int primary key,
name varchar(20),
age int)


create or alter proc
proc_BulkInsert(@filepath nvarchar(max))
as
begin
	declare @insertQuery nvarchar(max)
	
	set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	with(
	FIRSTROW=2,
	FIELDTERMINATOR='','',
	ROWTERMINATOR= ''\n'')' ---syntax for bulk insert using csv: BULK INSERT tablename FROM filepath with(...)
	exec sp_executesql @insertQuery  ---execute the insert query inside the sp
end
--- only for bulk insert without having status logs

exec proc_BulkInsert 'D:\Genspark-Training\May8\Storedprocedures-file_insertion.txt'

select * from people

truncate table people

create table BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())

create or alter procedure
proc_LogFiling(@filepath nvarchar(max))
as
begin 
---use try catch exception handling
	begin try
		declare @insertQuery nvarchar(max)
	
		set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
		with(
		FIRSTROW=2,
		FIELDTERMINATOR='','',
		ROWTERMINATOR= ''\n'')'
		exec sp_executesql @insertQuery     
							---	after successfully inserting, insert log into logstable
		
		insert into BulkInsertLog(filepath,status,message)
		values(@filepath,'Success','Bulk Insert completed')
	end try
	begin catch
		insert into BulkInsertLog(filepath,status,message)
		values(@filepath,'Failed',ERROR_MESSAGE())
	end catch

end

exec proc_LogFiling 'D:\Genspark-Training\May8\Storedprocedures-file_iserion.txt' -- no such file exists
exec proc_LogFiling 'D:\Genspark-Training\May8\Storedprocedures-file_isertion.txt' --will perform successfull insert

select * from people

select * from BulkInsertLog
